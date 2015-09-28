using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EasyWeixin.AdvancedAPIs.RedPack;
using EasyWeixin.AdvancedAPIs.RedPack.impl;
using EasyWeixin.AdvancedAPIs.RedPack.inter;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Diagnostics;

namespace EasyWeixin.Test.AdvancedAPIs.RedPack
{
    [TestClass]
    public class RedPackServiceTest
    {
        [TestMethod]
        public void TestSendAsync()
        {
            var appId = "wx00f1a2e4c8da9fff";
            var mchId = "1238660702";
            var payKey = "DFB29A32C88F4BA9A30A33186681ADB2";

            var billNo = mchId + DateTime.Now.ToString("yyyyMMdd") + DateTime.Now.ToString("ddHHmmssff");

            var openId = "ocBLpjvEkE9-ml35Cl8lvi6hanYs";
            var ipAddress = "127.0.0.1";
            var actName = "测试";
            var amount = Amounter.GetAmount(3105, 3500);
            var remark = "备注";
            var sendName = "商户名称";
            var wishing = "祝福语";

            var certPath = AppDomain.CurrentDomain.BaseDirectory.Replace(@"bin\Debug", "").Replace(@"bin\Obj", "").
                Replace(@"bin\debug", "").Replace(@"bin\obj", "") + @"cert\apiclient_cert.p12";
            var certPass = "1238660702";
            ICertFinder certFinder = new CertFindByFile(certPath, certPass);
            ISendCert sendCert = new SendCert(certFinder);

            var service = new RedPacketService(mchId, payKey, sendCert);

            var redPacket = new RedPacket
            {
                ActName = actName,
                Amount = amount,
                AppId = appId,
                BillNumber = billNo,
                IpAddress = ipAddress,
                OpenId = openId,
                Remark = remark,
                SendName = sendName,
                Wishing = wishing
            };
            var result = service.SendAsync(redPacket).Result;
            Assert.AreNotEqual(result.Succeeded, false);
        }

        [TestMethod]
        public void TestSend()
        {
            var appId = "wx00f1a2e4c8da9fff";
            var mchId = "1238660702";
            var payKey = "DFB29A32C88F4BA9A30A33186681ADB2";

            var billNo = mchId + DateTime.Now.ToString("yyyyMMdd") + DateTime.Now.ToString("ddHHmmssff");

            var openId = "ocBLpjvEkE9-ml35Cl8lvi6hanYs";
            var ipAddress = "127.0.0.1";
            var actName = "测试";
            var amount = Amounter.GetAmount(100, 1000);
            var remark = "备注";
            var sendName = "商户名称";
            var wishing = "祝福语";

            var certPath = AppDomain.CurrentDomain.BaseDirectory.Replace(@"bin\Debug", "").Replace(@"bin\Obj", "").
                Replace(@"bin\debug", "").Replace(@"bin\obj", "") + @"cert\apiclient_cert.p12";
            var certPass = "1238660702";
            ICertFinder certFinder = new CertFindByFile(certPath, certPass);
            ISendCert sendCert = new SendCert(certFinder);

            var service = new RedPacketService(mchId, payKey, sendCert);

            var redPacket = new RedPacket
            {
                ActName = actName,
                Amount = amount,
                AppId = appId,
                BillNumber = billNo,
                IpAddress = ipAddress,
                OpenId = openId,
                Remark = remark,
                SendName = sendName,
                Wishing = wishing
            };
            var result = service.Send(redPacket);
            Trace.WriteLine(JsonConvert.SerializeObject(result));
            Assert.AreNotEqual(result.Succeeded, false);
        }
        [TestMethod]
        public void TestSendByHttpWebRequest()
        {
            var appId = "wx00f1a2e4c8da9fff";
            var mchId = "1238660702";
            var payKey = "DFB29A32C88F4BA9A30A33186681ADB2";

            var billNo = mchId + DateTime.Now.ToString("yyyyMMdd") + DateTime.Now.ToString("ddHHmmssff");

            var openId = "ocBLpjvEkE9-ml35Cl8lvi6hanYs";
            var ipAddress = "127.0.0.1";
            var actName = "测试";
            var amount = Amounter.GetAmount(100, 1000);
            var remark = "备注";
            var sendName = "商户名称";
            var wishing = "祝福语";

            var certPath = AppDomain.CurrentDomain.BaseDirectory.Replace(@"bin\Debug", "").Replace(@"bin\Obj", "").
                Replace(@"bin\debug", "").Replace(@"bin\obj", "") + @"cert\apiclient_cert.p12";
            var certPass = "1238660702";
            ICertFinder certFinder = new CertFindByFile(certPath, certPass);
            ISendCert sendCert = new SendCert(certFinder);

            var service = new RedPacketService(mchId, payKey, sendCert);

            var redPacket = new RedPacket
            {
                ActName = actName,
                Amount = amount,
                AppId = appId,
                BillNumber = billNo,
                IpAddress = ipAddress,
                OpenId = openId,
                Remark = remark,
                SendName = sendName,
                Wishing = wishing
            };
            var result = service.SendByHttpWebRequest(redPacket);
            Trace.WriteLine(JsonConvert.SerializeObject(result));
            Assert.AreNotEqual(result.Succeeded, false);
        }
    }
}
