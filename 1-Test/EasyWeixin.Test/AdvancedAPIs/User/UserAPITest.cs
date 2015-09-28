using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EasyWeixin.CommonAPIs;
using System.Linq;
using EasyWeixin.AdvancedAPIs.User;
using EasyWeixin.AdvancedAPIs.TemplateMessage;
using EasyWeixin.AdvancedAPIs.TemplateMessage.TemplateMessageJson;

namespace EasyWeixin.Test.AdvancedAPIs.User
{
    /// <summary>
    /// UserListTest 的摘要说明
    /// </summary>
    [TestClass]
    public class UserAPITest
    {
        public UserAPITest()
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
        public void TestGetUserList()
        {
            int errorNum = 0;
            foreach (var item in WeixinData.WeixinList.Take(1))
            {
                var token = AccessTokenContainer.TryGetToken(item.AppId, item.AppSecret, true);
                var result = new List<string>();
                GetResult(token, ref result);
                string templateId = "4YF7LN1TbpHA9VPNxDlOkiCS26hzSZDGguKxTfjBAd4";

                //获取了之后就可以发送模板消息了
                foreach (var openid in result)
                {
                    try
                    {
                        var testData = new
                        {
                            first = new TemplateDataItem("123456"),
                            keyword1 = new TemplateDataItem(DateTime.Now.ToString()),
                            keyword2 = new TemplateDataItem("123456"),
                        };

                        TemplateApi.SendTemplateMessage(token, openid, templateId, "red",
                            "http://www.baidu.com", testData);
                    }
                    catch
                    {
                        errorNum++;
                        continue;
                    }
                }
            }


        }

        private void GetResult(string token, ref List<string> result, string next_openid = "")
        {
            var resultJson = UserApi.Get(token, next_openid);
            result.AddRange(resultJson.data.openid);
            if (resultJson.count == 10000)
            {
                GetResult(token, ref result, resultJson.next_openid);
            }
        }
    }


}
namespace EasyWeixin.Test
{
    public class WeixinData
    {
        public static List<WeixinData> WeixinList = new List<WeixinData>
        {
            new WeixinData{Name="深圳互动力",AppId="wx00f1a2e4c8da9fff",AppSecret="d0cecf2fa0b11a67077a4cbeb4c127ab"},
            new WeixinData{Name="suibianapp",AppId="wxb903604f5a7ba8fe",AppSecret="1a3397bf73fb85e3aee28c6972cafe78"},
            new WeixinData{Name="",AppId="wxe34085e3c35cb235",AppSecret="fb684ff3ed78b9141c21614982052e2e"},
            new WeixinData{Name="",AppId="wxb0191ee68408a19f",AppSecret="a6e824782c12eb9279863f3af18d"},
            new WeixinData{Name="",AppId="wx45ec07794f9a07e5",AppSecret="addfac9f0fb61a0cb46de9dbd07434e"},
            new WeixinData{Name="",AppId="wx3ef6dddda11055ea",AppSecret="a885ef98ab889de9d125e98cb77c55e4"},
            new WeixinData{Name="",AppId="wx882e85c786f016b2",AppSecret="106addef0a82e301860d421c62d8db74"},
            new WeixinData{Name="",AppId="wx87624ed5e94177eb",AppSecret="80d3be6b365fff8563cadd0f4620562a"},
            new WeixinData{Name="",AppId="wxf14e555322b60b2a",AppSecret="ce183f5e3b0b0e38eb247dba4b0f6e3"},
            new WeixinData{Name="",AppId="wx001dfd8677200929",AppSecret="dda3ddbd9ed7ee7a49559fb817f49d04"},
            new WeixinData{Name="",AppId="wxb903604f5a7ba8fe",AppSecret="a3397bf73fb85e3aee28c6972cafe78"},
            new WeixinData{Name="",AppId="wx246f094dc95b2d85",AppSecret="cb4189112fe837a5285ebeb975f4fd0a"},
            new WeixinData{Name="",AppId="wxc44b07c834a672ff",AppSecret="530a0242379e752c1ac2cc9c3880ce1a"},
            new WeixinData{Name="",AppId="wx7e71184dd1c98cd62",AppSecret="81f9759a1d515dcc9da7b94fdccd7a372"},
        };
        public string Name { get; set; }
        public string AppId { get; set; }
        public string AppSecret { get; set; }
    }
}
