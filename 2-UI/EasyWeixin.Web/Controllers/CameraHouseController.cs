using Apworks.Specifications;
using AutoMapper;
using EasyWeixin.Data.Repositories;
using EasyWeixin.Model;
using EasyWeixin.Web.Filters;
using EasyWeixin.Web.Helpers;
using EasyWeixin.Web.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace EasyWeixin.Web.Controllers
{
    [InitializeSimpleMembership]
    [Authorize(Roles = "CameraHouse")]
    public class CameraHouseController : Controller
    {
        //照片墙
        // GET: /CameraHouse/
        private readonly IUserProfileRepository UserProfileRepository;

        private readonly ICameraPhotoRepository CameraPhotoRepository;
        private readonly IPhotoWallRepository PhotoWallRepository;
        private readonly ICameraLogRepository CameraLogRepository;

        public CameraHouseController(IPhotoWallRepository PhotoWallRepository,
            ICameraPhotoRepository CameraPhotoRepository,
            IUserProfileRepository UserProfileRepository,
             ICameraLogRepository CameraLogRepository)
        {
            this.UserProfileRepository = UserProfileRepository;
            this.CameraPhotoRepository = CameraPhotoRepository;
            this.PhotoWallRepository = PhotoWallRepository;
            this.CameraLogRepository = CameraLogRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        #region 主页列表

        public ActionResult CameraIndex(int pageid = 1)
        {
            var UserId = WebSecurity.GetUserId(User.Identity.Name);
            var camera = PhotoWallRepository.FindAll(Specification<PhotoWall>.Eval(o => o.UserId == UserId)).ToList();
            var Pagerlist = PhotoWallRepository.GetListByPages(camera, pageid, 10);
            return View(Pagerlist);
        }

        #endregion 主页列表

        #region 打开创建页

        public ActionResult CameraCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult CameraCreate(CameraViewModel form)
        {
            var UserId = WebSecurity.GetUserId(User.Identity.Name);
            var user = UserProfileRepository.Find(Specification<EasyWeixin.Model.UserProfile>.Eval(o => o.UserId == UserId));
            form.UserId = WebSecurity.GetUserId(User.Identity.Name);
            form.AddDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                PhotoWall pp = Mapper.Map<CameraViewModel, PhotoWall>(form);
                PhotoWallRepository.Add(pp);
                PhotoWallRepository.Context.Commit();
                switch (UserId.ToString())
                {
                    case "28":
                        //上海
                        pp.GetURL = "http://" + Request.Url.Host + "/ActivityCamare/CameraIndex?PhotoID=" + pp.ID + "&User_ID=" + user.ID;
                        break;

                    case "27":
                        //天津
                        pp.GetURL = "http://" + Request.Url.Host + "/ActivityCamare/TCameraIndex?PhotoID=" + pp.ID + "&User_ID=" + user.ID;
                        break;

                    case "29":
                        //武汉
                        pp.GetURL = "http://" + Request.Url.Host + "/ActivityCamare/WCameraIndex?PhotoID=" + pp.ID + "&User_ID=" + user.ID;
                        break;

                    case "32":
                        //云南
                        pp.GetURL = "http://" + Request.Url.Host + "/ActivityCamare/YCameraIndex?PhotoID=" + pp.ID + "&User_ID=" + user.ID;
                        break;

                    case "31":
                        //泰州
                        pp.GetURL = "http://" + Request.Url.Host + "/ActivityCamare/ZCameraIndex?PhotoID=" + pp.ID + "&User_ID=" + user.ID;
                        break;

                    case "25":
                        //深圳
                        pp.GetURL = "http://" + Request.Url.Host + "/ActivityCamare/SZCameraIndex?PhotoID=" + pp.ID + "&User_ID=" + user.ID;
                        break;

                    default:
                        pp.GetURL = "http://" + Request.Url.Host + "/ActivityCamare/CameraIndex?PhotoID=" + pp.ID + "&User_ID=" + user.ID;
                        break;
                }

                PhotoWallRepository.Update(pp);
                PhotoWallRepository.Context.Commit();
            }
            return Redirect("/CameraHouse/CameraIndex");
        }

        #endregion 打开创建页

        #region 打开编辑页

        public ActionResult CameraEdit(Guid id)
        {
            PhotoWall pp = PhotoWallRepository.GetByKey(id);
            CameraViewModel form = Mapper.Map<PhotoWall, CameraViewModel>(pp);
            return View(form);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult CameraEdit(CameraViewModel form)
        {
            if (ModelState.IsValid)
            {
                PhotoWall pp = PhotoWallRepository.GetByKey(form.ID);
                pp.PhotoTitle = form.PhotoTitle;
                pp.PhotoDesc = form.PhotoDesc;
                PhotoWallRepository.Update(pp);
                PhotoWallRepository.Context.Commit();
            }
            return Redirect("/CameraHouse/CameraIndex");
        }

        #endregion 打开编辑页

        #region 删除

        public ActionResult CameraDelete(Guid id)
        {
            var pp = PhotoWallRepository.GetByKey(id);
            if (pp.CameraPhoto.ToList().Count < 500)
            {
                foreach (var item in pp.CameraPhoto.ToList())
                {
                    CameraPhotoRepository.Remove(item);
                    CameraPhotoRepository.Context.Commit();
                }
                PhotoWallRepository.Remove(pp);
                PhotoWallRepository.Context.Commit();
            }
            return RedirectToAction("CameraIndex");
        }

        #endregion 删除

        #region 照片列表

        public ActionResult CameraPhotoList(Guid PhotoID, int pageid = 1, DateTime? dt1 = null, DateTime? dt2 = null, bool sort = true)
        {
            try
            {
                List<CameraPhoto> cp = new List<CameraPhoto>();
                if (dt1 != null && dt2 != null && dt1 <= dt2)
                {
                    int UserId = WebSecurity.GetUserId(User.Identity.Name);
                    PhotoWall photoWall = PhotoWallRepository.GetByKey(PhotoID);
                    dt2 = DateTime.Parse(dt2.ToString()).AddDays(1);
                    var list = CameraPhotoRepository.FindAll(Specification<CameraPhoto>.Eval(o => o.PhotoID == photoWall.PhotoID && o.AddTime > dt1 && o.AddTime < dt2)).ToList();
                    if (sort)
                    {
                        list.OrderBy(s => s.IsCheck);
                    }
                    else
                    {
                        list.OrderByDescending(s => s.IsCheck);
                    }
                    foreach (var item in list)
                    {
                        var cc = CameraLogRepository.FindAll(Specification<CameraLog>.Eval(o => o.CameraID == item.CameraID)).ToList();
                        CameraPhoto mm = new CameraPhoto();
                        mm.LoveNum = cc.Count();
                        mm.Remark = item.Remark;
                        mm.PhotoID = item.PhotoID;
                        mm.Name = item.Name;
                        mm.AddTime = item.AddTime;
                        mm.ID = item.ID;
                        mm.IsCheck = item.IsCheck;
                        cp.Add(mm);
                    }

                    var Pagerlist = CameraPhotoRepository.GetListByPages(cp, pageid, 10);
                    return View(Pagerlist);
                }
                else
                {
                    int UserId = WebSecurity.GetUserId(User.Identity.Name);
                    PhotoWall p = PhotoWallRepository.GetByKey(PhotoID);
                    var list = CameraPhotoRepository.FindAll(Specification<CameraPhoto>.Eval(o => o.PhotoID == p.PhotoID)).OrderBy(s => s.IsCheck).ToList();

                    if (sort)
                    {
                        list.OrderBy(s => s.IsCheck);
                    }
                    else
                    {
                        list.OrderByDescending(s => s.IsCheck);
                    }
                    foreach (var item in list)
                    {
                        var cc = CameraLogRepository.FindAll(Specification<CameraLog>.Eval(o => o.CameraID == item.CameraID)).ToList();
                        CameraPhoto mm = new CameraPhoto();
                        mm.LoveNum = cc.Count();
                        mm.Remark = item.Remark;
                        mm.PhotoID = item.PhotoID;
                        mm.Name = item.Name;
                        mm.AddTime = item.AddTime;
                        mm.ID = item.ID;
                        mm.IsCheck = item.IsCheck;
                        cp.Add(mm);
                    }
                    var Pagerlist = CameraPhotoRepository.GetListByPages(cp, pageid, 10);
                    return View(Pagerlist);
                }
            }
            catch (Exception)
            {
                return Redirect("/CameraHouse/CameraIndex");
            }
        }

        public ActionResult DeleteAll(string IDlist, string PhotoID)
        {
            var pp = PhotoWallRepository.Find(Specification<EasyWeixin.Model.PhotoWall>.Eval(o => o.PhotoID == Convert.ToInt32(PhotoID)));
            string[] aa = IDlist.Split('|');
            for (int i = 0; i < aa.Length; i++)
            {
                var ss = CameraPhotoRepository.GetByKey(new Guid(aa[i]));
                if (ss != null)
                {
                    CameraPhotoRepository.Remove(ss);
                    CameraPhotoRepository.Context.Commit();
                }
            }
            return Redirect("/CameraHouse/CameraPhotoList?PhotoID=" + pp.ID);
        }

        public ActionResult DeleteCameraPhoto(Guid id)
        {
            var pp = CameraPhotoRepository.GetByKey(id);
            var ss = PhotoWallRepository.Find(Specification<EasyWeixin.Model.PhotoWall>.Eval(o => o.PhotoID == Convert.ToInt32(pp.PhotoID)));
            if (pp != null)
            {
                CameraPhotoRepository.Remove(pp);
                CameraPhotoRepository.Context.Commit();
            }
            return Redirect("/CameraHouse/CameraPhotoList?PhotoID=" + ss.ID);
        }

        #endregion 照片列表

        #region CheckCameraPhoto

        public ActionResult CheckCameraPhoto(Guid id)
        {
            try
            {
                var cameraPhoto = CameraPhotoRepository.GetByKey(id);
                var photoWall = PhotoWallRepository.Find(Specification<EasyWeixin.Model.PhotoWall>.Eval(o => o.PhotoID == Convert.ToInt32(cameraPhoto.PhotoID)));
                if (cameraPhoto != null)
                {
                    cameraPhoto.IsCheck = !cameraPhoto.IsCheck;
                    CameraPhotoRepository.Update(cameraPhoto);
                    CameraPhotoRepository.Context.Commit();
                }
                return Json(new JsonError { errorcode = "success", message = "" });
            }
            catch (Exception ex)
            {
                return Json(new JsonError { errorcode = "error", message = ex.Message });
            }
        }

        #endregion CheckCameraPhoto

        public ActionResult GetLink(Guid id)
        {
            try
            {
                PhotoWall pp = PhotoWallRepository.GetByKey(id);
                return View(pp);
            }
            catch (Exception)
            {
                return Redirect("/CameraHouse/CameraIndex");
            }
        }

        #region 图片上传相关代码

        /// <summary>
        /// 指定浏览远程图片的服务器端程序
        /// </summary>
        /// <param name="context"></param>
        public JsonResult file_manager_json()
        {
            HttpPostedFileBase imgFile = Request.Files[0];
            String aspxUrl = Request.Path.Substring(0, Request.Path.LastIndexOf("/") + 1);
            //根目录路径，相对路径
            String rootPath = "../images/camera/";
            //根目录URL，可以指定绝对路径，比如 http://www.yoursite.com/attached/
            String rootUrl = aspxUrl + "../images/camera/";
            //图片扩展名
            String fileTypes = "gif,jpg,jpeg,png,bmp";

            String currentPath = "";
            String currentUrl = "";
            String currentDirPath = "";
            String moveupDirPath = "";

            String dirPath = Server.MapPath(rootPath);
            String dirName = Request.Params["dir"];
            if (!String.IsNullOrEmpty(dirName))
            {
                if (Array.IndexOf("image,flash,media,file".Split(','), dirName) == -1)
                {
                    return Json(showError("Invalid Directory name."));
                }
                dirPath += dirName + "/";
                rootUrl += dirName + "/";
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
            }

            //根据path参数，设置各路径和URL
            String path = Request.Params["path"];
            path = String.IsNullOrEmpty(path) ? "" : path;
            if (path == "")
            {
                currentPath = dirPath;
                currentUrl = rootUrl;
                currentDirPath = "";
                moveupDirPath = "";
            }
            else
            {
                currentPath = dirPath + path;
                currentUrl = rootUrl + path;
                currentDirPath = path;
                moveupDirPath = Regex.Replace(currentDirPath, @"(.*?)[^\/]+\/$", "$1");
            }

            //排序形式，name or size or type
            String order = Request.Params["order"];
            order = String.IsNullOrEmpty(order) ? "" : order.ToLower();

            //不允许使用..移动到上一级目录
            if (Regex.IsMatch(path, @"\.\."))
            {
                return Json(showError("Access is not allowed."));
            }
            //最后一个字符不是/
            if (path != "" && !path.EndsWith("/"))
            {
                return Json(showError("Parameter is not valid."));
            }
            //目录不存在或不是目录
            if (!Directory.Exists(currentPath))
            {
                return Json(showError("Directory does not exist."));
            }

            //遍历目录取得文件信息
            string[] dirList = Directory.GetDirectories(currentPath);
            string[] fileList = Directory.GetFiles(currentPath);

            switch (order)
            {
                case "size":
                    Array.Sort(dirList, new NameSorter());
                    Array.Sort(fileList, new SizeSorter());
                    break;

                case "type":
                    Array.Sort(dirList, new NameSorter());
                    Array.Sort(fileList, new TypeSorter());
                    break;

                case "name":
                default:
                    Array.Sort(dirList, new NameSorter());
                    Array.Sort(fileList, new NameSorter());
                    break;
            }

            Hashtable result = new Hashtable();
            result["moveup_dir_path"] = moveupDirPath;
            result["current_dir_path"] = currentDirPath;
            result["current_url"] = currentUrl;
            result["total_count"] = dirList.Length + fileList.Length;
            List<Hashtable> dirFileList = new List<Hashtable>();
            result["file_list"] = dirFileList;
            for (int i = 0; i < dirList.Length; i++)
            {
                DirectoryInfo dir = new DirectoryInfo(dirList[i]);
                Hashtable hash = new Hashtable();
                hash["is_dir"] = true;
                hash["has_file"] = (dir.GetFileSystemInfos().Length > 0);
                hash["filesize"] = 0;
                hash["is_photo"] = false;
                hash["filetype"] = "";
                hash["filename"] = dir.Name;
                hash["datetime"] = dir.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
                dirFileList.Add(hash);
            }
            for (int i = 0; i < fileList.Length; i++)
            {
                FileInfo file = new FileInfo(fileList[i]);
                Hashtable hash = new Hashtable();
                hash["is_dir"] = false;
                hash["has_file"] = false;
                hash["filesize"] = file.Length;
                hash["is_photo"] = (Array.IndexOf(fileTypes.Split(','), file.Extension.Substring(1).ToLower()) >= 0);
                hash["filetype"] = file.Extension.Substring(1);
                hash["filename"] = file.Name;
                hash["datetime"] = file.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
                dirFileList.Add(hash);
            }
            return Json(result);
        }

        public class NameSorter : IComparer
        {
            public int Compare(object x, object y)
            {
                if (x == null && y == null)
                {
                    return 0;
                }
                if (x == null)
                {
                    return -1;
                }
                if (y == null)
                {
                    return 1;
                }
                FileInfo xInfo = new FileInfo(x.ToString());
                FileInfo yInfo = new FileInfo(y.ToString());

                return xInfo.FullName.CompareTo(yInfo.FullName);
            }
        }

        public class SizeSorter : IComparer
        {
            public int Compare(object x, object y)
            {
                if (x == null && y == null)
                {
                    return 0;
                }
                if (x == null)
                {
                    return -1;
                }
                if (y == null)
                {
                    return 1;
                }
                FileInfo xInfo = new FileInfo(x.ToString());
                FileInfo yInfo = new FileInfo(y.ToString());

                return xInfo.Length.CompareTo(yInfo.Length);
            }
        }

        public class TypeSorter : IComparer
        {
            public int Compare(object x, object y)
            {
                if (x == null && y == null)
                {
                    return 0;
                }
                if (x == null)
                {
                    return -1;
                }
                if (y == null)
                {
                    return 1;
                }
                FileInfo xInfo = new FileInfo(x.ToString());
                FileInfo yInfo = new FileInfo(y.ToString());

                return xInfo.Extension.CompareTo(yInfo.Extension);
            }
        }

        /// <summary>
        /// 指定上传文件的服务器端程序
        /// </summary>
        /// <returns></returns>
        public JsonResult upload_json()
        {
            HttpPostedFileBase imgFile = Request.Files[0];
            String aspxUrl = Request.Path.Substring(0, Request.Path.LastIndexOf("/") + 1);

            //文件保存目录路径
            String savePath = "../images/camera/";
            //文件保存目录URL
            String saveUrl = aspxUrl + "../images/camera/";

            //定义允许上传的文件扩展名
            Hashtable extTable = new Hashtable();
            extTable.Add("image", "gif,jpg,jpeg,png,bmp");
            extTable.Add("flash", "swf,flv");
            extTable.Add("media", "swf,flv,mp3,wav,wma,wmv,mid,avi,mpg,asf,rm,rmvb");
            extTable.Add("file", "doc,docx,xls,xlsx,ppt,htm,html,txt,zip,rar,gz,bz2");

            //最大文件大小
            int maxSize = 1000000;
            if (imgFile == null)
            {
                return Json(showError("请选择文件。"));
            }

            String dirPath = Server.MapPath(savePath);
            if (!Directory.Exists(dirPath))
            {
                return Json(showError("上传目录不存在。"));
            }

            String dirName = Request.Params["dir"];
            if (String.IsNullOrEmpty(dirName))
            {
                dirName = "image";
            }
            if (!extTable.ContainsKey(dirName))
            {
                return Json(showError("目录名不正确。"));
            }

            String fileName = imgFile.FileName;
            String fileExt = Path.GetExtension(fileName).ToLower();

            if (imgFile.InputStream == null || imgFile.InputStream.Length > maxSize)
            {
                return Json(showError("上传文件大小超过限制。"));
            }

            if (String.IsNullOrEmpty(fileExt) || Array.IndexOf(((String)extTable[dirName]).Split(','), fileExt.Substring(1).ToLower()) == -1)
            {
                return Json(showError("上传文件扩展名是不允许的扩展名。\n只允许" + ((String)extTable[dirName]) + "格式。"));
            }

            //创建文件夹
            dirPath += dirName + "/";
            saveUrl += dirName + "/";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            String ymd = DateTime.Now.ToString("yyyyMM", DateTimeFormatInfo.InvariantInfo);
            dirPath += ymd + "/";
            saveUrl += ymd + "/";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            String newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + fileExt;
            String filePath = dirPath + newFileName;

            imgFile.SaveAs(filePath);

            String fileUrl = saveUrl + newFileName;

            Hashtable hash = new Hashtable();
            hash["error"] = 0;
            hash["url"] = fileUrl;
            return Json(hash);
        }

        private Hashtable showError(string message)
        {
            Hashtable hash = new Hashtable();
            hash["error"] = 1;
            hash["message"] = message;
            return hash;
        }

        #endregion 图片上传相关代码
    }
}