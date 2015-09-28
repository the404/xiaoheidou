using System.Web.Mvc;

namespace EasyWeixin.Web.Areas.Activity
{
    public class ActivityAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Activity";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Activity_default",
                "Activity/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}