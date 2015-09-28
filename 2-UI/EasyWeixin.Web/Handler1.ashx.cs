using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace WebApplication1
{
    /// <summary>
    /// Handler1 的摘要说明
    /// </summary>
    public class Handler1 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var result = new { Status = 200 };
            try
            {
                Dictionary<string, string> qs = new Dictionary<string, string>();
                qs.Add("groupId", "handler1");

                HubConnection hubConnection = new HubConnection("http://" + System.Web.HttpContext.Current.Request.Url.Authority + "/", qs);
                IHubProxy testHubProxy = hubConnection.CreateHubProxy("TestHub");

                ServicePointManager.DefaultConnectionLimit = 10;
                hubConnection.Start().ContinueWith(task =>
                {
                    testHubProxy.Invoke("hello", "handler");
                });
            }
            catch
            {
                result = new { Status = 500 };
            }
            finally
            {

            }
            context.Response.Write(result);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}