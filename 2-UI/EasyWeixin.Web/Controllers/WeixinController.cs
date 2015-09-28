using Apworks.Repositories.EntityFramework;
using Apworks.Specifications;
using AttributeRouting.Web.Mvc;
using EasyWeixin.Data;
using EasyWeixin.Data.Repositories;
using EasyWeixin.Helpers;
using EasyWeixin.Model;
using EasyWeixin.Web.Framework.CommonService.CustomMessageHandler;
using EasyWeixin.Web.Framework.Controllers;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace EasyWeixin.Web.Controllers
{
    [AllowAnonymous]
    public class WeixinController : BaseApiController
    {
        private volatile bool _postingFlag = false;
        private object _postingLock = new object();
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IQrCodeRepository _qrCodeRepository;

        public WeixinController(IUserProfileRepository userProfileRepository,
            IQrCodeRepository qrCodeRepository)
        {
            _userProfileRepository = userProfileRepository;
            _qrCodeRepository = qrCodeRepository;
        }

        #region GET

        /// <summary>
        /// 开发者首次提交验证请求时,微信服务器将发送GET请求到填写的URL上,并且带上4个参数(Signnture,timestamp,nonce,echostr)
        /// </summary>
        /// <param name="id">用于找到系统中的用户</param>
        /// <param name="signature">签名</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">随机数</param>
        /// <param name="echostr">随机字符串</param>
        /// <returns></returns>
        [GET("/weixinapi/{id}")]
        public ContentResult Get(string id, string signature, string timestamp, string nonce, string echostr)
        {
            UserProfile userProfile = _userProfileRepository.Find(Specification<UserProfile>.Eval(e => e.UserCode == id));

            if (userProfile != null)
            {
                if (CheckSignature.Check(signature, timestamp, nonce, userProfile.WeixinToken))
                {
                    return Content(echostr);
                    //返回随机字符串则表示验证通过
                }
                else
                {
                    return Content("failed:验证失败");
                }
            }
            else
            {
                return Content("failed:没有此用户");
            }
        }

        #endregion GET

        /// <summary>
        /// 最简化的处理流程
        /// 如果一直返回结果的时候,微信在(超时之前)一个请求中可能会多次调用该方法,
        /// </summary>
        [POST("/weixinapi/{id}")]
        public ContentResult MiniPost(string id, string signature, string timestamp, string nonce, string echostr)
        {
            lock (_postingLock)
            {
                if (!_postingFlag)
                {

                    _postingFlag = true;

                    UserProfile userProfile = _userProfileRepository.Find(Specification<UserProfile>.Eval(e => e.UserCode == id));
                    if (userProfile != null)
                    {
                        if (!CheckSignature.Check(signature, timestamp, nonce, userProfile.WeixinToken))
                        {
                            return Content("参数错误！");
                        }
                    }
                    else
                    {
                        return Content("参数错误！");
                    }
                    var logfilename = userProfile.UserName;

                    try
                    {
                        var messageHandler = new CustomMessageHandler(Request.InputStream,
                            userProfile, signature, timestamp, nonce, _qrCodeRepository);

                        //LogHelper.WriteLog(string.Format(@" signature:  {0}timestamp:   {1}nonce:    {2}echostr:  {3}requestdocument:     {4}", signature, timestamp, nonce, echostr,
                        //     messageHandler.RequestDocument.ToString(), userProfile.UserName),
                        //     logfilename + "debug接收微信xml");

                        messageHandler.Execute();

                        //if (messageHandler.ResponseDocument != null)
                        //    LogHelper.WriteLog(messageHandler.ResponseDocument.ToString(),
                        //       logfilename + "debug返回微信xml");

                        _postingFlag = false;




                        if (!CheckContent(messageHandler))
                        {
                            return Content("");
                        }
                        return Content(messageHandler.ResponseDocument.ToString());//v0.7-
                    }
                    catch (Exception ex)
                    {
                        LogHelper.WriteException(ex, logfilename);
                        return Content("");
                    }
                }
                else
                    return Content("");
            }
        }

        /// <summary>
        /// 之判断返回文字的时候的Content属性,因为Content如果为"",则会提示该公众号已停止服务
        /// </summary>
        /// <param name="messageHandler"></param>
        /// <returns></returns>
        private bool CheckContent(CustomMessageHandler messageHandler)
        {
            if (messageHandler.ResponseDocument != null)
            {
                var xDocument = messageHandler.ResponseDocument;
                var element = xDocument.Descendants("Content").FirstOrDefault();
                if (element != null)
                {
                    if (!string.IsNullOrEmpty(element.Value))
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        /*
         * v0.3.0之前的原始Post方法见：WeixinController_OldPost.cs
         *
         * 注意：虽然这里提倡使用CustomerMessageHandler的方法，但是MessageHandler基类最终还是基于OldPost的判断逻辑，
         * 因此如果需要深入了解Senparc.Weixin.MP内部处理消息的机制，可以查看WeixinController_OldPost.cs中的OldPost方法。
         * 目前为止OldPost依然有效，依然可用于生产。
         */
    }
}