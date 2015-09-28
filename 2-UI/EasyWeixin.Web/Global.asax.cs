using EasyWeixin.Helpers;
using EasyWeixin.Web.Mappers;
using Newtonsoft.Json;
using StackExchange.Profiling;
using System;
using System.IO;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebMatrix.WebData;

namespace EasyWeixin.Web
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.SetupInformation.ApplicationBase);
            this.Error += MvcApplication_Error;

            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            Bootstrapper.Initialise();

            AutoMapperConfiguration.Configure();

            WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
        }

        protected void Application_BeginRequest()
        {
            if (Request.IsLocal)
            {
                MiniProfiler.Start();
            }
        }

        protected void Application_EndRequest()
        {
            MiniProfiler.Stop();
        }

        private void MvcApplication_Error(object sender, System.EventArgs e)
        {
            var ex = Server.GetLastError();
            var str = JsonConvert.SerializeObject(ex, Formatting.Indented);
            var directoryname = Directory.GetCurrentDirectory() + @"\logs\";
            var filename = ex.Message + DateTime.Now.ToString("yyyyMMdd hhmmssfff") + ".json";
            if (!Directory.Exists(directoryname))
                Directory.CreateDirectory(directoryname);
            File.WriteAllText(directoryname + filename, str);
            //todo 跳转到一个404页面
        }
    }
}