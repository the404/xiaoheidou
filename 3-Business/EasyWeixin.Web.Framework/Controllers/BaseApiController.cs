using EasyWeixin.Helpers;
using System.Diagnostics;
using System.Web.Mvc;

namespace EasyWeixin.Web.Framework.Controllers
{
    public class BaseApiController : Controller
    {
        protected override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var mpFileVersionInfo = FileVersionInfo.GetVersionInfo(Server.MapPath("~/bin/EasyWeixin.dll"));
            var extensionFileVersionInfo = FileVersionInfo.GetVersionInfo(Server.MapPath("~/bin/EasyWeixin.MvcExtension.dll"));
            TempData["MpVersion"] = string.Format("{0}.{1}", mpFileVersionInfo.FileMajorPart, mpFileVersionInfo.FileMinorPart); //Regex.Match(fileVersionInfo.FileVersion, @"\d+\.\d+");
            TempData["ExtensionVersion"] = string.Format("{0}.{1}", extensionFileVersionInfo.FileMajorPart, extensionFileVersionInfo.FileMinorPart); //Regex.Match(fileVersionInfo.FileVersion, @"\d+\.\d+");

            base.OnResultExecuting(filterContext);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            LogHelper.WriteException(filterContext.Exception);
        }
    }
}