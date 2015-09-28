using Apworks.Repositories.EntityFramework;
using EasyWeixin.Data;
using EasyWeixin.Data.Repositories;
using EasyWeixin.Model;
using EasyWeixin.Web.Framework.CommonService.CustomMessageHandler;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace EasyWeixin.Web.Api.Tests
{
    [TestClass]
    public class UnitTest1
    {
        private IUserProfileRepository _userRepo;
        private IQrCodeRepository _qrCodeRepo;
        private UserProfile _userProfile;
        private IUnityContainer _container = Bootstrapper.Initialise();
        //first:构造函数
        //seconed:testInintialize
        //thried:testMethod

        public UnitTest1()
        {
            _userRepo = _container.Resolve(typeof(UserProfileRepository)) as IUserProfileRepository;
            _qrCodeRepo = _container.Resolve(typeof(QrCodeRepository)) as IQrCodeRepository;
            _userProfile = _userRepo.FindAll().Single(s => s.UserName == "ipow");
        }

        [TestInitialize]
        public void Start()
        {
        }

        [TestMethod]
        public void TestMethod1()
        {
            var str = @"<xml>
                            <ToUserName><![CDATA[gh_38f4ef5d3064]]></ToUserName>
                            <FromUserName><![CDATA[ocBLpjvEkE9-ml35Cl8lvi6hanYs]]></FromUserName>
                            <CreateTime>1432173685</CreateTime>
                            <MsgType><![CDATA[event]]></MsgType>
                            <Event><![CDATA[SCAN]]></Event>
                            <EventKey><![CDATA[141]]></EventKey>
                            <Ticket><![CDATA[gQHl7zoAAAAAAAAAASxodHRwOi8vd2VpeGluLnFxLmNvbS9xL3FIV212TmpsUUc0NVh3U2JuMXUwAAIEGDtdVQMEgDoJAA==]]></Ticket>
                        </xml>";

            UnicodeEncoding uniEncoding = new UnicodeEncoding();
            byte[] bytes = uniEncoding.GetBytes(str);
            using (Stream stream = new MemoryStream(bytes))
            {
                CustomMessageHandler messageHandler = new CustomMessageHandler(
                    stream, _userProfile, _qrCodeRepo);
                messageHandler.Execute();
            };
        }

        [TestMethod]
        public void TestMethod2()
        {
            //var setting = false;
            //ConfigurationManager.AppSettings["IsWriteLog"] = setting.ToString();
            var islog = ConfigurationManager.AppSettings["IsWriteLog"];
            var flag = true;
            var result = Boolean.TryParse(islog, out flag);
            Assert.AreEqual(false, flag);
            Assert.IsNull(islog);
            Assert.AreEqual(false, result);
        }
    }
}