using Apworks.Specifications;
using AutoMapper;
using EasyWeixin.Core.Common;
using EasyWeixin.Data.Repositories;
using EasyWeixin.Model;
using EasyWeixin.Web.Filters;
using EasyWeixin.Web.Helpers;
using EasyWeixin.Web.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using WebMatrix.WebData;
using System.Threading.Tasks;
using EasyWeixin.Core.MvcPager;

namespace EasyWeixin.Web.Controllers
{
    [InitializeSimpleMembership]
    [Authorize(Roles = "Scratch")]
    public class ScratchController : Controller
    {
        #region Ctor Filed

        private static readonly PixelFormat[] IndexedPixelFormats =
        {
            PixelFormat.Undefined, PixelFormat.DontCare,
            PixelFormat.Format16bppArgb1555, PixelFormat.Format1bppIndexed, PixelFormat.Format4bppIndexed,
            PixelFormat.Format8bppIndexed
        };

        private readonly IScratchItemRepository _scratchItemRepository;
        private readonly IScratchRepository _scratchRepository;
        private readonly IScratchUserRepository _scratchUserRepository;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IWeixinUserInActivityRepository _weixinUserInActivityRepository;
        private readonly IWeixinUserRepository _weixinUserRepository;

        public ScratchController(IScratchItemRepository scratchItemRepository, IScratchRepository scratchRepository,
            IScratchUserRepository scratchUserRepository, IUserProfileRepository userProfileRepository,
            IWeixinUserInActivityRepository weixinUserInActivityRepository, IWeixinUserRepository weixinUserRepository)
        {
            _userProfileRepository = userProfileRepository;
            _scratchUserRepository = scratchUserRepository;
            _scratchRepository = scratchRepository;
            _scratchItemRepository = scratchItemRepository;
            _weixinUserInActivityRepository = weixinUserInActivityRepository;
            _weixinUserRepository = weixinUserRepository;
        }

        #endregion Ctor Filed

        #region 活动

        #region 活动列表
        public  ActionResult ScratchIndex(int pageid = 1)
        {
            var userId = WebSecurity.CurrentUserId;
            var scratch = _scratchRepository.FindAll(Specification<Scratch>.Eval(o => o.UserId == userId)).ToList();
            var pagedList = _scratchRepository.GetListByPages(scratch, pageid, 10);
            return View(pagedList);
        }


        #endregion 活动列表

        #region 创建活动

        public ActionResult ScratchCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult ScratchCreate(ScratchViewModel form)
        {
            form.ResponseImageTextViewModel.ImageTextName = form.ScratchTitle;
            form.ResponseImageTextViewModel.ImageTextType = 101;
            form.ResponseImageTextViewModel.UserId = WebSecurity.CurrentUserId;
            form.ResponseImageTextViewModel.AddTime = DateTime.Now;
            form.UserId = WebSecurity.CurrentUserId;
            form.AddDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                var scratch = Mapper.Map<ScratchViewModel, Scratch>(form);
                scratch.ScratchStyle = "Scratch.css";
                scratch.ResponseImageText =
                    Mapper.Map<ResponseImageTextViewModel, ResponseImageText>(form.ResponseImageTextViewModel);
                _scratchRepository.Add(scratch);
                _scratchRepository.Context.Commit();

                scratch.ResponseImageText.Url = "http//" + Request.Url.Host + "/ActivityScratch/Index?ScratchID=";
                scratch.GetURL = "http//" + Request.Url.Host + "/ActivityScratch/Index?ScratchID=" + scratch.ID;
                _scratchRepository.Update(scratch);
                _scratchRepository.Context.Commit();

                //List<ScratchItem> ScratchItemList = Scratch.ScratchItems.ToList();
                // by tianxiu 2014-2-17
                List<ScratchItem> scratchItemList = null;
                if (scratch.ScratchItems == null)
                {
                    scratchItemList = new List<ScratchItem>();
                }
                else
                {
                    scratchItemList = scratch.ScratchItems.ToList();
                }

                if (scratchItemList.Count == 0)
                {
                    for (var i = 0; i < 3; i++)
                    {
                        var si = new ScratchItem();
                        si.ScratchItemScale = i + 1;
                        si.ScratchItemName = (i + 1) + "等奖";
                        si.ImageUrl = imageurl((i + 1) + "等奖");
                        si.ScratchItemAward = "";
                        si.isOrder = i;
                        si.ScratchID = scratch.ScratchID;
                        si.AddDate = DateTime.Now;
                        _scratchItemRepository.Add(si);
                        _scratchItemRepository.Context.Commit();
                    }
                }
                return Redirect("/Scratch/ScratchIndex");
            }
            return View(form);
        }

        #endregion 创建活动

        #region 删除活动

        public ActionResult ScratchDelete(Guid id)
        {
            var Scratch = _scratchRepository.GetByKey(id);
            if (Scratch.ScratchUsers.ToList().Count < 500)
            {
                foreach (var item in Scratch.ScratchUsers.ToList())
                {
                    _scratchUserRepository.Remove(item);
                    _scratchUserRepository.Context.Commit();
                }
                foreach (var item in Scratch.ScratchItems.ToList())
                {
                    _scratchItemRepository.Remove(item);
                    _scratchItemRepository.Context.Commit();
                }
                _scratchRepository.Remove(Scratch);
                _scratchRepository.Context.Commit();
            }
            return RedirectToAction("ScratchIndex");
        }

        #endregion 删除活动

        #region 编辑活动

        public ActionResult ScratchEdit(Guid id)
        {
            var scratch = _scratchRepository.GetByKey(id);
            var form = Mapper.Map<Scratch, ScratchViewModel>(scratch);
            form.ResponseImageTextViewModel =
                Mapper.Map<ResponseImageText, ResponseImageTextViewModel>(scratch.ResponseImageText);
            return View(form);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult ScratchEdit(ScratchViewModel scratchViewModel)
        {
            
            if (ModelState.IsValid)
            {
                var scratch = _scratchRepository.GetByKey(scratchViewModel.ID);
                scratch.ScratchTitle = scratchViewModel.ScratchTitle;
                scratch.StartDate = scratchViewModel.StartDate;
                scratch.EndDate = scratchViewModel.EndDate;
                scratch.ScratchDesc = scratchViewModel.ScratchDesc;
                scratch.ScratchScale = scratchViewModel.ScratchScale;
                scratch.GetURL = "http://" + Request.Url.Host + "/ActivityScratch/Index?ScratchID=" +
                                                scratch.ID;
                scratch.ResponseImageText.Url = "http://" + Request.Url.Host;
                scratch.ResponseImageText.ImageTextName = scratchViewModel.ScratchTitle;
                scratch.ResponseImageText.Content = scratchViewModel.ResponseImageTextViewModel.Content;
                scratch.PicUrl = scratchViewModel.PicUrl;
                _scratchRepository.Update(scratch);
                _scratchRepository.Context.Commit();
                return Redirect("/Scratch/ScratchIndex");
            }
            return View(scratchViewModel);
        }

        #endregion 编辑活动

        #endregion 活动

        #region 奖项

        #region 奖项列表

        public ActionResult ScratchItemIndex(int ScratchID)
        {
            var Scratch = _scratchRepository.Find(Specification<Scratch>.Eval(o => o.ScratchID == ScratchID));
            var ScratchItemList = Scratch.ScratchItems.ToList();

            return View(ScratchItemList);
        }

        #endregion 奖项列表

        #region 奖项编辑

        public ActionResult ScratchItemEdit(int ScratchID, Guid? ID = null)
        {
            if (ID == null)
            {
                var scratchitem = new ScratchItem { ScratchID = ScratchID };
                return View(scratchitem);
            }
            else
            {
                var scratchitem = _scratchItemRepository.GetByKey(ID);
                return View(scratchitem);
            }
        }

        [HttpPost]
        public JsonResult ScratchItemEdit(ScratchItem form)
        {
            if (form.ScratchItemID == 0)
            {
                form.AddDate = DateTime.Now;
                form.ImageUrl = imageurl(form.ScratchItemName);
                _scratchItemRepository.Add(form);
                _scratchItemRepository.Context.Commit();
                return Json(form);
            }
            var scratchItem =
                _scratchItemRepository.Find(Specification<ScratchItem>.Eval(o => o.ScratchItemID == form.ScratchItemID));
            scratchItem.ScratchItemAward = form.ScratchItemAward;
            scratchItem.ScratchItemName = form.ScratchItemName;
            scratchItem.ImageUrl = imageurl(form.ScratchItemName);
            scratchItem.ScratchItemScale = form.ScratchItemScale;
            _scratchItemRepository.Update(scratchItem);
            _scratchItemRepository.Context.Commit();
            return Json(scratchItem);
        }

        #endregion 奖项编辑

        #region 奖项删除

        public ActionResult ScratchItemDelete(Guid id)
        {
            var scratchItem = _scratchItemRepository.GetByKey(id);
            var scratchId = scratchItem.Scratch.ScratchID;
            _scratchItemRepository.Remove(scratchItem);
            _scratchItemRepository.Context.Commit();
            return Redirect("/Scratch/ScratchItemIndex?ScratchID=" + scratchId);
        }

        #endregion 奖项删除

        #endregion 奖项

        #region 查看活动链接

        /// <summary>
        /// 查看活动链接
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetLink(Guid id)
        {
            try
            {
                var scratch = _scratchRepository.GetByKey(id);
                return View(scratch);
            }
            catch (Exception)
            {
                return Redirect("/Scratch/ScratchIndex");
            }
        }

        #endregion 查看活动链接

        #region 参与该活动的用户列表

        public ActionResult ScratchUserList(Guid? scratchId, int pageid = 1, DateTime? dt1 = null, DateTime? dt2 = null)
        {
            try
            {
                var scratch = _scratchRepository.GetByKey(scratchId);
                var weixinUserList = _weixinUserRepository.FindAll().Where(s => !string.IsNullOrWhiteSpace(s.Openid));
                var weixinUserInActivityList = _weixinUserInActivityRepository.FindAll().Where(s =>
                        s.ActType == ActType.Scratch &&
                        s.ActId == scratch.ScratchID).OrderByDescending(s => s.SumCount)
                        .Join(weixinUserList, s => s.WeixinUserId, b => b.WeixinUserId, (s, b) => s)
                        .ToList();
                if (dt1 != null && dt2 != null && dt1 <= dt2)
                {
                    dt2 = DateTime.Parse(dt2.ToString()).AddDays(1);
                    weixinUserInActivityList = weixinUserInActivityList.Where(s =>
                            s.ActType == ActType.Scratch &&
                            s.AddDate >= dt1 && s.AddDate <= dt2)
                            .ToList();
                }
                var pagedList = _weixinUserInActivityRepository.GetListByPages(weixinUserInActivityList, pageid, 10);
                return View(pagedList);
            }
            catch
            {
                return View();
            }
        }

        #endregion 参与该活动的用户列表

        #region 查看中奖

        /// <summary>
        /// 获取获奖用户的信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetAwardDatas(Guid? id, int pageid = 1)
        {
            try
            {
                var scratch = _scratchRepository.GetByKey(id);
                var awardlist = _scratchUserRepository.FindAll().Where(s =>
                    s.ScratchID == scratch.ScratchID &&
                    !string.IsNullOrWhiteSpace(s.Name) &&
                    !string.IsNullOrWhiteSpace(s.Phone))
                    .OrderBy(s => s.AddDate).ToList();
                var pagedList = _scratchUserRepository.GetListByPages(awardlist, pageid, 10);
                return View(pagedList);
            }
            catch (Exception)
            {
                return Redirect("/Scratch/ScratchIndex");
            }
        }

        /// <summary>
        /// 兑奖
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAward(Guid? id)
        {
            if (id == null)
            {
                return Json(new JsonError { errorcode = "error", message = "兑奖失败，请刷新重试" }, JsonRequestBehavior.AllowGet);
            }
            var scratchUser = _scratchUserRepository.GetByKey(id);
            if (scratchUser != null)
            {
                scratchUser.IsAward = true;
                scratchUser.AwardDate = DateTime.Now.ToString(CultureInfo.InvariantCulture);
                _scratchUserRepository.Update(scratchUser);
                _scratchUserRepository.Context.Commit();
                return Json(new JsonError { errorcode = "ok", message = "兑奖成功" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new JsonError { errorcode = "error", message = "兑奖失败，不存在该用户的中奖信息" }, JsonRequestBehavior.AllowGet);
        }

        #endregion 查看中奖

        #region Helper Method

        private string imageurl(string imagename)
        {
            //定义一个服务器路径
            var name = Function.MD5(imagename) + ".jpg";
            var newpath = System.Web.HttpContext.Current.Server.MapPath("~/images/Activity/Scratch/" + name);
            if (System.IO.File.Exists(newpath))
            {
                return "/images/Activity/Scratch/" + name;
            }
            var oripath = System.Web.HttpContext.Current.Server.MapPath("~/images/Activity/Scratch/ggl.jpg");
            var img = Image.FromFile(oripath);
            Bitmap bmp;
            //如果原图片是索引像素格式之列的，则需要转换
            if (IsPixelFormatIndexed(img.PixelFormat))
            {
                bmp = new Bitmap(img.Width, img.Height, PixelFormat.Format32bppArgb);
            }
            else
            {
                bmp = new Bitmap(oripath);
            }
            float w = bmp.Width / 2;
            var g = Graphics.FromImage(bmp);
            var str = imagename;
            var font = new Font("宋体", 40);
            var sbrush = new SolidBrush(Color.White);
            g.DrawString(str, font, sbrush, new PointF(10, 20));
            var ms = new MemoryStream();
            bmp.Save(newpath, ImageFormat.Jpeg);
            return "/images/Activity/Scratch/" + name;
        }

        /// <summary>
        /// 如果原图片是索引像素格式之列的，则需要转换
        /// </summary>
        /// <param name="imgPixelFormat"></param>
        /// <returns></returns>
        private static bool IsPixelFormatIndexed(PixelFormat imgPixelFormat)
        {
            foreach (var pf in IndexedPixelFormats)
            {
                if (pf.Equals(imgPixelFormat)) return true;
            }
            return false;
        }

        #endregion Helper Method
    }
}