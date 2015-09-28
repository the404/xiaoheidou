using EasyWeixin.CommonAPIs;
using EasyWeixin.Context;
using EasyWeixin.Core.Common;
using EasyWeixin.Data.Repositories;
using EasyWeixin.Entities.JsonResult;
using EasyWeixin.Entities.Request;
using EasyWeixin.Entities.Request.Events;
using EasyWeixin.Entities.Response;
using EasyWeixin.Exceptions;
using EasyWeixin.Helpers;
using EasyWeixin.MessageHandlers;
using EasyWeixin.Model;
using Senparc.Weixin.MP.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace EasyWeixin.Web.Framework.CommonService.CustomMessageHandler
{
    /// <summary>
    /// 自定义MessageHandler
    /// 把MessageHandler作为基类，重写对应请求的处理方法
    /// </summary>
    public partial class CustomMessageHandler : MessageHandler<MessageContext>
    {
        /*
         * 重要提示：v1.5起，MessageHandler提供了一个DefaultResponseMessage的抽象方法，
         * DefaultResponseMessage必须在子类中重写，用于返回没有处理过的消息类型（也可以用于默认消息，如帮助信息等）；
         * 其中所有原OnXX的抽象方法已经都改为虚方法，可以不必每个都重写。若不重写，默认返回DefaultResponseMessage方法中的结果。
         */

        public string AccessToken
        {
            get { return AccessTokenContainer.TryGetToken(UserProfile.AppId, UserProfile.AppSecret); }
        }

        public readonly UserProfile UserProfile;
        public readonly string Signature;
        public readonly string Timestamp;
        public readonly string Nonce;

        //todo 做一个属性依赖
        //[Dependency]
        //public IQrCodeRepository QrCodeRepo { get; set; }
        private IQrCodeRepository _qrCodeRepository;

        public WeixinUserInfoResult WxUserInfo { get; set; }

        public WeixinHelper WeixinHelper
        {
            get
            {
                return new WeixinHelper(_qrCodeRepository);
            }
        }

        public CustomMessageHandler(
            Stream inputStream,
            UserProfile userProfile,
            IQrCodeRepository qrCodeRepository)
            : base(inputStream)
        {
            //这里设置仅用于测试，实际开发可以在外部更全局的地方设置，
            //比如MessageHandler<MessageContext>.GlobalWeixinContext.ExpireMinutes = 3。
            WeixinContext.ExpireMinutes = 3;
            this.UserProfile = userProfile;
            _qrCodeRepository = qrCodeRepository;
        }

        public CustomMessageHandler(
            Stream inputStream,
            UserProfile userProfile,
            string signature,
            string timestamp,
            string nonce,
            IQrCodeRepository qrCodeRepository)
            : base(inputStream)
        {
            WeixinContext.ExpireMinutes = 3;
            this.UserProfile = userProfile;
            this.Signature = signature;
            this.Timestamp = timestamp;
            this.Nonce = nonce;
            _qrCodeRepository = qrCodeRepository;
        }

        public override void OnExecuting(IRequestMessageBase requestMessage = null)
        {
            //测试MessageContext.StorageData
            if (CurrentMessageContext.StorageData == null)
            {
                CurrentMessageContext.StorageData = 0;
            }
        }

        public override void OnExecuted(IRequestMessageBase requestMessage = null,
            IResponseMessageBase repMessage = null)
        {
            CurrentMessageContext.StorageData = ((int)CurrentMessageContext.StorageData) + 1;
        }

        /// <summary>
        ///将此方法返回值直接返回给微信用户即可
        /// </summary>
        /// <param name="proxyUrl">服务器URL</param>
        /// <param name="query">带的参数</param>
        /// <param name="postStr">请求XML</param>
        /// <returns></returns>
        public string CallProxy(string proxyUrl, string postStr)
        {
            string ret = "";
            try
            {
                ret = NetworkHelperAll.HttpPost(proxyUrl, Signature, Timestamp, Nonce, postStr);
            }
            catch
            {
                ret = null;
            }
            return ret;
        }

        /// <summary>
        /// 处理文字请求
        /// </summary>
        /// <returns></returns>
        public override IResponseMessageBase OnTextRequest(RequestMessageText requestMessage)
        {
            //TODO:这里的逻辑可以交给Service处理具体信息，参考OnLocationRequest方法或/Service/LocationSercice.cs

            //方法一（v0.1），此方法调用太过繁琐，已过时（但仍是所有方法的核心基础），建议使用方法二到四
            //var responseMessage =
            //    ResponseMessageBase.CreateFromRequestMessage(RequestMessage, ResponseMsgType.Text) as
            //    ResponseMessageText;

            //方法二（v0.4）
            //var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(RequestMessage);

            //方法三（v0.4），扩展方法，需要using Senparc.Weixin.MP.Helpers;
            //var responseMessage = RequestMessage.CreateResponseMessage<ResponseMessageText>();

            //方法四（v0.6+），仅适合在HandlerMessage内部使用，本质上是对方法三的封装
            //注意：下面泛型ResponseMessageText即返回给客户端的类型，可以根据自己的需要填写ResponseMessageNews等不同类型。

            #region 默认关键字

            #region 天气搜索

            if (requestMessage.Content.Contains("天气"))
            {
                string cityname = requestMessage.Content.Replace("天气", "");
                string citycode = WeiXinService.getCityCode(cityname);
                var responseMessage = CreateResponseMessage<ResponseMessageText>();

                if (citycode != "")
                {
                    weather w = WeiXinService.GetWeatherInfo(citycode);
                    responseMessage.Content = "【" + cityname + "天气预报】\n" + w.weatherinfo.date_y + " " +
                                              w.weatherinfo.fchh + "时发布" + "\n\n实时天气\n" + w.weatherinfo.weather1 + " " +
                                              w.weatherinfo.temp1 + " " + w.weatherinfo.wind1 + "\n\n温馨提示：" +
                                              w.weatherinfo.index_d + "\n\n明天\n" + w.weatherinfo.weather2 + " " +
                                              w.weatherinfo.temp2 + " " + w.weatherinfo.wind2 + "\n\n后天\n" +
                                              w.weatherinfo.weather3 + " " + w.weatherinfo.temp3 + " " +
                                              w.weatherinfo.wind3;
                }
                else
                {
                    responseMessage.Content = "抱歉，没有查到" + cityname + "的天气信息！";
                }
                return responseMessage;
            }

            #endregion 天气搜索

            #region 火车查询

            if (requestMessage.Content.Contains("火车查询"))
            {
                string traincode = requestMessage.Content.Replace("火车查询", "");
                if (traincode != "")
                {
                    var responseMessage = CreateResponseMessage<ResponseMessageText>();
                    responseMessage.Content = WeiXinService.GetJHTrainString(traincode);
                    return responseMessage;
                }
            }

            #endregion 火车查询

            #region 航班查询

            if (requestMessage.Content.Contains("航班"))
            {
                string flightcode = requestMessage.Content.Replace("航班", "");
                if (flightcode != "")
                {
                    var responseMessage = CreateResponseMessage<ResponseMessageText>();
                    responseMessage.Content = WeiXinService.GetFlightString(flightcode);
                    return responseMessage;
                }
            }

            #endregion 航班查询

            if (requestMessage.Content.Contains("我要上网"))
            {
                if (UserProfile.UserCode == "6d4603790dfc47bbb4cacde8f24e6fb7")
                {
                    try
                    {
                        //string sUrl = "http://cloud.doctorcom.com/drcomweixin/WXServlet";
                        string sUrl = "http://" + HttpContext.Current.Request.Url.Host + "/ChatLine/test";
                        string res = string.Format(@"<xml>
                                       <ToUserName><![CDATA[{0}]]></ToUserName>
                                       <FromUserName><![CDATA[{1}]]></FromUserName>
                                        <CreateTime>{2}</CreateTime>
                                        <MsgType><![CDATA[{3}]]></MsgType>
                                        <Content><![CDATA[{4}]]></Content>
                                       </xml> ",
                            requestMessage.FromUserName, requestMessage.ToUserName, DateTime.Now, requestMessage.MsgType,
                            requestMessage.Content);

                        string ss = CallProxy(sUrl, res);
                        var responseMessage = CreateResponseMessage<ResponseMessageText>();
                        responseMessage.Content = ss;
                        return responseMessage;
                    }
                    catch
                    {
                        return null;
                    }
                }
            }

            if (requestMessage.Content.Contains("官方"))
            {
                return AutoResponseMessage(requestMessage.FromUserName);
            }

            #endregion 默认关键字

            try
            {
                var datas =
                    UserProfile.ResponseKeys.FirstOrDefault(
                        o => o.Key == requestMessage.Content && o.IsFullMatch == 1);
                if (datas != null && datas.ResponseKeyRules != null &&
                    datas.ResponseKeyRules.ResponseMessages.ToList().Count > 0)
                {
                    var responseMessages = datas.ResponseKeyRules.ResponseMessages.ToList();
                    return GetResponseMessages(responseMessages, requestMessage.FromUserName);
                }
                else
                {
                    datas =
                        UserProfile.ResponseKeys.FirstOrDefault(
                            o => o.Key.Contains(requestMessage.Content) && o.IsFullMatch == 0);
                    if (datas != null && datas.ResponseKeyRules != null &&
                        datas.ResponseKeyRules.ResponseMessages.ToList().Count > 0)
                    {
                        var responseMessages = datas.ResponseKeyRules.ResponseMessages.ToList();
                        return GetResponseMessages(responseMessages, requestMessage.FromUserName);
                    }
                    else
                    {
                        datas =
                            UserProfile.ResponseKeys.FirstOrDefault(
                                o => requestMessage.Content.Contains(o.Key) && o.IsFullMatch == 0);
                        if (datas != null && datas.ResponseKeyRules != null &&
                            datas.ResponseKeyRules.ResponseMessages.ToList().Count > 0)
                        {
                            var responseMessages = datas.ResponseKeyRules.ResponseMessages.ToList();
                            return GetResponseMessages(responseMessages, requestMessage.FromUserName);
                        }
                    }

                    //自动回复信息
                    return AutoResponseMessage(requestMessage.FromUserName);
                }
            }
            catch (ErrorJsonResultException)
            {
                return AutoResponseMessage(requestMessage.FromUserName);
            }
        }

        /// <summary>
        /// 处理位置请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnLocationRequest(RequestMessageLocation requestMessage)
        {
            var locationService = new LocationService();
            var responseMessage = locationService.GetResponseMessage(requestMessage as RequestMessageLocation);
            return responseMessage;
        }

        /// <summary>
        /// 处理图片请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnImageRequest(RequestMessageImage requestMessage)
        {
            //var responseMessage = CreateResponseMessage<ResponseMessageNews>();
            //responseMessage.Articles.Add(new Article()
            //{
            //    Title = "您刚才发送了图片信息",
            //    Description = "您发送的图片将会显示在边上",
            //    PicUrl = requestMessage.PicUrl,
            //    Url = "http://weixin.senparc.com"
            //});
            //responseMessage.Articles.Add(new Article()
            //{
            //    Title = "第二条",
            //    Description = "第二条带连接的内容",
            //    PicUrl = requestMessage.PicUrl,
            //    Url = "http://weixin.senparc.com"
            //});
            //return responseMessage;
            try
            {
                //var datas = UserProfile.ResponseKeys.FirstOrDefault(o => o.Key == requestMessage.Content && o.IsFullMatch == 1);
                //if (datas != null && datas.ResponseKeyRules != null && datas.ResponseKeyRules.ResponseMessages.ToList().Count > 0)
                //{
                //    var ResponseMessages = datas.ResponseKeyRules.ResponseMessages.ToList();
                //    return GetResponseMessages(ResponseMessages, requestMessage.FromUserName);
                //}
                //else
                //{
                //    datas = UserProfile.ResponseKeys.FirstOrDefault(o => o.Key.Contains(requestMessage.Content) && o.IsFullMatch == 0);
                //    if (datas != null && datas.ResponseKeyRules != null && datas.ResponseKeyRules.ResponseMessages.ToList().Count > 0)
                //    {
                //        var ResponseMessages = datas.ResponseKeyRules.ResponseMessages.ToList();
                //        return GetResponseMessages(ResponseMessages, requestMessage.FromUserName);
                //    }
                //    else
                //    {
                //        datas = UserProfile.ResponseKeys.FirstOrDefault(o => requestMessage.Content.Contains(o.Key) && o.IsFullMatch == 0);
                //        if (datas != null && datas.ResponseKeyRules != null && datas.ResponseKeyRules.ResponseMessages.ToList().Count > 0)
                //        {
                //            var ResponseMessages = datas.ResponseKeyRules.ResponseMessages.ToList();
                //            return GetResponseMessages(ResponseMessages, requestMessage.FromUserName);
                //        }
                //    }

                //自动回复信息
                return AutoResponseMessage(requestMessage.FromUserName);
                // }
            }
            catch (ErrorJsonResultException)
            {
                return AutoResponseMessage(requestMessage.FromUserName);
            }
        }

        /// <summary>
        /// 处理语音请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnVoiceRequest(RequestMessageVoice requestMessage)
        {
            var responseMessage = CreateResponseMessage<ResponseMessageMusic>();
            responseMessage.Music.MusicUrl = "http://weixin.senparc.com/Content/music1.mp3";
            return responseMessage;
        }

        /// <summary>
        /// 处理链接消息请求
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnLinkRequest(RequestMessageLink requestMessage)
        {
            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            responseMessage.Content = string.Format(@"您发送了一条连接信息：
Title：{0}
Description:{1}
Url:{2}", requestMessage.Title, requestMessage.Description, requestMessage.Url);
            return responseMessage;
        }

        /// <summary>
        /// 处理事件请求（这个方法一般不用重写，这里仅作为示例出现。除非需要在判断具体Event类型以外对Event信息进行统一操作
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        //public override IResponseMessageBase OnEventRequest(RequestMessageEventBase requestMessage)
        //{
        //    var eventResponseMessage = base.OnEventRequest(requestMessage);//对于Event下属分类的重写方法，见：CustomerMessageHandler_Events.cs
        //    //TODO: 对Event信息进行统一操作
        //    return eventResponseMessage;
        //}
        public override IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage)
        {
            var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "敬请期待！";
            return responseMessage;
        }

        public override IResponseMessageBase DefaultResponseMessage(string content = "")
        {
            var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "";
            return responseMessage;
        }

        /// <summary>
        /// 获取内容页第一张图片
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public string GetImageUrl(string html)
        {
            var ImageUrl = HtmlStringHelper.GetHtmlImageUrlList(html).Length > 0
                ? HtmlStringHelper.GetHtmlImageUrlList(html)[0]
                : "";
            if (!ImageUrl.Contains("http") && !ImageUrl.Contains("weixin.ipow.cn"))
            {
                ImageUrl = "http://" + HttpContext.Current.Request.Url.Host + "" + ImageUrl;
            }
            return ImageUrl;
        }

        /// <summary>
        /// 返回自动输出信息
        /// </summary>
        /// <param name="userWexinId"></param>
        /// <returns></returns>
        public IResponseMessageBase AutoResponseMessage(string userWexinId)
        {
            var responseMessages = UserProfile.ResponseMessages.Where(o => o.ResponseType == 2).ToList();
            return GetResponseMessages(responseMessages, userWexinId);
        }

        /// <summary>
        /// 依照类型 返回自动输出信息
        /// </summary>
        /// <param name="responseMessages"></param>
        /// <param name="userWexinId"></param>
        /// <returns></returns>
        public IResponseMessageBase GetResponseMessages(List<ResponseMessage> responseMessages, string userWexinId)
        {
            //返回类型Text (ButtonType == 0)
            if (responseMessages.Where(o => o.ButtonType == 0).ToList().Count > 0)
            {
                var responseMessage = CreateResponseMessage<ResponseMessageText>();
                responseMessage.Content = responseMessages.Where(o => o.ButtonType == 0).ElementAt(0).Content;
                return responseMessage;
            }

            //返回类型Music (ButtonType == 1)
            else if (responseMessages.Where(o => o.ButtonType == 1).ToList().Count > 0)
            {
                var responseMessage = CreateResponseMessage<ResponseMessageMusic>();
                responseMessage.Music.Title = responseMessages.ElementAt(0).ResponseMusic.MusicName;
                responseMessage.Music.HQMusicUrl = "http://" + HttpContext.Current.Request.Url.Host + "" +
                                                   responseMessages.ElementAt(0).ResponseMusic.HQMusicUrl;
                responseMessage.Music.MusicUrl = "http://" + HttpContext.Current.Request.Url.Host + "" +
                                                 responseMessages.ElementAt(0).ResponseMusic.MusicUrl;
                responseMessage.Music.Description = "";
                return responseMessage;
            }
            //返回类型Image (ButtonType == 2)
            else if (responseMessages.Where(o => o.ButtonType == 2).ToList().Count > 0)
            {
                var responseMessage = CreateResponseMessage<ResponseMessageImage>();
                responseMessage.PicUrl = "http://" + HttpContext.Current.Request.Url.Host + "" +
                                         responseMessages.ElementAt(0).ResponseImage.ImageUrl;
                return responseMessage;
            }
            //返回类型Video (ButtonType == 3)
            else if (responseMessages.Where(o => o.ButtonType == 3).ToList().Count > 0)
            {
                var responseMessage = CreateResponseMessage<ResponseMessageVideo>();
                responseMessage.VideoUrl = "http://" + HttpContext.Current.Request.Url.Host + "" +
                                           responseMessages.ElementAt(0).ResponseVideo.VideoUrl;
                return responseMessage;
            }
            //返回类型News (ButtonType == 4)
            else if (responseMessages.Where(o => o.ButtonType == 4).ToList().Count > 0)
            {
                var responseMessage = CreateResponseMessage<ResponseMessageNews>();

                foreach (var item in responseMessages)
                {
                    var picurl = String.IsNullOrEmpty(item.ResponseImageText.PicUrl)
                        ? GetImageUrl(item.ResponseImageText.Content)
                        : item.ResponseImageText.PicUrl;
                    var desc = !String.IsNullOrEmpty(item.ResponseImageText.ImageTextDesc)
                        ? HtmlStringHelper.ClearHTMLString(item.ResponseImageText.ImageTextDesc)
                        : "";
                    responseMessage.Articles.Add(new Article()
                    {
                        Description = desc,
                        Title = item.ResponseImageText.ImageTextName,
                        PicUrl = picurl,
                        Url = item.ResponseImageText.Url + "&User_ID=" + UserProfile.ID + "&UserWexinID=" + userWexinId
                    });
                }
                return responseMessage;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 依照类型 返回自动输出信息
        /// </summary>
        /// <param name="responseMessages"></param>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public IResponseMessageBase GetEventsResponseMessages(List<ResponseMessage> responseMessages,
            IRequestMessageEventBase requestMessage)
        {
            //返回类型Text (ButtonType == 0)
            try
            {
                if (responseMessages.Where(o => o.ButtonType == 0).ToList().Count > 0)
                {
                    #region Text

                    var responseMessage =
                        ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
                    responseMessage.Content = responseMessages.Where(o => o.ButtonType == 0).ElementAt(0).Content;
                    //当这里的Content为空的时候,微信客户端会提示"公众号无法服务",所以不能这样子
                    //TODO 检测这边的逻辑到底对不对呢?需要从创建菜单开始来看
                    if (string.IsNullOrEmpty(responseMessage.Content))
                    {
                        return DefaultResponseMessage(requestMessage);
                    }
                    return responseMessage;

                    #endregion Text
                }
                //返回类型Music (ButtonType == 1)
                else if (responseMessages.Where(o => o.ButtonType == 1).ToList().Count > 0)
                {
                    #region Music

                    var responseMessage =
                        ResponseMessageBase.CreateFromRequestMessage<ResponseMessageMusic>(requestMessage);
                    responseMessage.Music.Title = responseMessages.ElementAt(0).ResponseMusic.MusicName;
                    responseMessage.Music.HQMusicUrl = "http://" + HttpContext.Current.Request.Url.Host + "" +
                                                       responseMessages.ElementAt(0).ResponseMusic.HQMusicUrl;
                    responseMessage.Music.MusicUrl = "http://" + HttpContext.Current.Request.Url.Host + "" +
                                                     responseMessages.ElementAt(0).ResponseMusic.MusicUrl;
                    responseMessage.Music.Description = "";
                    return responseMessage;

                    #endregion Music
                }
                //返回类型Image (ButtonType == 2)
                else if (responseMessages.Where(o => o.ButtonType == 2).ToList().Count > 0)
                {
                    #region Image

                    var responseMessage =
                        ResponseMessageBase.CreateFromRequestMessage<ResponseMessageImage>(requestMessage);
                    responseMessage.PicUrl = "http://" + HttpContext.Current.Request.Url.Host + "" +
                                             responseMessages.ElementAt(0).ResponseImage.ImageUrl;
                    return responseMessage;

                    #endregion Image
                }
                //返回类型Video (ButtonType == 3)
                else if (responseMessages.Where(o => o.ButtonType == 3).ToList().Count > 0)
                {
                    #region Video

                    var responseMessage =
                        ResponseMessageBase.CreateFromRequestMessage<ResponseMessageVideo>(requestMessage);
                    responseMessage.VideoUrl = "http://" + HttpContext.Current.Request.Url.Host + "" +
                                               responseMessages.ElementAt(0).ResponseVideo.VideoUrl;
                    return responseMessage;

                    #endregion Video
                }
                //返回类型News(图文消息) (ButtonType == 4)
                else if (responseMessages.Where(o => o.ButtonType == 4).ToList().Count > 0)
                {
                    #region 图文消息

                    WeixinUserInfoResult wx = new WeixinUserInfoResult();
                    try
                    {
                        string token = AccessTokenContainer.TryGetToken(UserProfile.AppId, UserProfile.AppSecret);
                        wx = CommonApi.GetUserInfo(token, requestMessage.FromUserName);
                    }
                    catch (Exception)
                    {//这个错误是因为某些公众号并没有申请微信认证
                        wx.nickname = "";
                    }
                    var responseMessage =
                        ResponseMessageBase.CreateFromRequestMessage<ResponseMessageNews>(requestMessage);

                    try
                    {
                        //时间戳
                        string timestamp =
                            Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds)
                                .ToString();
                        string SALT = "YNOCT";
                        string[] tmpArr = new string[3] { SALT, timestamp, requestMessage.FromUserName };
                        Array.Sort(tmpArr, string.CompareOrdinal);
                        string signature = CheckSignature.Implode("", tmpArr);
                        signature = CheckSignature.sha1(signature);

                        if (UserProfile.UserCode == "592095e074a347678a52fa81370f8caa")
                        {
                            foreach (var item in responseMessages)
                            {
                                var picurl = String.IsNullOrEmpty(item.ResponseImageText.PicUrl)
                                    ? GetImageUrl(item.ResponseImageText.Content)
                                    : item.ResponseImageText.PicUrl;
                                var desc = !String.IsNullOrEmpty(item.ResponseImageText.ImageTextDesc)
                                    ? HtmlStringHelper.ClearHTMLString(item.ResponseImageText.ImageTextDesc)
                                    : "";
                                string url = string.Empty;
                                if (item.ResponseImageText.Url.IndexOf("3.4.5.6") > -1)
                                {
                                    url = item.ResponseImageText.Url + "&User_ID=" + UserProfile.ID + "&openid=" +
                                          requestMessage.FromUserName + "&signature=" + signature + "&timestamp=" +
                                          timestamp + "&nickname=" + wx.nickname;
                                }
                                else if (item.ResponseImageText.Url.IndexOf("http://vpay.upcard.com.cn") > -1)
                                {
                                    url = item.ResponseImageText.Url + "&User_ID=" + UserProfile.ID + "&openId=" +
                                          requestMessage.FromUserName;
                                }
                                else
                                {
                                    url = item.ResponseImageText.Url + "&User_ID=" + UserProfile.ID + "&UserWexinID=" +
                                          requestMessage.FromUserName;
                                }
                                responseMessage.Articles.Add(new Article()
                                {
                                    Description = desc,
                                    Title = item.ResponseImageText.ImageTextName,
                                    PicUrl = picurl,
                                    Url = url
                                });
                            }
                        }
                        else
                        {
                            foreach (var item in responseMessages)
                            {
                                var picurl = String.IsNullOrEmpty(item.ResponseImageText.PicUrl)
                                    ? GetImageUrl(item.ResponseImageText.Content)
                                    : item.ResponseImageText.PicUrl;
                                var desc = !String.IsNullOrEmpty(item.ResponseImageText.ImageTextDesc)
                                    ? HtmlStringHelper.ClearHTMLString(item.ResponseImageText.ImageTextDesc)
                                    : "";
                                responseMessage.Articles.Add(new Article()
                                {
                                    Description = desc,
                                    Title = item.ResponseImageText.ImageTextName,
                                    PicUrl = picurl,
                                    Url =
                                        item.ResponseImageText.Url + "&User_ID=" + UserProfile.ID + "&UserWexinID=" +
                                        requestMessage.FromUserName
                                });
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        string path = System.Web.HttpContext.Current.Server.MapPath("/txt/");
                        string ss = "bb.txt";
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        StreamWriter sw = new StreamWriter(path + ss, true, Encoding.UTF8);
                        sw.WriteLine(e.Message);
                        sw.Close();

                        throw new Exception(e.Message);
                    }

                    //武汉欢乐谷抢红包
                    //var IsHave = RecordWUserRepository.Find(Specification<RecordWUser>.Eval(o => o.FromUserName == requestMessage.FromUserName));
                    //if (IsHave == null)
                    //{
                    //    RecordWUser rw = new RecordWUser();
                    //    rw.FromUserName = requestMessage.FromUserName;
                    //    rw.ToUserName = requestMessage.ToUserName;
                    //    rw.HeadimgUrl = wx.headimgurl;
                    //    rw.sex = wx.sex;
                    //    rw.NickName = wx.nickname;
                    //    RecordWUserRepository.Add(rw);
                    //    RecordWUserRepository.Context.Commit();
                    //}

                    return responseMessage;

                    #endregion 图文消息
                }
            }
            catch
            {
                return DefaultResponseMessage(requestMessage);
            }
            return DefaultResponseMessage(requestMessage);
        }
    }
}