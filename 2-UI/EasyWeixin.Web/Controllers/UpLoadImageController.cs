using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace EasyWeixin.Web.Controllers
{
    public class UpLoadImageController : Controller
    {
        //
        // GET: /UpLoadImage/

        public ActionResult Index(UpLoadImageModel model)
        {
            if (model.UpLoadImageSrc == "" || model.UpLoadImageSrc == null)
            {
                model.UpLoadImageSrc = "http://www.placehold.it/360x200/EFEFEF/AAAAAA";
            }
            ViewData["UpLoadImageSrc"] = model.UpLoadImageSrc;
            return View(model);
        }

        [HttpPost]
        public ActionResult UpLoadImage()
        {
            UpLoadImageModel model = new UpLoadImageModel();
            if (ViewData["UpLoadImageSrc"] != null)
            {
                model.UpLoadImageSrc = ViewData["UpLoadImageSrc"].ToString();
            }
            else
            {
                model.UpLoadImageSrc = "http://www.placehold.it/360x200/EFEFEF/AAAAAA";
            }
            HttpPostedFileBase imgFile = Request.Files[0];
            if (imgFile != null && imgFile.FileName != "")
            {
                if (User.Identity.Name != "")
                {
                    //获得上传图片的名字
                    string strPath = imgFile.FileName;
                    string type = strPath.Substring(strPath.LastIndexOf(".") + 1).ToLower();
                    string name = DateTime.Now.Ticks.ToString() + "." + type;
                    //获取上传用户id
                    int userid = WebSecurity.GetUserId(User.Identity.Name);
                    string uppath = System.Web.HttpContext.Current.Server.MapPath("~/images/UpLoadImage/" + User.Identity.Name.ToString());
                    var ImageUrl = "/images/UpLoadImage/" + User.Identity.Name.ToString() + "/" + name;
                    if (!Directory.Exists(uppath))
                    {
                        Directory.CreateDirectory(uppath);
                    }
                    if (ValidateImg(type))
                    {
                        if (imgFile.ContentLength < 1024 * 1024 * 2)
                        {
                            imgFile.SaveAs(uppath + "\\" + name);
                            model.UpLoadImageSrc = ImageUrl;
                            model.Message = "上传成功!";
                        }
                        else
                        {
                            model.Message = "图片大小小于2M";
                        }
                    }
                    else
                    {
                        model.Message = "图片内容不符合格式";
                    }
                }
                else
                {
                }
            }
            else
            {
                model.Message = "上传图片不能为空";
            }
            return View("Index", model);
        }

        public bool ValidateImg(string imgName)
        {
            string[] imgType = new string[] { "gif", "jpg", "png", "bmp", "jpeg", "GIF", "JPG", "PNG", "BMP", "JPEG" };

            int i = 0;
            bool blean = false;
            string message = string.Empty;

            //判断是否为Image类型文件
            while (i < imgType.Length)
            {
                if (imgName.Equals(imgType[i].ToString()))
                {
                    blean = true;
                    break;
                }
                else if (i == (imgType.Length - 1))
                {
                    break;
                }
                else
                {
                    i++;
                }
            }
            return blean;
        }
    }

    public class UpLoadImageModel
    {
        public string UpLoadImageSrc { set; get; }

        public string Message { set; get; }
    }
}