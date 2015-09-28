using EasyWeixin.Web;
using EasyWeixin.Web.Hubs;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;
using System;
using System.Diagnostics;
using System.IO;

[assembly: OwinStartup(typeof(Startup))]

namespace EasyWeixin.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Map("/signalr", map =>
            {
                map.UseCors(CorsOptions.AllowAll);

                GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => new UserIdProvider());

                map.RunSignalR();
            });
        }
    }
}