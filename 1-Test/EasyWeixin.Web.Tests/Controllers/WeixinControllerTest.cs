using Apworks.Repositories.EntityFramework;
using EasyWeixin.Data;
using EasyWeixin.Data.Repositories;
using EasyWeixin.Helpers;
using EasyWeixin.MvcExtension.Results;
using EasyWeixin.Web.Controllers;
using EasyWeixin.Web.Tests.Mock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;


namespace EasyWeixin.Web.Tests.Controllers
{
    [TestClass]
    public class WeixinControllerTest : BaseTest
    {


        WeixinController target = null;
        Stream inputStream;

        string xmlTextFormat = @"<xml>
                                    <ToUserName><![CDATA[gh_a96a4a619366]]></ToUserName>
                                    <FromUserName><![CDATA[olPjZjsXuQPJoV0HlruZkNzKc91E]]></FromUserName>
                                    <CreateTime>{0}</CreateTime>
                                    <MsgType><![CDATA[text]]></MsgType>
                                    <Content><![CDATA[TNT2]]></Content>
                                    <MsgId>5832509444155992350</MsgId>
                                </xml>
";

        string xmlLocationFormat = @"<xml>
                                      <ToUserName><![CDATA[gh_a96a4a619366]]></ToUserName>
                                      <FromUserName><![CDATA[olPjZjsXuQPJoV0HlruZkNzKc91E]]></FromUserName>
                                      <CreateTime>{0}</CreateTime>
                                      <MsgType><![CDATA[location]]></MsgType>
                                      <Location_X>31.285774</Location_X>
                                      <Location_Y>120.597610</Location_Y>
                                      <Scale>19</Scale>
                                      <Label><![CDATA[中国江苏省苏州市沧浪区桐泾南路251号-309号]]></Label>
                                      <MsgId>5832828233808572154</MsgId>
                                    </xml>";
        string xmlEVENT = @"<xml><ToUserName><![CDATA[toUser]]></ToUserName>
                            <FromUserName><![CDATA[FromUser]]></FromUserName>
                            <CreateTime>123456789</CreateTime>
                            <MsgType><![CDATA[event]]></MsgType>
                            <Event><![CDATA[CLICK]]></Event>
                            <EventKey><![CDATA[SubClickRoot_Text]]></EventKey>
                            </xml>";

        private string xmlScan = @"<xml>
                                    <ToUserName><![CDATA[gh_38f4ef5d3064]]></ToUserName>
                                    <FromUserName><![CDATA[ocBLpjvEkE9-ml35Cl8lvi6hanYs]]></FromUserName>
                                    <CreateTime>1431507841</CreateTime>
                                    <MsgType><![CDATA[event]]></MsgType>
                                    <Event><![CDATA[SCAN]]></Event>
                                    <EventKey><![CDATA[4294967295]]></EventKey>
                                    <Ticket><![CDATA[gQE38ToAAAAAAAAAASxodHRwOi8vd2VpeGluLnFxLmNvbS9xL0RYWE9LaVhsSVc1WWthRUU5MXUwAAIEkgtTVQMEgDoJAA==]]></Ticket>
                                    <Encrypt><![CDATA[bGHfLmY7FmnmRVSUsPYc/rqnsybT94BKUA7Xpv02TjTU5Cnfsb0Zhs0HCu/fsdGQO76MaNLZNKwMq/RhL+cqwqAMFKF+oA3J+jBICLRtlktGlMMVuaPp7nV7Uu8gR7PUt4P1EqIkTMtA6zH89wMoy6urKJLrmYEJxWWN+ddTTiZNW4xb/X3nbCvJhY7DZLZrU1r5I9QiDexxGDkXxGBZoSxMkwc3XUfh2v4aEQJ4bB+Z9XGYBqxqOCn5dszluDUcfnPWUUZq16eerM3cI8d0gq4nG90Gnbq9qwYt3v5MntZXiSX1uR6NJ3ckJeoPxxFTUIvsoro8BkyhnGM0wtC12GB+ywVOlt8B6DZkFXo3psoNw90EzwFPQXLNhGj4+j0njOo2GB8GKxa5+rBmQb/Yl+Kb3DoWw74h0nC3oYrTwFYJOZ7UB6HIsNi6BiAC2oKpI5BT2mHBAFrwWoIDGgWHExc1QvZ0TvtSquT4uwKnO7AU4SGJCASMYjSnd7r0tjVHJqUz5/d23O36rC7dRPSHUwYhswfjDsQFxa+otNXT+IWiI+lVw8/qJgyEBOs6oe7SMXVSuCK87xoAVjnqd0NMIg==]]></Encrypt>
                                </xml>";
        /// <summary>
        /// 初始化控制器及相关请求参数
        /// </summary>
        /// <param name="xmlFormat"></param>
        private void Init(string xmlFormat)
        {
            // target = StructureMap.ObjectFactory.GetInstance<WeixinController>();//使用IoC的在这里必须注入，不要直接实例化

            //target = new WeixinController(new UserProfileRepository(new EntityFrameworkRepositoryContext(new EasyWeixinDbContext())));

            inputStream = new MemoryStream();
            var xml = string.Format(xmlFormat, DateTimeHelper.GetWeixinDateTime(DateTime.Now));
            var bytes = System.Text.Encoding.UTF8.GetBytes(xml);
            inputStream.Write(bytes, 0, bytes.Length);
            inputStream.Flush();
            inputStream.Seek(0, SeekOrigin.Begin);
            target.SetFakeControllerContext(inputStream);
        }

        /// <summary>
        /// 测试不同类型的请求
        /// </summary>
        /// <param name="xml">微信发过来的xml原文</param>
        private void PostTest(string xml)
        {
            Init(xml);//初始化

            var timestamp = "itsafaketimestamp";
            var nonce = "whateveryouwant";
            var signature = EasyWeixin.CheckSignature.GetSignature(timestamp, nonce, "wx");

            DateTime st = DateTime.Now;
            //这里使用MiniPost，绕过日志记录
            var actual = target.MiniPost("10000", signature, timestamp, nonce, "echostr") as WeixinResult;
            DateTime et = DateTime.Now;

            Assert.IsNotNull(actual);
            Assert.IsNotNull(actual.Content);

            Console.WriteLine(actual.Content);
            Console.WriteLine("页面用时（ms）：" + (et - st).TotalMilliseconds);
        }

        [TestMethod]
        public void TextPostTest()
        {

            PostTest(xmlTextFormat);
        }

        [TestMethod]
        public void LocationPostTest()
        {
            PostTest(xmlLocationFormat);
        }

        [TestMethod]
        public void EVENTPostTest()
        {
            PostTest(xmlEVENT);
        }

        [TestMethod]
        public void ScanPostTest()
        {
            PostTest(xmlScan);
        }
    }

    public class BaseTest
    {
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

    }
}