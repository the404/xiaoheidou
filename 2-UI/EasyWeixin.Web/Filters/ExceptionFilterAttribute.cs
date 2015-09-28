using EasyWeixin.Helpers;
using System;
using System.Diagnostics;
using System.Web.Mvc;

namespace EasyWeixin.Web.Filters
{
    public class ExceptionFilterAttribute : IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                LogHelper.WriteException(filterContext.Exception);
                filterContext.ExceptionHandled = true;
            }
            throw filterContext.Exception;
        }
    }
}