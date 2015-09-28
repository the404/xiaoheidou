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
    [Authorize(Roles = "Snow")]
    public class SnowController : Controller
    {
        //
        // GET: /Snow/

        private readonly IResponseImageTextRepository ResponseImageTextRepository;
        private readonly IUserProfileRepository UserProfileRepository;
        private readonly ISnowUserRepository SnowUserRepository;
        private readonly ISnowRepository SnowRepository;
        private readonly ISnowItemRepository SnowItemRepository;

        public SnowController(ISnowItemRepository SnowItemRepository, ISnowRepository SnowRepository, ISnowUserRepository SnowUserRepository, IResponseImageTextRepository ResponseImageTextRepository, IUserProfileRepository UserProfileRepository)
        {
            this.ResponseImageTextRepository = ResponseImageTextRepository;
            this.UserProfileRepository = UserProfileRepository;
            this.SnowUserRepository = SnowUserRepository;
            this.SnowRepository = SnowRepository;
            this.SnowItemRepository = SnowItemRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        #region 主页列表

        public ActionResult SnowIndex(int pageid = 1)
        {
            var UserId = WebSecurity.GetUserId(User.Identity.Name);
            var Snow = SnowRepository.FindAll(Specification<Snow>.Eval(o => o.UserId == UserId)).Where(o => o.Mark == 1).ToList();
            var Pagerlist = SnowRepository.GetListByPages(Snow, pageid, 10);
            return View(Pagerlist);
        }

        #endregion 主页列表
        #region 打开创建页

        public ActionResult SnowCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult SnowCreate(SnowViewModel form)
        {
            var UserId = WebSecurity.GetUserId(User.Identity.Name);
            var user = UserProfileRepository.Find(Specification<EasyWeixin.Model.UserProfile>.Eval(o => o.UserId == UserId));
            form.ResponseImageTextViewModel.ImageTextName = form.SnowTitle;
            form.ResponseImageTextViewModel.ImageTextType = 101;
            form.ResponseImageTextViewModel.UserId = WebSecurity.GetUserId(User.Identity.Name);
            form.ResponseImageTextViewModel.AddTime = DateTime.Now;
            form.UserId = WebSecurity.GetUserId(User.Identity.Name);
            form.AddDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                Snow Snow = Mapper.Map<SnowViewModel, Snow>(form);
                Snow.SnowStyle = "Snow.css";
                Snow.Mark = 1;
                Snow.ResponseImageText = Mapper.Map<ResponseImageTextViewModel, ResponseImageText>(form.ResponseImageTextViewModel);
                SnowRepository.Add(Snow);
                SnowRepository.Context.Commit();
                Snow.ResponseImageText.Url = "http://" + Request.Url.Host + "/ActivitySnow/SnowIndex?SnowID=" + Snow.ID + "&ImageTextID=" + Snow.ResponseImageText.ID;
                Snow.GetURL = "http://" + Request.Url.Host + "/ActivitySnow/SnowIndex?SnowID=" + Snow.ID + "&ImageTextID=" + Snow.ResponseImageText.ID + "&User_ID=" + user.ID;
                SnowRepository.Update(Snow);
                SnowRepository.Context.Commit();
            }
            return Redirect("/Snow/SnowIndex");
        }

        #endregion 打开创建页
        #region 打开编辑页

        public ActionResult SnowEdit(Guid id)
        {
            Snow Snow = SnowRepository.GetByKey(id);
            SnowViewModel form = Mapper.Map<Snow, SnowViewModel>(Snow);
            form.ResponseImageTextViewModel = Mapper.Map<ResponseImageText, ResponseImageTextViewModel>(Snow.ResponseImageText);
            return View(form);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult SnowEdit(SnowViewModel form)
        {
            if (ModelState.IsValid)
            {
                Snow Snow = SnowRepository.GetByKey(form.ID);
                Snow.SnowTitle = form.SnowTitle;
                Snow.StartDate = form.StartDate;
                Snow.EndDate = form.EndDate;
                Snow.SnowDesc = form.SnowDesc;
                Snow.SnowScale = form.SnowScale;

                Snow.ResponseImageText.ImageTextName = form.SnowTitle;
                Snow.ResponseImageText.Content = form.ResponseImageTextViewModel.Content;
                SnowRepository.Update(Snow);
                SnowRepository.Context.Commit();
            }
            return Redirect("/Snow/SnowIndex");
        }

        #endregion 打开编辑页
        #region 删除

        public ActionResult SnowDelete(Guid id)
        {
            var Snow = SnowRepository.GetByKey(id);
            if (Snow.SnowUsers.ToList().Count < 500)
            {
                foreach (var item in Snow.SnowUsers.ToList())
                {
                    SnowUserRepository.Remove(item);
                    SnowUserRepository.Context.Commit();
                }
                SnowRepository.Remove(Snow);
                SnowRepository.Context.Commit();
            }
            return RedirectToAction("SnowIndex");
        }

        #endregion 删除
        #region

        public ActionResult SnowUserList(Guid SnowID, int pageid = 1, DateTime? dt1 = null, DateTime? dt2 = null)
        {
            try
            {
                int guesscount = 0;
                int guesspeoplecount = 0;
                if (dt1 != null && dt2 != null && dt1 <= dt2)
                {
                    int UserId = WebSecurity.GetUserId(User.Identity.Name);
                    Snow Snow = SnowRepository.GetByKey(SnowID);
                    dt2 = DateTime.Parse(dt2.ToString()).AddDays(1);
                    var list = SnowUserRepository.FindAll(Specification<SnowUser>.Eval(o => o.SnowID == Snow.SnowID && o.AddDate > dt1 && o.AddDate < dt2)).OrderByDescending(o => o.Score).ToList();

                    guesscount = list.Count;
                    guesspeoplecount = list.GroupBy(o => o.SnowUserPhone).ToList().Count;
                    ViewData["guesscount"] = guesscount;
                    ViewData["guesspeoplecount"] = guesspeoplecount;

                    var Pagerlist = SnowUserRepository.GetListByPages(list, pageid, 10);
                    return View(Pagerlist);
                }
                else
                {
                    int UserId = WebSecurity.GetUserId(User.Identity.Name);
                    Snow Snow = SnowRepository.GetByKey(SnowID);
                    var EndDate = Snow.EndDate.AddDays(1);
                    var list = SnowUserRepository.FindAll(Specification<SnowUser>.Eval(o => o.SnowID == Snow.SnowID && o.AddDate > Snow.StartDate && o.AddDate < EndDate)).OrderByDescending(o => o.Score).ToList();
                    guesscount = list.Count;
                    guesspeoplecount = list.GroupBy(o => o.SnowUserPhone).ToList().Count;
                    ViewData["guesscount"] = guesscount;
                    ViewData["guesspeoplecount"] = guesspeoplecount;
                    var Pagerlist = SnowUserRepository.GetListByPages(list, pageid, 10);
                    return View(Pagerlist);
                }
            }
            catch (Exception)
            {
                return Redirect("/Snow/SnowIndex");
            }
        }

        #endregion

        #region 子项列表

        public ActionResult SnowItemIndex(int SnowID)
        {
            var Snow = SnowRepository.Find(Specification<Snow>.Eval(o => o.SnowID == SnowID));
            List<SnowItem> SnowItemList = Snow.SnowItems.ToList();
            return View(SnowItemList);
        }

        #endregion
        #region 子项编辑

        public ActionResult SnowItemEdit(int SnowID, Guid? ID = null)
        {
            if (ID == null)
            {
                SnowItem Snowitem = new SnowItem();
                Snowitem.SnowID = SnowID;
                Snowitem.Snow = SnowRepository.Find(Specification<Snow>.Eval(o => o.SnowID == SnowID));
                return View(Snowitem);
            }
            else
            {
                var Snowitem = SnowItemRepository.GetByKey(ID);
                return View(Snowitem);
            }
        }

        [HttpPost]
        public JsonResult SnowItemEdit(SnowItem form)
        {
            if (form.SnowItemID == 0)
            {
                form.AddDate = DateTime.Now;
                SnowItemRepository.Add(form);
                SnowItemRepository.Context.Commit();
                return Json(form);
            }
            else
            {
                var SnowItem = SnowItemRepository.Find(Specification<SnowItem>.Eval(o => o.SnowItemID == form.SnowItemID));
                SnowItem.SnowScore = form.SnowScore;
                SnowItem.SnowItemName = form.SnowItemName;
                SnowItem.SnowItemScale = form.SnowItemScale;
                SnowItemRepository.Update(SnowItem);
                SnowItemRepository.Context.Commit();
                return Json(SnowItem);
            }
        }

        #endregion
        #region 子项删除

        public ActionResult SnowItemDelete(Guid id)
        {
            var SnowItem = SnowItemRepository.GetByKey(id);
            int SnowID = SnowItem.Snow.SnowID;
            SnowItemRepository.Remove(SnowItem);
            SnowItemRepository.Context.Commit();
            return Redirect("/Snow/SnowItemIndex?SnowID=" + SnowID);
        }

        #endregion

        public ActionResult GetLink(Guid id)
        {
            try
            {
                Snow Snow = SnowRepository.GetByKey(id);
                return View(Snow);
            }
            catch (Exception)
            {
                return Redirect("/Snow/SnowIndex");
            }
        }

        public ActionResult GetAwardDatas(Guid id)
        {
            try
            {
                Snow Snow = SnowRepository.GetByKey(id);
                var EndDate = Snow.EndDate.AddDays(1);
                var awardlist = Snow.SnowUsers.Where(o => o.AddDate < EndDate && o.IsAward > 0).OrderBy(o => o.AddDate).ToList();
                return View(awardlist);
            }
            catch (Exception)
            {
                return Redirect("/Snow/SnowIndex");
            }
        }
    }
}