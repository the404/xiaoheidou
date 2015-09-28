using EasyWeixin.CommonAPIs;
using EasyWeixin.Data.Repositories;
using EasyWeixin.Entities.JsonResult;
using EasyWeixin.Model;
using EasyWeixin.Web.Framework.Controllers;
using EasyWeixin.Web.Models;
using System;
using System.IO;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace EasyWeixin.Web.Controllers
{
    public class SendMessageController : BaseApiController
    {
        private readonly IUserProfileRepository UserProfileRepository;
        private readonly IPayCustomerRepository PayCustomerRepository;

        public SendMessageController(IUserProfileRepository UserProfileRepository,
            IPayCustomerRepository PayCustomerRepository)
        {
            this.UserProfileRepository = UserProfileRepository;
            this.PayCustomerRepository = PayCustomerRepository;
        }

        public JsonResult GetCustomerMsg(string openId, string msg, string userPhone)
        {
            var messg = "";
            ///获取用户数据
            //var user = UserProfileRepository.Find(Specification<EasyWeixin.Model.UserProfile>.Eval(o => o.UserId == WebSecurity.CurrentUserId));

            string token = AccessTokenContainer.TryGetToken("wxc44b07c834a672ff", "530a0242379e752c1ac2cc9c3880ce1a");

            SendMsg sm = new SendMsg();
            sm.touser = openId;
            sm.msgtype = "text";

            TextMsg tm = new TextMsg();
            tm.content = Server.UrlDecode(msg);
            sm.text = tm;

            //记录日志，写入记事本
            string path = System.Web.HttpContext.Current.Server.MapPath("/txt/");
            string ss = "aa.txt";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            StreamWriter sw = new StreamWriter(path + ss, true, Encoding.UTF8);
            sw.WriteLine("游客OpenId:" + sm.touser + "|支付信息:" + tm.content);
            sw.Close();

            JavaScriptSerializer js = new JavaScriptSerializer();
            string jsonString = js.Serialize(sm);
            WxJsonResult wx = CommonApi.SendCustomerMsg(token, jsonString);

            WeixinUserInfoResult wu = CommonApi.GetUserInfo(token, openId);

            if (wx.errcode == 0)
            {
                //插入支付成功并且发送完客服消息后的用户信息
                //支付成功用户表字段：手机号，支付信息，是否抽奖，支付时间，抽奖时间，奖品，openid
                PayCustomer watch = new PayCustomer();
                watch.Award = "";
                watch.IsAward = 0;
                watch.OpendId = openId;
                watch.PayDate = null; //抽奖时间
                watch.PayInfo = Server.UrlDecode(msg);
                watch.UPhone = userPhone;
                watch.CreateDate = DateTime.Now;
                watch.nickname = wu.nickname;
                watch.headUrl = wu.headimgurl;
                PayCustomerRepository.Add(watch);
                PayCustomerRepository.Context.Commit();
                messg = "发送客服消息成功!";
            }
            else
            {
                messg = "发送客服消息失败!";
            }

            return Json(new { success = messg }, JsonRequestBehavior.AllowGet);
        }
    }
}