using EasyWeixin.Data.Repositories;
using System;
using System.Web.Mvc;

namespace EasyWeixin.Web.Filters
{
    public class Auther3FilterAttribute : ActionFilterAttribute
    {
        private IUserProfileRepository _userProfileRepository;

        public Auther3FilterAttribute(IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext == null)
                throw new ArgumentNullException("filterContext");

            string code = filterContext.HttpContext.
                      Request.QueryString["code"];
            string userId = filterContext.HttpContext.
                Request.QueryString["userId"];
            //todo这里需要根据传入的userId来获取系统内的用户数据

            var appId = "";
            if (string.IsNullOrEmpty(code))
            {
                var url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_userinfo&state=1#wechat_redirect";
                filterContext.Result = new RedirectResult(string.Format(url, appId, filterContext.HttpContext.Request.Url.AbsoluteUri));
            }
        }
    }
}