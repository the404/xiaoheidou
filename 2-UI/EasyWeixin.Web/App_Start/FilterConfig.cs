using EasyWeixin.Web.Filters;
using System.Web.Mvc;

namespace EasyWeixin.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new InitializeSimpleMembershipAttribute());
            filters.Add(new AllowCrossSiteJsonAttribute());
            filters.Add(new ExceptionFilterAttribute());
            //filters.Add(new TotalCacheFilterAttribute());
        }
    }
}