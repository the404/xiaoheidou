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
    public class ResponseVideoController : Controller
    {
        //
        // GET: /ResponseVideo/
        private readonly IResponseVideoRepository ResponseVideoRepository;

        public ResponseVideoController(IResponseVideoRepository ResponseVideoRepository)
        {
            this.ResponseVideoRepository = ResponseVideoRepository;
        }

        public ActionResult Index()
        {
            var UserId = WebSecurity.GetUserId(User.Identity.Name);
            var ResponseVideo = ResponseVideoRepository.FindAll(Specification<ResponseVideo>.Eval(o => o.UserId == UserId));
            return View(ResponseVideo.ToList());
        }

        /// <summary>
        /// 弹出层
        /// </summary>
        /// <returns></returns>
        public ActionResult DialogSelectList()
        {
            var UserId = WebSecurity.GetUserId(User.Identity.Name);
            var ResponseVideo = ResponseVideoRepository.FindAll(Specification<ResponseVideo>.Eval(o => o.UserId == UserId));
            return View(ResponseVideo.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UpLoadVideo()
        {
            HttpPostedFileBase imgFile = Request.Files[0];
            if (imgFile != null)
            {
                if (User.Identity.Name != "")
                {
                    //获得上传图片的名字
                    string strPath = imgFile.FileName;
                    string type = strPath.Substring(strPath.LastIndexOf(".") + 1).ToLower();
                    if (ValidateVideo(type))
                    {
                        //获取上传用户id
                        int userid = WebSecurity.GetUserId(User.Identity.Name);
                        ResponseVideo ResponseVideo = new ResponseVideo();
                        ResponseVideo.AddTime = DateTime.Now;
                        ResponseVideo.UserId = userid;
                        ResponseVideo.VideoName = imgFile.FileName.Replace("." + type, "");
                        string uppath = System.Web.HttpContext.Current.Server.MapPath("~/images/ResponseVideo/" + userid.ToString());
                        ResponseVideo.VideoUrl = "/images/ResponseVideo/" + userid.ToString() + "/" + imgFile.FileName;
                        if (!Directory.Exists(uppath))
                        {
                            Directory.CreateDirectory(uppath);
                        }

                        if (imgFile.ContentLength < 1024 * 1024 * 20)
                        {
                            if (ResponseVideoRepository.Find(Specification<ResponseVideo>.Eval(o => o.VideoName == strPath.Replace("." + type, "") && o.UserId == userid)) == null)
                            {
                                imgFile.SaveAs(uppath + "\\" + imgFile.FileName);
                                if (ModelState.IsValid)
                                {
                                    ResponseVideoRepository.Add(ResponseVideo);
                                    ResponseVideoRepository.Context.Commit();
                                    return Redirect("/ResponseVideo/Index");
                                }
                            }
                            else
                            {
                                TempData["ErrorMessage"] = "语音名称已经存在";
                            }
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "语音大小小于20M";
                        }
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "类型格式错误";
                    }
                }
                else
                {
                }
            }
            else
            {
                TempData["ErrorMessage"] = "上传语音不能为空";
            }
            return Redirect("/ResponseVideo/Create");
        }

        [HttpPost]
        public ActionResult Create(ResponseVideo ResponseVideo)
        {
            if (ModelState.IsValid)
            {
                ResponseVideoRepository.Add(ResponseVideo);
                ResponseVideoRepository.Context.Commit();
                return RedirectToAction("Index");
            }
            return View(ResponseVideo);
        }

        public ActionResult Edit(Guid id)
        {
            ResponseVideo ResponseVideo = ResponseVideoRepository.GetByKey(id);
            if (ResponseVideo == null)
            {
                return HttpNotFound();
            }
            return View(ResponseVideo);
        }

        [HttpPost]
        public ActionResult Edit(ResponseVideo ResponseVideo)
        {
            if (ModelState.IsValid)
            {
                ResponseVideoRepository.Update(ResponseVideo);
                ResponseVideoRepository.Context.Commit();

                return RedirectToAction("Index");
            }
            return View(ResponseVideo);
        }

        public ActionResult Delete(Guid id)
        {
            ResponseVideo ResponseVideo = ResponseVideoRepository.GetByKey(id);
            ResponseVideoRepository.Remove(ResponseVideo);
            ResponseVideoRepository.Context.Commit();
            return RedirectToAction("Index");
        }

        public bool ValidateVideo(string Name)
        {
            string[] Type = new string[] { "rm", "rmvb", "wmv", "avi", "mpg", "mpeg", "mp4" };

            int i = 0;
            bool blean = false;
            string message = string.Empty;

            //判断是否为Image类型文件
            while (i < Type.Length)
            {
                if (Name.Equals(Type[i].ToString()))
                {
                    blean = true;
                    break;
                }
                else if (i == (Type.Length - 1))
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