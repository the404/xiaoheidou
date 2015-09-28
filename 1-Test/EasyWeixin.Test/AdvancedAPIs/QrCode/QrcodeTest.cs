using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EasyWeixin.CommonAPIs;
using EasyWeixin.AdvancedAPIs.QrCode;

namespace EasyWeixin.Test.AdvancedAPIs.QrCode
{
    [TestClass]
    public class QrcodeTest
    {
        string appId = "wx00f1a2e4c8da9fff";
        string appsecrct = "d0cecf2fa0b11a67077a4cbeb4c127ab";
        [TestMethod]
        public void TestMethod1()
        {
            var token = AccessTokenContainer.TryGetToken(appId, appsecrct);
            var qrResult = QrCodeApi.Create(token, "1", 800);
            var url = QrCodeApi.GetShowQrCodeUrl(qrResult.ticket);
            Assert.IsNotNull(url);
        }
        [TestMethod]
        public void TestMaxNum()
        {
            //验证可以使用32位的string来创建临时二维码
            var token = AccessTokenContainer.TryGetToken(appId, appsecrct);
            var num = DateTime.Now.ToString("yyyyMMddhhmmssffff").PadLeft(32, 'a');
            var qrResult = QrCodeApi.Create(token, num, 800);
            var url = QrCodeApi.GetShowQrCodeUrl(qrResult.ticket);
            Assert.IsNotNull(url);
        }
        [TestMethod]
        public void TestYongjiu()
        {
            var token = AccessTokenContainer.TryGetToken(appId, appsecrct);
            var num = 100000.ToString();
            var qrResult = QrCodeApi.Create(token, num, 0);
            var url = QrCodeApi.GetShowQrCodeUrl(qrResult.ticket);
            Assert.IsNotNull(url);
        }
        [TestMethod]
        [ExpectedException(typeof(EasyWeixin.Exceptions.ErrorJsonResultException))]
        public void TestYongjiuThrowEx()
        {
            //验证不能草果100000
            var token = AccessTokenContainer.TryGetToken(appId, appsecrct);
            var num = 100001.ToString();
            var qrResult = QrCodeApi.Create(token, num, 0);
        }
        [TestMethod]
        public void TestYongjiuEquals()
        {
            var token = AccessTokenContainer.TryGetToken(appId, appsecrct);
            var num = 99.ToString();
            var qrResult = QrCodeApi.Create(token, num, 0);
            var url = QrCodeApi.GetShowQrCodeUrl(qrResult.ticket);
            var qrResult2 = QrCodeApi.Create(token, num, 0);
            var url2 = QrCodeApi.GetShowQrCodeUrl(qrResult2.ticket);

            Assert.AreEqual(url, url2);
        }

    }
}
