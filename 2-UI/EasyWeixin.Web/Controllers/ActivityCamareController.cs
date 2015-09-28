using Apworks.Specifications;
using EasyWeixin.Data.Repositories;
using EasyWeixin.Helpers;
using EasyWeixin.Model;
using EasyWeixin.Web.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace EasyWeixin.Web.Controllers
{
    public class ActivityCamareController : Controller
    {
        private readonly IUserProfileRepository UserProfileRepository;
        private readonly IPhotoWallRepository PhotoWallRepository;
        private readonly ICameraPhotoRepository CameraPhotoRepository;
        private readonly ICameraLogRepository CameraLogRepository;

        public ActivityCamareController(IPhotoWallRepository PhotoWallRepository,
            ICameraPhotoRepository CameraPhotoRepository,
            IUserProfileRepository UserProfileRepository,
            ICameraLogRepository CameraLogRepository)
        {
            this.PhotoWallRepository = PhotoWallRepository;
            this.CameraPhotoRepository = CameraPhotoRepository;
            this.UserProfileRepository = UserProfileRepository;
            this.CameraLogRepository = CameraLogRepository;
        }

        //照片墙
        // GET: /ActivityCamare/

        public ActionResult Index()
        {
            return View();
        }

        #region 上海

        public ActionResult CameraIndex(Guid PhotoID, string UserWexinID = "")
        {
            string isExits = CookieHelper.GetCookie(Request, "u_c");
            if (isExits == "")
            {
                CookieHelper.AddCookie(Response, "u_c", DateTime.Now.ToString("yyyyMMddHHmmssfff"), 365, 0, 0, 0);
            }
            //获得该链接的所有点赞数
            var pp = PhotoWallRepository.GetByKey(PhotoID);
            var Leader = CameraPhotoRepository.FindAll(Specification<CameraPhoto>.Eval(o => o.PhotoID == pp.PhotoID)).OrderByDescending(o => o.AddTime).ToList();
            List<CameraPhotoViewModel> cp = new List<CameraPhotoViewModel>();
            foreach (var item in Leader)
            {
                var cc = CameraLogRepository.FindAll(Specification<CameraLog>.Eval(o => o.CameraID == item.CameraID)).ToList();
                //判断是否已经点过赞
                var zz = CameraLogRepository.FindAll(Specification<CameraLog>.Eval(o => o.CameraID == item.CameraID && o.Cookie == isExits)).Any();

                CameraPhotoViewModel mm = new CameraPhotoViewModel();
                if (item.IsCheck)
                {
                    mm.LoveNum = cc.Count();
                    mm.Remark = item.Remark;
                    mm.PhotoID = item.PhotoID;
                    mm.Name = item.Name;
                    mm.YName = item.YName;
                    mm.ID = item.ID;
                    mm.IsZan = zz == true ? 1 : 0;
                }
                cp.Add(mm);
            }
            return View(cp);
        }

        #endregion 上海

        #region 天津

        public ActionResult TCameraIndex(Guid PhotoID, string UserWexinID = "")
        {
            string isExits = CookieHelper.GetCookie(Request, "u_c");
            if (isExits == "")
            {
                CookieHelper.AddCookie(Response, "u_c", DateTime.Now.ToString("yyyyMMddHHmmssfff"), 365, 0, 0, 0);
            }
            //获得该链接的所有点赞数
            var pp = PhotoWallRepository.GetByKey(PhotoID);
            var Leader = CameraPhotoRepository.FindAll(Specification<CameraPhoto>.Eval(o => o.PhotoID == pp.PhotoID)).OrderByDescending(o => o.AddTime).ToList();
            List<CameraPhotoViewModel> cp = new List<CameraPhotoViewModel>();
            foreach (var item in Leader)
            {
                var cc = CameraLogRepository.FindAll(Specification<CameraLog>.Eval(o => o.CameraID == item.CameraID)).ToList();
                //判断是否已经点过赞
                var zz = CameraLogRepository.FindAll(Specification<CameraLog>.Eval(o => o.CameraID == item.CameraID && o.Cookie == isExits)).Any();

                CameraPhotoViewModel mm = new CameraPhotoViewModel();
                mm.LoveNum = cc.Count();
                mm.Remark = item.Remark;
                mm.PhotoID = item.PhotoID;
                mm.Name = item.Name;
                mm.YName = item.YName;
                mm.ID = item.ID;
                mm.IsZan = zz == true ? 1 : 0;
                cp.Add(mm);
            }
            return View(cp);
        }

        #endregion 天津

        #region 武汉

        public ActionResult WCameraIndex(Guid PhotoID, string UserWexinID = "")
        {
            string isExits = CookieHelper.GetCookie(Request, "u_c");
            if (isExits == "")
            {
                CookieHelper.AddCookie(Response, "u_c", DateTime.Now.ToString("yyyyMMddHHmmssfff"), 365, 0, 0, 0);
            }
            //获得该链接的所有点赞数
            var pp = PhotoWallRepository.GetByKey(PhotoID);
            var Leader = CameraPhotoRepository.FindAll(Specification<CameraPhoto>.Eval(o => o.PhotoID == pp.PhotoID)).OrderByDescending(o => o.AddTime).ToList();
            List<CameraPhotoViewModel> cp = new List<CameraPhotoViewModel>();
            foreach (var item in Leader)
            {
                var cc = CameraLogRepository.FindAll(Specification<CameraLog>.Eval(o => o.CameraID == item.CameraID)).ToList();
                //判断是否已经点过赞
                var zz = CameraLogRepository.FindAll(Specification<CameraLog>.Eval(o => o.CameraID == item.CameraID && o.Cookie == isExits)).Any();

                CameraPhotoViewModel mm = new CameraPhotoViewModel();
                mm.LoveNum = cc.Count();
                mm.Remark = item.Remark;
                mm.PhotoID = item.PhotoID;
                mm.Name = item.Name;
                mm.YName = item.YName;
                mm.ID = item.ID;
                mm.IsZan = zz == true ? 1 : 0;
                cp.Add(mm);
            }
            return View(cp);
        }

        #endregion 武汉

        #region 云南

        public ActionResult YCameraIndex(Guid PhotoID, string UserWexinID = "")
        {
            string isExits = CookieHelper.GetCookie(Request, "u_c");
            if (isExits == "")
            {
                CookieHelper.AddCookie(Response, "u_c", DateTime.Now.ToString("yyyyMMddHHmmssfff"), 365, 0, 0, 0);
            }
            //获得该链接的所有点赞数
            var pp = PhotoWallRepository.GetByKey(PhotoID);
            var Leader = CameraPhotoRepository.FindAll(Specification<CameraPhoto>.Eval(o => o.PhotoID == pp.PhotoID)).OrderByDescending(o => o.AddTime).ToList();
            List<CameraPhotoViewModel> cp = new List<CameraPhotoViewModel>();
            foreach (var item in Leader)
            {
                var cc = CameraLogRepository.FindAll(Specification<CameraLog>.Eval(o => o.CameraID == item.CameraID)).ToList();
                //判断是否已经点过赞
                var zz = CameraLogRepository.FindAll(Specification<CameraLog>.Eval(o => o.CameraID == item.CameraID && o.Cookie == isExits)).Any();

                CameraPhotoViewModel mm = new CameraPhotoViewModel();
                mm.LoveNum = cc.Count();
                mm.Remark = item.Remark;
                mm.PhotoID = item.PhotoID;
                mm.Name = item.Name;
                mm.YName = item.YName;
                mm.ID = item.ID;
                mm.IsZan = zz == true ? 1 : 0;
                cp.Add(mm);
            }
            return View(cp);
        }

        #endregion 云南

        #region 深圳

        public ActionResult SZCameraIndex(Guid PhotoID, string UserWexinID = "")
        {
            string isExits = CookieHelper.GetCookie(Request, "u_c");
            if (isExits == "")
            {
                CookieHelper.AddCookie(Response, "u_c", DateTime.Now.ToString("yyyyMMddHHmmssfff"), 365, 0, 0, 0);
            }
            //获得该链接的所有点赞数
            var pp = PhotoWallRepository.GetByKey(PhotoID);
            var Leader = CameraPhotoRepository.FindAll(Specification<CameraPhoto>.Eval(o => o.PhotoID == pp.PhotoID)).OrderByDescending(o => o.AddTime).ToList();
            List<CameraPhotoViewModel> cp = new List<CameraPhotoViewModel>();
            foreach (var item in Leader)
            {
                var cc = CameraLogRepository.FindAll(Specification<CameraLog>.Eval(o => o.CameraID == item.CameraID)).ToList();
                //判断是否已经点过赞
                var zz = CameraLogRepository.FindAll(Specification<CameraLog>.Eval(o => o.CameraID == item.CameraID && o.Cookie == isExits)).Any();

                CameraPhotoViewModel mm = new CameraPhotoViewModel();
                mm.LoveNum = cc.Count();
                mm.Remark = item.Remark;
                mm.PhotoID = item.PhotoID;
                mm.Name = item.Name;
                mm.YName = item.YName;
                mm.ID = item.ID;
                mm.IsZan = zz == true ? 1 : 0;
                cp.Add(mm);
            }
            return View(cp);
        }

        #endregion 深圳

        #region 泰州

        public ActionResult ZCameraIndex(Guid PhotoID, string UserWexinID = "")
        {
            string isExits = CookieHelper.GetCookie(Request, "u_c");
            if (isExits == "")
            {
                CookieHelper.AddCookie(Response, "u_c", DateTime.Now.ToString("yyyyMMddHHmmssfff"), 365, 0, 0, 0);
            }
            //获得该链接的所有点赞数
            var pp = PhotoWallRepository.GetByKey(PhotoID);
            var Leader = CameraPhotoRepository.FindAll(Specification<CameraPhoto>.Eval(o => o.PhotoID == pp.PhotoID)).OrderByDescending(o => o.AddTime).ToList();
            List<CameraPhotoViewModel> cp = new List<CameraPhotoViewModel>();
            foreach (var item in Leader)
            {
                var cc = CameraLogRepository.FindAll(Specification<CameraLog>.Eval(o => o.CameraID == item.CameraID)).ToList();
                //判断是否已经点过赞
                var zz = CameraLogRepository.FindAll(Specification<CameraLog>.Eval(o => o.CameraID == item.CameraID && o.Cookie == isExits)).Any();

                CameraPhotoViewModel mm = new CameraPhotoViewModel();
                mm.LoveNum = cc.Count();
                mm.Remark = item.Remark;
                mm.PhotoID = item.PhotoID;
                mm.Name = item.Name;
                mm.YName = item.YName;
                mm.ID = item.ID;
                mm.IsZan = zz == true ? 1 : 0;
                cp.Add(mm);
            }
            return View(cp);
        }

        #endregion 泰州

        /// <summary>
        /// 插入到数据库
        /// </summary>string uname,
        /// <param name="datas"></param>
        /// <returns></returns>
        public JsonResult UploadCamera(string Remark, string PhotoID)
        {
            try
            {
                if (Session["newname"] != null)
                {
                    Guid ss = new Guid(PhotoID);
                    var pp = PhotoWallRepository.GetByKey(ss);
                    CameraPhoto form = new CameraPhoto();
                    int userID = WebSecurity.GetUserId(User.Identity.Name);
                    form.AddTime = DateTime.Now;
                    form.IpAddress = Request.UserHostAddress;
                    form.State = 1;
                    form.LoveNum = 0;
                    form.Name = Session["newname"].ToString();
                    form.YName = Session["inewname"].ToString();
                    form.Remark = Remark;
                    form.PhotoID = pp.PhotoID;
                    CameraPhotoRepository.Add(form);
                    CameraPhotoRepository.Context.Commit();
                    return Json(new { status = "ok" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

            }
            return Json(new { status = "error" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加点赞数或取消点赞
        /// </summary>

        public JsonResult AddOrDeleteLoveNum(Guid cId, string act)
        {
            var pp = CameraPhotoRepository.GetByKey(cId);

            string isExits = CookieHelper.GetCookie(Request, "u_c");
            if (isExits == "")
            {
                return null;
            }
            //判断是否已赞过
            var cc = CameraLogRepository.FindAll(Specification<CameraLog>.Eval(o => o.CameraID == pp.CameraID && o.Cookie == isExits)).Any();
            //判断是否是点赞还是取消赞
            if (act == "1" && cc == false)
            {
                //添加点赞数
                CameraLog cg = new CameraLog();
                cg.CreateDate = DateTime.Now;
                cg.CreateIp = Request.UserHostAddress;
                cg.CameraID = pp.CameraID;
                cg.Cookie = isExits;
                CameraLogRepository.Add(cg);
                CameraLogRepository.Context.Commit();
            }
            else
            {
                //取消赞
                var zlist = CameraLogRepository.FindAll(Specification<CameraLog>.Eval(o => o.CameraID == pp.CameraID && o.Cookie == isExits)).ToList();
                foreach (var item in zlist)
                {
                    if (item != null)
                    {
                        CameraLogRepository.Remove(item);
                        CameraLogRepository.Context.Commit();
                    }
                }
            }

            //获得最新的点赞集合
            var newcc = CameraLogRepository.FindAll(Specification<CameraLog>.Eval(o => o.CameraID == pp.CameraID)).ToList();
            return Json(new { lovenum = newcc.Count() }, JsonRequestBehavior.AllowGet);
        }

        #region 上传图片

        [HttpPost]
        public JsonResult Base64StringToImage(string UpLoadImageSrc)
        {
            byte[] arr = Convert.FromBase64String(UpLoadImageSrc);
            MemoryStream ms = new MemoryStream(arr);
            Bitmap bmp = new Bitmap(ms);
            string uppath = System.Web.HttpContext.Current.Server.MapPath("/images/camera/");
            var newName = System.DateTime.Now.ToFileTime();
            bmp.Save(uppath + newName + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);//保存原图

            //获得缩约图
            string thumPath = ImageHelper.GetThumbImg(200, "/images/camera/" + newName + ".jpg", "1");
            string yThumPath = ImageHelper.GetThumbImg(400, "/images/camera/" + newName + ".jpg", "2");
            Session["newname"] = thumPath;
            Session["inewname"] = yThumPath;
            ms.Close();

            return Json(new { msg = "" }, JsonRequestBehavior.AllowGet);
        }

        #endregion 上传图片
    }
}