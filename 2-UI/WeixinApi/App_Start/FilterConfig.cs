using WeixinApi.Filters;
using System.Web;
using System.Web.Mvc;

namespace WeixinApi
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new InitializeSimpleMembershipAttribute());
            filters.Add(new AllowCrossSiteJsonAttribute());
            filters.Add(new ExceptionFilterAttribute());
        }
    }
}