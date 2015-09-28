using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EasyWeixin.Entities.JsonResult;
using EasyWeixin.Entities.Menu;
using EasyWeixin.Exceptions;
using EasyWeixin.HttpUtility;
using Senparc.Weixin.MP.Entities;
using EasyWeixin.Data.Repositories;
using EasyWeixin.Model;
using Apworks.Specifications;
using EasyWeixin.Web.Models;
using System.Data;
using System.Data.Entity;
using System.Linq;
using EasyWeixin.CommonAPIs;
using EasyWeixin.Web.Controllers;
namespace EasyWeixin.Test.CommonAPIs
{
    [TestClass]
    public class CommonApiMenuTest
    {
        [TestMethod]
        public void CreateMenuTest()
        {
            ButtonGroup bg = new ButtonGroup();
            var list = new List<Button>();
            for (int i = 1; i < 4; i++)
            {
                Button b = new Button();
                b.name = i.ToString();
                list.Add(b);
            }
            foreach (var item in list)
            {
                SingleClickButton bb = new SingleClickButton();
                bb.name = item.name;
                bb.key = "click";
                bg.button.Add(bb);
            }

            string token = AccessTokenContainer.TryGetToken("wx00f1a2e4c8da9fff", "d0cecf2fa0b11a67077a4cbeb4c127ab");
            WxJsonResult wx = CommonApi.CreateMenu(token, bg);
            Assert.IsTrue(wx != null);

        }

        [TestMethod]
        public void GetSingleButtonFromJsonObjectTest()
        {


        }
        [TestMethod]
        public void GetMenuTest()
        {
            string token = AccessTokenContainer.TryGetToken("wx00f1a2e4c8da9fff", "d0cecf2fa0b11a67077a4cbeb4c127ab");

            GetMenuResult R = CommonApi.GetMenu(token);
            Assert.IsTrue(R != null);

        }
        [TestMethod]
        public void GetMenuFromJsonTest()
        {


        }
        [TestMethod]
        public void DeleteMenuTest()
        {
            string token = AccessTokenContainer.TryGetToken("wx00f1a2e4c8da9fff", "d0cecf2fa0b11a67077a4cbeb4c127ab");

            WxJsonResult wx = CommonApi.DeleteMenu(token);
           Assert.IsTrue(wx != null);
        }

    }
}
