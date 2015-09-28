using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Net;
using EasyWeixin.Entities.JsonResult;
using EasyWeixin.Helpers;
using EasyWeixin.HttpUtility;
using Senparc.Weixin.MP.Entities;
using EasyWeixin.CommonAPIs;
namespace EasyWeixin.Test.CommonAPIs
{
    [TestClass]
    public class CommonApiTest
    {
        [TestMethod]
        public void GetUserInfoTest()
        {
            string token = AccessTokenContainer.TryGetToken("wx00f1a2e4c8da9fff", "d0cecf2fa0b11a67077a4cbeb4c127ab");
            WeixinUserInfoResult WeixinUserInfoResult = CommonApi.GetUserInfo(token, "olPjZjsXuQPJoV0HlruZkNzKc91E");

            Assert.IsTrue(WeixinUserInfoResult != null);
        }
        [TestMethod]
        public void UploadMediaFileTest()
        {


        }
    }
}
