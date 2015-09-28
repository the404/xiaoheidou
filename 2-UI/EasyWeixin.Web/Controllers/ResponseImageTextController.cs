using Apworks.Specifications;
using AutoMapper;
using EasyWeixin.Core.Common;
using EasyWeixin.Data.Repositories;
using EasyWeixin.Model;
using EasyWeixin.Web.Filters;
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
    [Authorize(Roles = "Weixin")]
    public class ResponseImageTextController : Controller
    {
        //
        // GET: /ResponseImageText/
        private readonly IResponseImageTextRepository ResponseImageTextRepository;

        private readonly IResponseMessageRepository ResponseMessageRepository;

        public ResponseImageTextController(IResponseMessageRepository ResponseMessageRepository, IResponseImageTextRepository ResponseImageTextRepository)
        {
            this.ResponseImageTextRepository = ResponseImageTextRepository;
            this.ResponseMessageRepository = ResponseMessageRepository;
        }

        public ActionResult Index()
        {
            var ResponseImageText = GetResponseImageTexts();
            return View(ResponseImageText.ToList());
        }

        private IOrderedEnumerable<ResponseImageText> GetResponseImageTexts()
        {
            var UserId = WebSecurity.CurrentUserId;
            var ResponseImageText =
                ResponseImageTextRepository.FindAll(
                    Specification<ResponseImageText>.Eval(o => o.UserId == UserId && o.ImageTextType == 0))
                    .ToList()
                    .OrderByDescending(o => o.AddTime);
            return ResponseImageText;
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
            var ResponseImageText = GetResponseImageTexts();
            return View(ResponseImageText.ToList());
        }

        [HttpPost]
        public ActionResult Create(ResponseImageTextViewModel form)
        {
            if(string.IsNullOrEmpty(form.ImageTextName))
            {
                ModelState.AddModelError("","标题不能为空");
                return View(form);
            }
            if (ModelState.IsValid)
            {
                form.UserId = WebSecurity.GetUserId(User.Identity.Name);
                form.ImageTextType = 0;
                form.AddTime = DateTime.Now;
                if (string.IsNullOrEmpty(form.PicUrl))
                {
                    form.PicUrl = GetImageUrl(form.Content);
                }

                var model = Mapper.Map<ResponseImageTextViewModel, ResponseImageText>(form);
                ResponseImageTextRepository.Add(model);
                ResponseImageTextRepository.Context.Commit();
                if (string.IsNullOrEmpty(form.Url))
                {
                    model.Url = "http://" + Request.Url.Host + "/News/Index?ImageTextID=" + model.ID;
                    ResponseImageTextRepository.Update(model);
                    ResponseImageTextRepository.Context.Commit();
                }
                return Redirect("/ResponseImageText/Index");
            }
            return View(form);
        }

        public ActionResult Edit(Guid id)
        {
            ResponseImageText ResponseImageText = ResponseImageTextRepository.GetByKey(id);
            if (ResponseImageText == null)
            {
                return HttpNotFound();
            }
            var viewModel = Mapper.Map<ResponseImageText, ResponseImageTextViewModel>(ResponseImageText);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(ResponseImageTextViewModel form)
        {
            if (string.IsNullOrEmpty(form.ImageTextName))
            {
                ModelState.AddModelError("", "标题不能为空");
                return View(form);
            }
            if (ModelState.IsValid)
            {
                form.UserId = WebSecurity.GetUserId(User.Identity.Name);
                form.ImageTextType = 0;
                form.AddTime = DateTime.Now;

                var model = Mapper.Map<ResponseImageTextViewModel, ResponseImageText>(form);
                if (string.IsNullOrEmpty(form.Url))
                {
                    model.Url = "http://" + Request.Url.Host + "/News/Index?ImageTextID=" + model.ID;
                }
                ResponseImageTextRepository.Update(model);
                ResponseImageTextRepository.Context.Commit();
                return Redirect("/ResponseImageText/Index");
            }
            return View(form);
        }

        /// <summary>
        /// 获取内容页第一张图片
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        private string GetImageUrl(string html)
        {
            if (string.IsNullOrEmpty(html))
                return "";
            var ImageUrl = HtmlStringHelper.GetHtmlImageUrlList(html).Length > 0 ? HtmlStringHelper.GetHtmlImageUrlList(html)[0] : "";
            if (!ImageUrl.Contains("http") && !ImageUrl.Contains("weixin.ipow.cn") && ImageUrl != "")
            {
                ImageUrl = "http://" + Request.Url.Host + "" + ImageUrl;
            }
            return ImageUrl;
        }

        public ActionResult Delete(Guid id)
        {
            ResponseImageText ResponseImageText = ResponseImageTextRepository.GetByKey(id);
            if (ResponseImageText != null)
            {
                //by tianxiu 2014-3-21

                var remsg = ResponseImageText.ResponseMessages;
                if (remsg.Count >= 2)
                {
                    foreach (var item in remsg)
                    {
                        ResponseMessageRepository.Remove(item);
                        ResponseMessageRepository.Context.Commit();
                    }
                }
                else if (remsg.Count == 1)
                {
                    ResponseMessageRepository.Remove(remsg.FirstOrDefault());
                    ResponseMessageRepository.Context.Commit();
                }

                ResponseImageTextRepository.Remove(ResponseImageText);
                ResponseImageTextRepository.Context.Commit();
            }
            return Redirect("/ResponseImageText/Index");
        }

        /// <summary>
        /// 指定浏览远程图片的服务器端程序
        /// </summary>
        /// <param name="context"></param>
        public JsonResult file_manager_json()
        {
            HttpPostedFileBase imgFile = Request.Files[0];
            String aspxUrl = Request.Path.Substring(0, Request.Path.LastIndexOf("/") + 1);
            //根目录路径，相对路径
            String rootPath = "../images/ResponseImageText/";
            //根目录URL，可以指定绝对路径，比如 http://www.yoursite.com/attached/
            String rootUrl = aspxUrl + "../images/ResponseImageText/";
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
            String savePath = "../images/ResponseImageText/";
            //文件保存目录URL
            String saveUrl = aspxUrl + "../images/ResponseImageText/";

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
    }
}