using Apworks.Specifications;
using EasyWeixin.Data.Repositories;
using EasyWeixin.Model;
using EasyWeixin.Web.Filters;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace EasyWeixin.Web.Controllers
{
    [InitializeSimpleMembership]
    [Authorize(Roles = "Weixin")]
    public class ResponseImageController : Controller
    {
        //
        // GET: /ResponseImage/

        private readonly IResponseImageRepository ResponseImageRepository;
        private readonly IResponseMessageRepository ResponseMessageRepository;

        public ResponseImageController(IResponseMessageRepository ResponseMessageRepository, IResponseImageRepository ResponseImageRepository)
        {
            this.ResponseMessageRepository = ResponseMessageRepository;
            this.ResponseImageRepository = ResponseImageRepository;
        }

        public ActionResult Index()
        {
            var UserId = WebSecurity.GetUserId(User.Identity.Name);
            return View(ResponseImageRepository.FindAll(Specification<ResponseImage>.Eval(o => o.UserId == UserId)).ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// 弹出层
        /// </summary>
        /// <returns></returns>
        public ActionResult DialogSelectList()
        {
            var UserId = WebSecurity.GetUserId(User.Identity.Name);
            var ResponseImage = ResponseImageRepository.FindAll(Specification<ResponseImage>.Eval(o => o.UserId == UserId));
            return View(ResponseImage.ToList());
        }

        [HttpPost]
        public ActionResult UpLoadImage()
        {
            HttpPostedFileBase imgFile = Request.Files[0];
            if (imgFile != null)
            {
                if (User.Identity.Name != "")
                {
                    //获得上传图片的名字
                    string strPath = imgFile.FileName;
                    string type = strPath.Substring(strPath.LastIndexOf(".") + 1).ToLower();

                    //获取上传用户id
                    int userid = WebSecurity.GetUserId(User.Identity.Name);
                    ResponseImage ResponseImage = new ResponseImage();
                    ResponseImage.AddTime = DateTime.Now;
                    ResponseImage.UserId = userid;
                    ResponseImage.ImageName = imgFile.FileName.Replace("." + type, "");
                    string uppath = System.Web.HttpContext.Current.Server.MapPath("~/images/ResponseImage/" + User.Identity.Name.ToString());
                    ResponseImage.ImageUrl = "/images/ResponseImage/" + User.Identity.Name.ToString() + "/" + imgFile.FileName;
                    if (!Directory.Exists(uppath))
                    {
                        Directory.CreateDirectory(uppath);
                    }
                    if (ValidateImg(type))
                    {
                        if (imgFile.ContentLength < 1024 * 1024 * 2)
                        {
                            if (ResponseImageRepository.Find(Specification<ResponseImage>.Eval(o => o.ImageName == strPath.Replace("." + type, "") && o.UserId == userid)) == null)
                            {
                                imgFile.SaveAs(uppath + "\\" + imgFile.FileName);
                                if (ModelState.IsValid)
                                {
                                    ResponseImageRepository.Add(ResponseImage);
                                    ResponseImageRepository.Context.Commit();
                                    return Redirect("/ResponseImage/Index");
                                }
                            }
                            else
                            {
                                TempData["ErrorMessage"] = "图片名称已经存在";
                            }
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "图片大小小于2M";
                        }
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "图片内容不符合格式";
                    }
                }
                else
                {
                }
            }
            else
            {
                TempData["ErrorMessage"] = "上传图片不能为空";
            }
            return Redirect("/ResponseImage/Create");
        }

        public ActionResult Edit(Guid id)
        {
            ResponseImage ResponseImage = ResponseImageRepository.GetByKey(id);
            if (ResponseImage == null)
            {
                return HttpNotFound();
            }
            return View(ResponseImage);
        }

        [HttpPost]
        public ActionResult Edit(ResponseImage ResponseImage)
        {
            ResponseImage.UserId = WebSecurity.GetUserId(User.Identity.Name);
            ResponseImage.AddTime = DateTime.Now;
            if (ModelState.IsValid)
            {
                ResponseImageRepository.Update(ResponseImage);
                ResponseImageRepository.Context.Commit();

                return Redirect("/ResponseImage/Index");
            }
            return View(ResponseImage);
        }

        public ActionResult Delete(Guid id)
        {
            ResponseImage ResponseImage = ResponseImageRepository.GetByKey(id);
            foreach (var item in ResponseImage.ResponseMessages)
            {
                ResponseMessageRepository.Remove(item);
                ResponseMessageRepository.Context.Commit();
            }
            ResponseImageRepository.Remove(ResponseImage);
            ResponseImageRepository.Context.Commit();
            return Redirect("/ResponseImage/Index");
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
}