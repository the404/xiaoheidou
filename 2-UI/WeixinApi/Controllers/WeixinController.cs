using Apworks.Repositories.EntityFramework;
using Apworks.Specifications;
using EasyWeixin;
using EasyWeixin.Data;
using EasyWeixin.Data.Repositories;
using EasyWeixin.Helpers;
using EasyWeixin.Model;
using EasyWeixin.Web.Framework.CommonService.CustomMessageHandler;
using EasyWeixin.Web.Framework.Controllers;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace WeixinApi.Controllers
{
    public class WeixinController : ApiController
    {
        private volatile bool _postingFlag = false;
        private object _postingLock = new object();
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IQrCodeRepository _qrCodeRepository;

        public WeixinController(
            IUserProfileRepository userProfileRepository,
            IQrCodeRepository qrCodeRepository)
        {
            _userProfileRepository = userProfileRepository;
            _qrCodeRepository = qrCodeRepository;
        }
        /// <summary>
        /// /api/Weixin/message=123
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public HttpResponseMessage Get(string message)
        {
            var context = HttpContext.Current;
            LogHelper.WriteLog("", "test");
            return new HttpResponseMessage { Content = new StringContent(message) };
        }
        public HttpResponseMessage Get()
        {
            var url = "http://" + Request.RequestUri.Authority + "/api/weixin?message=1244";
            WebClient wc = new WebClient();
            var str = wc.DownloadString(url);
            return Request.CreateResponse(HttpStatusCode.OK, str);
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
        [Route("/weixinapi/{id}")]
        [HttpGet]
        public HttpResponseMessage Get(string id, string signature = "", string timestamp = "", string nonce = "", string echostr = "")
        {
            UserProfile userProfile = _userProfileRepository.Find(Specification<UserProfile>.Eval(e => e.UserCode == id));
            //            LogHelper.WriteLog(string.Format(@" signature:  {0}
            //                                                timestamp:   {1}
            //                                                nonce:    {2}
            //                                                echostr:  {3}",signature,timestamp,nonce,echostr));
            //LogHelper.WriteLog("成功访问Get", "访问Get");
            if (userProfile != null)
            {
                if (CheckSignature.Check(signature, timestamp, nonce, userProfile.WeixinToken))
                {
                    //LogHelper.WriteLog("成功", "访问Get");
                    return new HttpResponseMessage { Content = new StringContent(echostr) };
                }
                else
                {
                    //LogHelper.WriteLog("failed:验证失败", "访问Get");
                    return new HttpResponseMessage { Content = new StringContent("failed:验证失败") };
                }
            }
            else
            {
                //LogHelper.WriteLog("failed:没有此用户", "访问Get");
                return new HttpResponseMessage { Content = new StringContent("failed:没有此用户") };
            }
        }

        #endregion GET

        #region POST
        /// <summary>
        /// 最简化的处理流程
        /// 如果一直返回结果的时候,微信在(超时之前)一个请求中可能会多次调用该方法,
        /// </summary>
        [Route("/weixinapi/{id}")]
        [HttpPost]
        public HttpResponseMessage Post(string id, string signature, string timestamp, string nonce, string echostr)
        {
            try
            {
                LogHelper.WriteLog(string.Format(@"进入Post: signature:  {0}
                                            timestamp:   {1}
                                            nonce:    {2}
                                            echostr:  {3}",
                                                signature, timestamp, nonce, echostr));
                lock (_postingLock)
                {
                    if (!_postingFlag)
                    {
                        _postingFlag = true;
                        //LogHelper.WriteLog(string.Format("{0}\n{1}\n{2}\n{3}\n{4}", id, signature, timestamp, nonce, echostr), "MiniPost");
                        UserProfile userProfile = _userProfileRepository.Find(Specification<UserProfile>.Eval(e => e.UserCode == id));
                        if (userProfile != null)
                        {
                            if (!CheckSignature.Check(signature, timestamp, nonce, userProfile.WeixinToken))
                            {
                                return new HttpResponseMessage { Content = new StringContent("参数错误！") };
                            }
                        }
                        else
                        {
                            return new HttpResponseMessage { Content = new StringContent("参数错误！") };
                        }
                        var logfilename = userProfile.UserName + "," + nonce;

                        try
                        {
                            var stream = HttpContext.Current.Request.InputStream;
                            if (stream == null)
                            {
                                throw new NullReferenceException("HttpContext.Current.Request.InputStream 是获取不到stream的");
                            }
                            var messageHandler = new CustomMessageHandler(stream,
                                userProfile, signature, timestamp, nonce, _qrCodeRepository);

                            LogHelper.WriteLog(string.Format(@" signature:  {0}
                                                            timestamp:   {1}
                                                            nonce:    {2}
                                                            echostr:  {3}
                                                            requestdocument:     {4}",
                                                                signature, timestamp, nonce, echostr,
                                 messageHandler.RequestDocument.ToString(), userProfile.UserName),
                                 logfilename + "debug接收微信xml");

                            messageHandler.Execute();
                            if (messageHandler.ResponseDocument != null)
                                LogHelper.WriteLog(messageHandler.ResponseDocument.ToString(),
                                   logfilename + "debug返回微信xml");

                            _postingFlag = false;

                            if (!CheckContent(messageHandler))
                            {
                                return new HttpResponseMessage { Content = new StringContent("") };
                            }
                            //return new WeixinResult(messageHandler);//v0.8+
                            return new HttpResponseMessage { Content = new StringContent(messageHandler.ResponseDocument.ToString()) };//v0.7-
                        }
                        catch (Exception ex)
                        {
                            LogHelper.WriteException(ex, logfilename);
                            return new HttpResponseMessage { Content = new StringContent("") };
                        }
                    }
                    else
                    {
                        return new HttpResponseMessage { Content = new StringContent("") };
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteException(ex, "错误");
                return new HttpResponseMessage { Content = new StringContent("...") };
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
        #endregion
    }
}