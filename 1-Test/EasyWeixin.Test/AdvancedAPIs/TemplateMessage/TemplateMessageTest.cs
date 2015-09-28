using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EasyWeixin.AdvancedAPIs.TemplateMessage;
using EasyWeixin.CommonAPIs;

namespace EasyWeixin.Test.AdvancedAPIs.TemplateMessage
{
    /// <summary>
    /// TemplateMessage 的摘要说明
    /// </summary>
    [TestClass]
    public class TemplateMessageTest
    {
        public TemplateMessageTest()
        {
            //
            //TODO:  在此处添加构造函数逻辑
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，该上下文提供
        ///有关当前测试运行及其功能的信息。
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

        #region 附加测试特性
        //
        // 编写测试时，可以使用以下附加特性: 
        //
        // 在运行类中的第一个测试之前使用 ClassInitialize 运行代码
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // 在类中的所有测试都已运行之后使用 ClassCleanup 运行代码
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 在运行每个测试之前，使用 TestInitialize 来运行代码
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 在每个测试运行完之后，使用 TestCleanup 来运行代码
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestSend()
        {
            try
            {
                string appId = "wx00f1a2e4c8da9fff";
                string appsecrct = "d0cecf2fa0b11a67077a4cbeb4c127ab";
                string openId = "需要正确的openId,但是好像一段时间之后会修改这个openId,这是为什么呢";
                string templateId = "4YF7LN1TbpHA9VPNxDlOkiCS26hzSZDGguKxTfjBAd4";
                var token = AccessTokenContainer.TryGetToken(appId, appsecrct);
                TemplateApi.SendTemplateMessage(token, openId, templateId, "red", "http://www.baidu.com", null);
            }
            catch (EasyWeixin.Exceptions.ErrorJsonResultException e)
            {
                //给出一些提示,写日志
                throw e;
            }
            catch (Exception)
            {
                //写下错误日志啊
            }
            finally
            {

            }
        }
    }
}
