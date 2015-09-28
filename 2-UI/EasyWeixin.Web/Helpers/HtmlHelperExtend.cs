using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using EasyWeixin.Core.Common;
using EasyWeixin.Entities.JsonResult;
using EasyWeixin.Data.Repositories;
using Microsoft.Practices.Unity;
using WebMatrix.WebData;
using System.Linq;
using EasyWeixin.CommonAPIs;
using EasyWeixin.AdvancedAPIs.QrCode;
using System;
using EasyWeixin.Exceptions;
using System.Web;
using System.Web.SessionState;

namespace EasyWeixin.Web.Helpers
{
    public static class HtmlHelperExtend
    {
        public static string GetShortUrlSina(this HtmlHelper help, string longUrl)
        {
            var result = GetShortUrl.GetSina(longUrl);

            return result;
        }
        /// <summary>
        /// 调用微信api获取短链接,一天限制使用1000条所以不建议使用
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="longUrl"></param>
        /// <returns></returns>
        public static string GetShortUrlWx(this HtmlHelper helper, string longUrl)
        {
            WeixinShortLinkResult result = new WeixinShortLinkResult();

            var userId = WebSecurity.CurrentUserId;
            IUserProfileRepository repo = Bootstrapper.Initialise().Resolve<IUserProfileRepository>();
            var user = repo.FindAll().SingleOrDefault(s => s.UserId == userId);
            string token = AccessTokenContainer.TryGetToken(user.AppId, user.AppSecret);

            var data = new WeixinShortLinkSend() { access_token = token, long_url = longUrl };

            result = CommonApi.GetShortLink(token, data);
            return result.short_url;
        }

        public static HeaderViewModle GetHeader(this HtmlHelper helper)
        {
            var result = new HeaderViewModle();

            try
            {
                IUserProfileRepository repo = Bootstrapper.Initialise().Resolve<IUserProfileRepository>();
                var user = repo.FindAll().SingleOrDefault(s => s.UserId == WebSecurity.CurrentUserId);
                if (string.IsNullOrEmpty(user.Header))
                {
                    var token = AccessTokenContainer.TryGetToken(user.AppId, user.AppSecret);
                    var qrResult = QrCodeApi.Create(token, "1", 0);
                    var header = QrCodeApi.GetShowQrCodeUrl(qrResult.ticket);
                    var data = new WeixinShortLinkSend() { access_token = token, long_url = header };
                    header = CommonApi.GetShortLink(token, data).short_url;
                    user.Header = header;
                    repo.Update(user);
                    repo.Context.Commit();
                }

                result.Header = user.Header;

            }
            catch (ErrorJsonResultException e)
            {
                result.Error = e.Message;
            }
            catch (Exception)
            {

            }
            return result;
        }
    }
    public class HeaderViewModle
    {
        public HeaderViewModle()
        {
            Header = "";
            Error = "";
        }
        public string Header { get; set; }
        public string Error { get; set; }
    }
}