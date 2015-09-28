using EasyWeixin.Entities.JsonResult;
using EasyWeixin.Helpers;
using EasyWeixin.HttpUtility;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace EasyWeixin.CommonAPIs
{
    /// <summary>
    /// 通用接口
    /// 通用接口用于和微信服务器通讯，一般不涉及自有网站服务器的通讯
    /// 见 http://mp.weixin.qq.com/wiki/home/index.html
    /// </summary>
    public partial class CommonApi
    {
        /// <summary>
        /// appid={0}redirect_uri={1}
        /// </summary>
        public const string Snsapi_baseUrl = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base&state=1#wechat_redirect";
        public const string Snsapi_userinfoUrl = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_userinfo&state=1#wechat_redirect";

        /// <summary>
        /// 获取凭证接口
        /// </summary>
        /// <param name="grantType">获取access_token填写client_credential</param>
        /// <param name="appid">第三方用户唯一凭证</param>
        /// <param name="secret">第三方用户唯一凭证密钥，既appsecret</param>
        /// <returns></returns>
        internal static AccessTokenResult GetToken(string appid, string secret, string grantType = "client_credential")
        {
            var url = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type={0}&appid={1}&secret={2}",
                                    grantType, appid, secret);

            AccessTokenResult result = Get.GetJson<AccessTokenResult>(url);

            return result;
        }

        /// <summary>
        /// 用户信息接口
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        public static WeixinUserInfoResult GetUserInfo(string accessToken, string openId)
        {
            var url = string.Format("https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN",
                                    accessToken, openId);
            WeixinUserInfoResult result = Get.GetJson<WeixinUserInfoResult>(url, Encoding.UTF8);
            return result;
        }

        /// <summary>
        /// 发送客服信息接口
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="jsonResults"></param>
        /// <returns></returns>
        public static WxJsonResult SendCustomerMsg(string accessToken, string jsonResults)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                var bytes = Encoding.UTF8.GetBytes(jsonResults);
                ms.Write(bytes, 0, bytes.Length);
                ms.Seek(0, SeekOrigin.Begin);

                var url = string.Format("https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token={0}", accessToken);
                var result = Post.PostGetJson<WxJsonResult>(url, null, ms);
                return result;
            }
        }

        /// <summary>
        /// 媒体文件上传接口
        ///注意事项
        ///1.上传的媒体文件限制：
        ///图片（image) : 1MB，支持JPG格式
        ///语音（voice）：1MB，播放长度不超过60s，支持MP4格式
        ///视频（video）：10MB，支持MP4格式
        ///缩略图（thumb)：64KB，支持JPG格式
        ///2.媒体文件在后台保存时间为3天，即3天后media_id失效
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="type">上传文件类型</param>
        /// <param name="fileName">上传文件完整路径+文件名</param>
        /// <returns></returns>
        public static UploadMediaFileResult UploadMediaFile(string accessToken, UploadMediaFileType type, string fileName)
        {
            var cookieContainer = new CookieContainer();
            var fileStream = FileHelper.GetFileStream(fileName);

            var url = string.Format("http://api.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={1}&filename={2}&filelength={3}",
                accessToken, type.ToString(), Path.GetFileName(fileName), fileStream != null ? fileStream.Length : 0);
            UploadMediaFileResult result = Post.PostGetJson<UploadMediaFileResult>(url, cookieContainer, fileStream);
            return result;
        }

        ///  <summary>
        ///  根据Code得到OAhth的AccessToken
        ///  错误时返回{"errcode":40029,"errmsg":"invalid code"}
        ///  获取code后，请求以下链接获取access_token：
        ///  </summary>
        ///  <param name="appid"></param>
        ///  <param name="secret"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static OAuthAccessTokenResult GetOAuthToken(string appid, string secret, string code)
        {
            var url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code",
                                    appid, secret, code);
            OAuthAccessTokenResult result = Get.GetJson<OAuthAccessTokenResult>(url);
            return result;
        }

        /// <summary>
        /// 最好不用，不知道为什么一刷新纠错
        /// 刷新access_token
        /// 由于access_token拥有较短的有效期，当access_token超时后，可以使用refresh_token进行刷新，refresh_token拥有较长的有效期（7天、30天、60天、90天），当refresh_token失效的后，需要用户重新授权。
        /// 获取第二步的refresh_token后，请求以下链接获取access_token：
        /// </summary>
        // ReSharper disable once UnusedMember.Local
        private static OAuthAccessTokenResult RefershOAuthToken(string appid)
        {
            var url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&grant_type=refresh_token&refresh_token=REFRESH_TOKEN",
                                    appid);

            OAuthAccessTokenResult result = Get.GetJson<OAuthAccessTokenResult>(url);
            return result;
        }

        /// <summary>
        /// {"errcode":40003,"errmsg":" invalid openid "}
        /// </summary>
        /// <returns></returns>
        public static OAuthWeixinUserInfoResult GetOAuthWeixinUserInfo(string accessToken, string openId)
        {
            var url = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN",
                                    accessToken, openId);
            OAuthWeixinUserInfoResult result = Get.GetJson<OAuthWeixinUserInfoResult>(url, Encoding.UTF8);
            return result;
        }

        public static bool CheckAccessToken(string accessToken, string openId)
        {
            var url = string.Format("https://api.weixin.qq.com/sns/auth?access_token={0}&openid={1}",
                                    accessToken, openId);
            CheckResult result = Get.GetJson<CheckResult>(url, Encoding.UTF8);

            return result.errcode == "0" && result.errmsg == "ok";
        }

        /// <summary>
        /// 获取微信JsApi_ticket
        /// </summary>
        internal static JsApiTicketResult GetJsApi_Ticket(string appId, string secret, string type = "jsapi")
        {
            var accessToken = AccessTokenContainer.TryGetToken(appId, secret);

            var url = string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type={1}",
                                    accessToken, type);

            JsApiTicketResult result = Get.GetJson<JsApiTicketResult>(url);
            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="secret"></param>
        /// <param name="url">要分享的url(从http开始，如果有参数，包含参数）</param>
        /// <returns></returns>
        public static WxConfigResult GetWxConfigResult(string appid, string secret, string url)
        {
            url = url.IndexOf("#", StringComparison.Ordinal) >= 0 ? url.Substring(0, url.IndexOf("#", StringComparison.Ordinal)) : url;

            var jsapiTicket = JsApiTicketContainer.TryGetTicket(appid, secret);
            var timestamp = CommonHelper.GetTimeStamp();
            var noncestr = CommonHelper.GuidTo16String(Guid.NewGuid());

            var str = string.Format("jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}", jsapiTicket, noncestr, timestamp, url);

            var hash = CommonHelper.Sha1(str);

            var wxConfigResult = new WxConfigResult()
            {
                appid = appid,
                timestamp = timestamp,
                noncestr = noncestr,
                signature = hash,
                ticket = jsapiTicket,
                url = str
            };
            return wxConfigResult;
        }

        public static WeixinShortLinkResult GetShortLink(string accessToken, WeixinShortLinkSend data)
        {
            var jsonString = JsonConvert.SerializeObject(data);
            CookieContainer cookieContainer = null;// 
            using (MemoryStream ms = new MemoryStream())
            {
                var bytes = Encoding.UTF8.GetBytes(jsonString);
                ms.Write(bytes, 0, bytes.Length);
                ms.Seek(0, SeekOrigin.Begin);

                var url = string.Format("https://api.weixin.qq.com/cgi-bin/shorturl?access_token={0}", accessToken);
                var result = Post.PostGetJson<WeixinShortLinkResult>(url, cookieContainer, ms);
                return result;
            }
        }
    }
}