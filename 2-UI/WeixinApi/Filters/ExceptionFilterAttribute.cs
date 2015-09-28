using EasyWeixin.Helpers;
using System;
using System.Diagnostics;
using System.Web.Mvc;

namespace WeixinApi.Filters
{
    public class ExceptionFilterAttribute : IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                LogHelper.WriteException(filterContext.Exception);
                LogHelper.AwardHubTs.TraceEvent(TraceEventType.Error, 1, filterContext.Exception.StackTrace +
                    Environment.NewLine + filterContext.Exception.Message);
                filterContext.ExceptionHandled = true;
            }
            throw filterContext.Exception;
        }
    }
}