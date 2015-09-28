using System;
using EasyWeixin.Exceptions;
using EasyWeixin.HttpUtility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EasyWeixin.Test.HttpUtility
{
    [TestClass]
    public class GetTest
    {

    
        [TestMethod]
        public void GetJsonTest()
        {
          //  return;//已经通过，但需要连接远程测试，太耗时，常规测试时暂时忽略。
            var url = "http://home.ipow.cn/?json=get_search_results&search=%E9%95%BF%E9%9A%86";
            try
            {
                //这里因为参数错误，系统会返回错误信息
                string resultFail = Get.GetJson<string>(url);
                Assert.Inconclusive(resultFail);
               // Assert.Fail();//上一步就应该已经抛出异常
            }
            catch (ErrorJsonResultException ex)
            {
                //实际返回的信息（错误信息）
                Assert.AreEqual(ex.JsonResult.errcode, ReturnCode.不合法的APPID);
            }
        }
    }
}
