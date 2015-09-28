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
    public class ResponseMusicController : Controller
    {
        //
        // GET: /ResponseMusic/
        private readonly IResponseMusicRepository ResponseMusicRepository;

        public ResponseMusicController(IResponseMusicRepository ResponseMusicRepository)
        {
            this.ResponseMusicRepository = ResponseMusicRepository;
        }

        public ActionResult Index()
        {
            var UserId = WebSecurity.GetUserId(User.Identity.Name);
            var ResponseMusic = ResponseMusicRepository.FindAll(Specification<ResponseMusic>.Eval(o => o.UserId == UserId));
            return View(ResponseMusic.ToList());
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
            var ResponseMusic = ResponseMusicRepository.FindAll(Specification<ResponseMusic>.Eval(o => o.UserId == UserId));
            return View(ResponseMusic.ToList());
        }

        [HttpPost]
        public ActionResult Create(ResponseMusic ResponseMusic)
        {
            if (ModelState.IsValid)
            {
                ResponseMusicRepository.Add(ResponseMusic);
                ResponseMusicRepository.Context.Commit();
                return RedirectToAction("Index");
            }
            return View(ResponseMusic);
        }

        public ActionResult Edit(Guid id)
        {
            ResponseMusic ResponseMusic = ResponseMusicRepository.GetByKey(id);
            if (ResponseMusic == null)
            {
                return HttpNotFound();
            }
            return View(ResponseMusic);
        }

        [HttpPost]
        public ActionResult UpLoadMusic()
        {
            HttpPostedFileBase File = Request.Files[0];
            if (File != null)
            {
                if (User.Identity.Name != "")
                {
                    //获得上传的名字
                    string strPath = File.FileName;
                    string type = strPath.Substring(strPath.LastIndexOf(".") + 1).ToLower();
                    if (ValidateMusic(type))
                    {
                        //获取上传用户id
                        int userid = WebSecurity.GetUserId(User.Identity.Name);

                        ResponseMusic ResponseMusic = new ResponseMusic();
                        ResponseMusic.AddTime = DateTime.Now;
                        ResponseMusic.UserId = userid;
                        ResponseMusic.MusicName = File.FileName.Replace("." + type, "");
                        string uppath = System.Web.HttpContext.Current.Server.MapPath("~/images/ResponseMusic/" + userid.ToString());
                        ResponseMusic.MusicUrl = "/images/ResponseMusic/" + userid.ToString() + "/" + File.FileName;
                        if (!Directory.Exists(uppath))
                        {
                            Directory.CreateDirectory(uppath);
                        }

                        if (File.ContentLength < 1024 * 1024 * 5)
                        {
                            if (ResponseMusicRepository.Find(Specification<ResponseMusic>.Eval(o => o.MusicName == strPath.Replace("." + type, "") && o.UserId == userid)) == null)
                            {
                                File.SaveAs(uppath + "\\" + File.FileName);
                                if (ModelState.IsValid)
                                {
                                    ResponseMusicRepository.Add(ResponseMusic);
                                    ResponseMusicRepository.Context.Commit();
                                    return Redirect("/ResponseMusic/Index");
                                }
                            }
                            else
                            {
                                TempData["ErrorMessage"] = "语音名称已经存在";
                            }
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "语音大小小于5M";
                        }
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "格式不对";
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
            return Redirect("/ResponseMusic/Create");
        }

        [HttpPost]
        public ActionResult Edit(ResponseMusic ResponseMusic)
        {
            if (ModelState.IsValid)
            {
                ResponseMusicRepository.Update(ResponseMusic);
                ResponseMusicRepository.Context.Commit();

                return Redirect("/ResponseMusic/Index");
            }
            return View(ResponseMusic);
        }

        public ActionResult Delete(Guid id)
        {
            ResponseMusic ResponseMusic = ResponseMusicRepository.GetByKey(id);
            ResponseMusicRepository.Remove(ResponseMusic);
            ResponseMusicRepository.Context.Commit();
            return Redirect("/ResponseMusic/Index");
        }

        public bool ValidateMusic(string Name)
        {
            string[] Type = new string[] { "mp3", "wma", "wav", "amr" };

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