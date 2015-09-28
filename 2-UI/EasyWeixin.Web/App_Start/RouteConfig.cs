using System.Web.Mvc;
using System.Web.Routing;

namespace EasyWeixin.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Admin", action = "index", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "shtml",
                url: "award/{action}.*html",
                defaults: new { controller = "Admin", action = "index", id = UrlParameter.Optional }
                );
            routes.MapRoute(
                name: "html",
                url: "award/{action}.html",
                defaults: new { controller = "Admin", action = "index", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "areas",
                url: "activity/{controller}/{action}/{id}",
                defaults: new { controller = "qrcode", action = "index", id = UrlParameter.Optional }
                );
        }
    }
}