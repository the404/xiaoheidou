using Apworks.Specifications;
using AutoMapper;
using EasyWeixin.Data.Repositories;
using EasyWeixin.Model;
using EasyWeixin.Web.Filters;
using EasyWeixin.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace EasyWeixin.Web.Controllers
{
    [InitializeSimpleMembership]
    [Authorize(Roles = "Fight")]
    public class FightController : Controller
    {
        //一战到底
        // GET: /Fight/
        private readonly IResponseImageTextRepository ResponseImageTextRepository;

        private readonly IUserProfileRepository UserProfileRepository;
        private readonly IFightUserRepository FightUserRepository;
        private readonly IFightRepository FightRepository;
        private readonly IFightLogRepository FightLogRepository;
        private readonly IFightItemRepository FightItemRepository;
        private readonly IFightUserItemRepository FightUserItemRepository;

        public FightController(IFightRepository FightRepository,
             IFightUserRepository FightUserRepository,
             IResponseImageTextRepository ResponseImageTextRepository,
             IUserProfileRepository UserProfileRepository,
             IFightLogRepository FightLogRepository,
             IFightItemRepository FightItemRepository,
             IFightUserItemRepository FightUserItemRepository)
        {
            this.ResponseImageTextRepository = ResponseImageTextRepository;
            this.UserProfileRepository = UserProfileRepository;
            this.FightUserRepository = FightUserRepository;
            this.FightRepository = FightRepository;
            this.FightLogRepository = FightLogRepository;
            this.FightItemRepository = FightItemRepository;
            this.FightUserItemRepository = FightUserItemRepository;
        }

        #region 主页列表

        public ActionResult FightIndex(int pageid = 1)
        {
            var UserId = WebSecurity.GetUserId(User.Identity.Name);
            var Fight = FightRepository.FindAll(Specification<Fight>.Eval(o => o.UserId == UserId)).ToList();
            var Pagerlist = FightRepository.GetListByPages(Fight, pageid, 10);
            return View(Pagerlist);
        }

        #endregion 主页列表

        #region 打开创建页

        public ActionResult FightCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult FightCreate(FightViewModel form)
        {
            var UserId = WebSecurity.GetUserId(User.Identity.Name);
            var user = UserProfileRepository.Find(Specification<EasyWeixin.Model.UserProfile>.Eval(o => o.UserId == UserId));
            form.ResponseImageTextViewModel.ImageTextName = form.FightTitle;
            form.ResponseImageTextViewModel.ImageTextType = 101;
            form.ResponseImageTextViewModel.UserId = WebSecurity.GetUserId(User.Identity.Name);
            form.ResponseImageTextViewModel.AddTime = DateTime.Now;
            form.UserId = WebSecurity.GetUserId(User.Identity.Name);
            form.AddDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                Fight Fight = Mapper.Map<FightViewModel, Fight>(form);
                Fight.FightStyle = "Fight.css";
                Fight.ResponseImageText = Mapper.Map<ResponseImageTextViewModel, ResponseImageText>(form.ResponseImageTextViewModel);
                FightRepository.Add(Fight);
                FightRepository.Context.Commit();
                Fight.ResponseImageText.Url = "http://" + Request.Url.Host + "/Activity/FightIndex?FightID=" + Fight.ID + "&ImageTextID=" + Fight.ResponseImageText.ID;
                Fight.GetURL = "http://" + Request.Url.Host + "/Activity/FightIndex?FightID=" + Fight.ID + "&ImageTextID=" + Fight.ResponseImageText.ID + "&User_ID=" + user.ID;
                FightRepository.Update(Fight);
                FightRepository.Context.Commit();
            }
            return Redirect("/Fight/FightIndex");
        }

        #endregion 打开创建页

        #region 打开编辑页

        public ActionResult FightEdit(Guid id)
        {
            Fight Fight = FightRepository.GetByKey(id);
            FightViewModel form = Mapper.Map<Fight, FightViewModel>(Fight);
            form.ResponseImageTextViewModel = Mapper.Map<ResponseImageText, ResponseImageTextViewModel>(Fight.ResponseImageText);
            return View(form);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult FightEdit(FightViewModel form)
        {
            if (ModelState.IsValid)
            {
                Fight Fight = FightRepository.GetByKey(form.ID);
                Fight.FightTitle = form.FightTitle;
                Fight.StartDate = form.StartDate;
                Fight.EndDate = form.EndDate;
                Fight.FightDesc = form.FightDesc;

                Fight.ResponseImageText.ImageTextName = form.FightTitle;
                Fight.ResponseImageText.Content = form.ResponseImageTextViewModel.Content;
                FightRepository.Update(Fight);
                FightRepository.Context.Commit();
            }
            return Redirect("/Fight/FightIndex");
        }

        #endregion 打开编辑页

        #region 删除

        public ActionResult FightDelete(Guid id)
        {
            var Fight = FightRepository.GetByKey(id);
            if (Fight.FightUsers.ToList().Count < 500)
            {
                foreach (var item in Fight.FightUsers.ToList())
                {
                    FightUserRepository.Remove(item);
                    FightUserRepository.Context.Commit();
                }
                FightRepository.Remove(Fight);
                FightRepository.Context.Commit();
            }
            return RedirectToAction("FightIndex");
        }

        #endregion 删除

        #region 一战到底用户列表

        public ActionResult FightUserList(Guid FightID, int pageid = 1, DateTime? dt1 = null, DateTime? dt2 = null)
        {
            if (dt1 != null && dt2 != null && dt1 <= dt2)
            {
                int UserId = WebSecurity.GetUserId(User.Identity.Name);
                Fight Fight = FightRepository.GetByKey(FightID);
                dt2 = DateTime.Parse(dt2.ToString()).AddDays(1);
                var list = FightUserRepository.FindAll(Specification<FightUser>.Eval(o => o.UserId == UserId && o.FightID == Fight.FightID && o.AddDate > dt1 && o.AddDate < dt2)).ToList();

                var Pagerlist = FightUserRepository.GetListByPages(list, pageid, 10);
                return View(Pagerlist);
            }
            else
            {
                int UserId = WebSecurity.GetUserId(User.Identity.Name);
                Fight Fight = FightRepository.GetByKey(FightID);
                var EndDate = Fight.EndDate.AddDays(1);
                var list = FightUserRepository.FindAll(Specification<FightUser>.Eval(o => o.UserId == UserId && o.FightID == Fight.FightID && o.AddDate > Fight.StartDate && o.AddDate < EndDate)).ToList();
                var Pagerlist = FightUserRepository.GetListByPages(list, pageid, 10);
                return View(Pagerlist);
            }
        }

        #endregion 一战到底用户列表

        #region 子项列表

        public ActionResult FightItemIndex(int FightID, int pageid = 1)
        {
            var Fight = FightRepository.Find(Specification<Fight>.Eval(o => o.FightID == FightID));
            List<FightItem> FightItemList = Fight.FightItems.ToList();
            var Pagerlist = FightItemRepository.GetListByPages(FightItemList, pageid, 10);
            return View(Pagerlist);
        }

        #endregion 子项列表

        #region 子项编辑

        public ActionResult FightItemEdit(int FightID, Guid? ID = null)
        {
            if (ID == null)
            {
                FightItem Fightitem = new FightItem();
                Fightitem.FightID = FightID;
                return View(Fightitem);
            }
            else
            {
                var Fightitem = FightItemRepository.GetByKey(ID);
                return View(Fightitem);
            }
        }

        [HttpPost]
        public JsonResult FightItemEdit(FightItem form)
        {
            if (form.FightItemID == 0)
            {
                form.AddDate = DateTime.Now;
                FightItemRepository.Add(form);
                FightItemRepository.Context.Commit();
                return Json(form);
            }
            else
            {
                var FightItem = FightItemRepository.Find(Specification<FightItem>.Eval(o => o.FightItemID == form.FightItemID));

                FightItem.FightItemName = form.FightItemName;
                FightItem.FightItemAnswers = form.FightItemAnswers;
                FightItem.FightItemTrueAnswer = form.FightItemTrueAnswer;
                FightItemRepository.Update(FightItem);
                FightItemRepository.Context.Commit();
                return Json(FightItem);
            }
        }

        #endregion 子项编辑

        #region 子项删除

        public ActionResult FightItemDelete(Guid id)
        {
            var FightItem = FightItemRepository.GetByKey(id);
            int FightID = FightItem.Fight.FightID;
            FightItemRepository.Remove(FightItem);
            FightItemRepository.Context.Commit();
            return Redirect("/Fight/FightItemIndex?FightID=" + FightID);
        }

        #endregion 子项删除
    }
}