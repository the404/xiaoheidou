using Apworks.Specifications;
using AutoMapper;
using EasyWeixin.Data.Repositories;
using EasyWeixin.Model;
using EasyWeixin.Web.Filters;
using EasyWeixin.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace EasyWeixin.Web.Controllers
{
    [InitializeSimpleMembership]
    [Authorize(Roles = "Egg")]
    public class EggController : Controller
    {
        //微信抢红包
        // GET: /Gift/
        private readonly IResponseImageTextRepository ResponseImageTextRepository;

        private readonly IUserProfileRepository UserProfileRepository;
        private readonly IWheelUserRepository WheelUserRepository;
        private readonly IWheelRepository WheelRepository;
        private readonly IWheelItemRepository WheelItemRepository;

        private readonly IWheelLogRepository WheelLogRepository;

        public EggController(IWheelItemRepository WheelItemRepository, IWheelRepository WheelRepository,
            IWheelUserRepository WheelUserRepository,
            IResponseImageTextRepository ResponseImageTextRepository,
            IUserProfileRepository UserProfileRepository,
            IWheelLogRepository WheelLogRepository)
        {
            this.ResponseImageTextRepository = ResponseImageTextRepository;
            this.UserProfileRepository = UserProfileRepository;
            this.WheelUserRepository = WheelUserRepository;
            this.WheelRepository = WheelRepository;
            this.WheelItemRepository = WheelItemRepository;
            this.WheelLogRepository = WheelLogRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        #region 主页列表

        public ActionResult EggIndex(int pageid = 1)
        {
            var UserId = WebSecurity.GetUserId(User.Identity.Name);
            var Wheel = WheelRepository.FindAll(Specification<Wheel>.Eval(o => o.UserId == UserId && o.Mark == 2)).ToList();
            var Pagerlist = WheelRepository.GetListByPages(Wheel, pageid, 10);
            return View(Pagerlist);
        }

        #endregion 主页列表

        #region 打开创建页

        public ActionResult EggCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult EggCreate(WheelViewModel form)
        {
            var UserId = WebSecurity.GetUserId(User.Identity.Name);
            var user = UserProfileRepository.Find(Specification<EasyWeixin.Model.UserProfile>.Eval(o => o.UserId == UserId));
            form.ResponseImageTextViewModel.ImageTextName = form.WheelTitle;
            form.ResponseImageTextViewModel.ImageTextType = 101;
            form.ResponseImageTextViewModel.UserId = WebSecurity.GetUserId(User.Identity.Name);
            form.ResponseImageTextViewModel.AddTime = DateTime.Now;
            form.UserId = WebSecurity.GetUserId(User.Identity.Name);
            form.AddDate = DateTime.Now;
            form.Mark = 2;
            if (ModelState.IsValid)
            {
                Wheel Wheel = Mapper.Map<WheelViewModel, Wheel>(form);
                Wheel.WheelStyle = "Wheel.css";
                Wheel.ResponseImageText = Mapper.Map<ResponseImageTextViewModel, ResponseImageText>(form.ResponseImageTextViewModel);

                WheelRepository.Add(Wheel);
                WheelRepository.Context.Commit();
                Wheel.ResponseImageText.Url = "http://" + Request.Url.Host + "/ActivityEgg/EggIndex?WheelID=" + Wheel.ID + "&ImageTextID=" + Wheel.ResponseImageText.ID;
                Wheel.GetURL = "http://" + Request.Url.Host + "/ActivityEgg/EggIndex?WheelID=" + Wheel.ID + "&ImageTextID=" + Wheel.ResponseImageText.ID + "&User_ID=" + user.ID;
                //for (int i = 0; i < 3; i++)
                //{
                //var Angle = GetAngle(i, 3, 5);
                WheelItem wi = new WheelItem();
                wi.WheelItemScale = 0;
                // wi.WheelItemName = GetChineseNum(i + 1) + "等奖";
                wi.WheelItemName = "一等奖";
                wi.WheelItemAward = "";
                wi.isOrder = 0;
                wi.MaxAngle = "";
                wi.MinAngle = "";
                wi.WheelID = Wheel.WheelID;
                wi.AddDate = DateTime.Now;
                WheelItemRepository.Add(wi);
                WheelItemRepository.Context.Commit();
                //}
                WheelRepository.Update(Wheel);
                WheelRepository.Context.Commit();
            }
            return Redirect("/Egg/EggIndex");
        }

        #endregion 打开创建页

        #region 打开编辑页

        public ActionResult EggEdit(Guid id)
        {
            Wheel Wheel = WheelRepository.GetByKey(id);
            WheelViewModel form = Mapper.Map<Wheel, WheelViewModel>(Wheel);
            form.ResponseImageTextViewModel = Mapper.Map<ResponseImageText, ResponseImageTextViewModel>(Wheel.ResponseImageText);
            return View(form);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult EggEdit(WheelViewModel form)
        {
            if (ModelState.IsValid)
            {
                Wheel Wheel = WheelRepository.GetByKey(form.ID);
                Wheel.WheelTitle = form.WheelTitle;
                Wheel.StartDate = form.StartDate;
                Wheel.EndDate = form.EndDate;
                Wheel.WheelDesc = form.WheelDesc;
                Wheel.WheelScale = form.WheelScale;
                //by tianxiu 2014-3-13
                //Wheel.EveryDayTimes = form.EveryDayTimes;

                Wheel.ResponseImageText.ImageTextName = form.WheelTitle;
                Wheel.ResponseImageText.Content = form.ResponseImageTextViewModel.Content;
                WheelRepository.Update(Wheel);
                WheelRepository.Context.Commit();
            }
            return Redirect("/Egg/EggIndex");
        }

        #endregion 打开编辑页

        #region 删除

        public ActionResult EggDelete(Guid id)
        {
            var Wheel = WheelRepository.GetByKey(id);
            if (Wheel.WheelUsers.ToList().Count < 500)
            {
                foreach (var item in Wheel.WheelUsers.ToList())
                {
                    WheelUserRepository.Remove(item);
                    WheelUserRepository.Context.Commit();
                }
                WheelRepository.Remove(Wheel);
                WheelRepository.Context.Commit();

                //by tianxiu 2014-3-10
                if (Wheel.WheelItems != null)
                {
                    foreach (var item in Wheel.WheelItems.ToList())
                    {
                        WheelItemRepository.Remove(item);
                        WheelItemRepository.Context.Commit();
                    }
                    WheelRepository.Remove(Wheel);
                    WheelRepository.Context.Commit();
                }
            }
            return RedirectToAction("EggIndex");
        }

        #endregion 删除

        #region 大转盘用户列表

        public ActionResult EggUserList(Guid WheelID, int pageid = 1, DateTime? dt1 = null, DateTime? dt2 = null)
        {
            if (dt1 != null && dt2 != null && dt1 <= dt2)
            {
                int UserId = WebSecurity.GetUserId(User.Identity.Name);
                Wheel Wheel = WheelRepository.GetByKey(WheelID);
                dt2 = DateTime.Parse(dt2.ToString()).AddDays(1);
                var list = WheelUserRepository.FindAll(Specification<WheelUser>.Eval(o => o.UserId == UserId && o.WheelID == Wheel.WheelID && o.AddDate > dt1 && o.AddDate < dt2)).ToList();

                var Pagerlist = WheelUserRepository.GetListByPages(list, pageid, 10);
                ViewData["num"] = WheelLogRepository.FindAll(Specification<WheelLog>.Eval(o => o.WheelID == Wheel.WheelID)).GroupBy(o => o.IP).Select(o => o.First()).Count();
                ViewData["num1"] = WheelUserRepository.FindAll(Specification<WheelUser>.Eval(o => o.UserId == UserId && o.WheelID == Wheel.WheelID)).Count();
                return View(Pagerlist);
            }
            else
            {
                int UserId = WebSecurity.GetUserId(User.Identity.Name);
                Wheel Wheel = WheelRepository.GetByKey(WheelID);
                var EndDate = Wheel.EndDate.AddDays(1);
                var list = WheelUserRepository.FindAll(Specification<WheelUser>.Eval(o => o.UserId == UserId && o.WheelID == Wheel.WheelID && o.AddDate > Wheel.StartDate && o.AddDate < EndDate)).ToList();
                var Pagerlist = WheelUserRepository.GetListByPages(list, pageid, 10);
                ViewData["num"] = WheelLogRepository.FindAll(Specification<WheelLog>.Eval(o => o.WheelID == Wheel.WheelID)).GroupBy(o => o.IP).Select(o => o.First()).Count();
                ViewData["num1"] = WheelUserRepository.FindAll(Specification<WheelUser>.Eval(o => o.UserId == UserId && o.WheelID == Wheel.WheelID)).Count();
                return View(Pagerlist);
            }
        }

        #endregion 大转盘用户列表

        #region 子项列表

        public ActionResult EggItemIndex(int WheelID)
        {
            var Wheel = WheelRepository.Find(Specification<Wheel>.Eval(o => o.WheelID == WheelID));
            List<WheelItem> WheelItemList = Wheel.WheelItems.ToList();
            if (WheelItemList.Count != 0)
            {
                return View(WheelItemList);
            }
            return null;
        }

        #endregion 子项列表

        #region 子项编辑

        public ActionResult EggItemEdit(int WheelID, Guid? ID = null)
        {
            if (ID == null)
            {
                WheelItem Wheelitem = new WheelItem();
                Wheelitem.WheelID = WheelID;
                return View(Wheelitem);
            }
            else
            {
                var Wheelitem = WheelItemRepository.GetByKey(ID);
                return View(Wheelitem);
            }
        }

        [HttpPost]
        public JsonResult EggItemEdit(WheelItem form)
        {
            if (form.WheelItemID == 0)
            {
                form.AddDate = DateTime.Now;
                WheelItemRepository.Add(form);
                WheelItemRepository.Context.Commit();
                return Json(form);
            }
            else
            {
                var WheelItem = WheelItemRepository.Find(Specification<WheelItem>.Eval(o => o.WheelItemID == form.WheelItemID));
                WheelItem.WheelItemAward = form.WheelItemAward;
                WheelItem.WheelItemName = form.WheelItemName;
                WheelItem.WheelItemScale = form.WheelItemScale;
                WheelItemRepository.Update(WheelItem);
                WheelItemRepository.Context.Commit();
                return Json(WheelItem);
            }
        }

        #endregion 子项编辑

        #region 子项删除

        public ActionResult EggItemDelete(Guid id)
        {
            var WheelItem = WheelItemRepository.GetByKey(id);
            int WheelID = WheelItem.Wheel.WheelID;
            WheelItemRepository.Remove(WheelItem);
            WheelItemRepository.Context.Commit();
            return Redirect("/Egg/EggItemIndex?WheelID=" + WheelID);
        }

        #endregion 子项删除
    }
}