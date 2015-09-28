using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EasyWeixin.Web.Controllers
{
    public class ChatLineController : Controller
    {
        //
        // GET: /ChatLine/

        public ActionResult Index()
        {
            return View();
        }


        public JsonResult test(string Sign,string Timesp,string Non)
        {
            //接收并读取POST过来的XML文件流
            StreamReader reader = new StreamReader(Request.InputStream);
            String xmlData = reader.ReadToEnd();
            //把数据重新返回给客户端
            Response.Write("好吧，我也是醉了");
            Response.End();
            return Json(new {ss=xmlData }, JsonRequestBehavior.AllowGet);
         }


    }
}
