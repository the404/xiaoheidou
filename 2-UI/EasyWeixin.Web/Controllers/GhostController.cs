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
    [Authorize(Roles = "Ghost")]
    public class GhostController : Controller
    {
        //恶魔快跑
        // GET: /Ghost/

        private readonly IResponseImageTextRepository ResponseImageTextRepository;
        private readonly IUserProfileRepository UserProfileRepository;
        private readonly ISnowUserRepository SnowUserRepository;
        private readonly ISnowRepository SnowRepository;
        private readonly ISnowItemRepository SnowItemRepository;

        public GhostController(ISnowItemRepository SnowItemRepository, ISnowRepository SnowRepository, ISnowUserRepository SnowUserRepository, IResponseImageTextRepository ResponseImageTextRepository, IUserProfileRepository UserProfileRepository)
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

        public ActionResult GhostIndex(int pageid = 1)
        {
            var UserId = WebSecurity.GetUserId(User.Identity.Name);
            var Snow = SnowRepository.FindAll(Specification<Snow>.Eval(o => o.UserId == UserId)).Where(o => o.Mark == 3).ToList();
            var Pagerlist = SnowRepository.GetListByPages(Snow, pageid, 10);
            return View(Pagerlist);
        }

        #endregion 主页列表

        #region 打开创建页

        public ActionResult GhostCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult GhostCreate(SnowViewModel form)
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
                Snow.Mark = 3;
                Snow.ResponseImageText = Mapper.Map<ResponseImageTextViewModel, ResponseImageText>(form.ResponseImageTextViewModel);
                SnowRepository.Add(Snow);
                SnowRepository.Context.Commit();
                Snow.ResponseImageText.Url = "http://" + Request.Url.Host + "/ActivityGhost/GhostIndex?SnowID=" + Snow.ID + "&ImageTextID=" + Snow.ResponseImageText.ID;
                Snow.GetURL = "http://" + Request.Url.Host + "/ActivityGhost/GhostIndex?SnowID=" + Snow.ID + "&ImageTextID=" + Snow.ResponseImageText.ID + "&User_ID=" + user.ID;
                SnowRepository.Update(Snow);
                SnowRepository.Context.Commit();
            }
            return Redirect("/Ghost/GhostIndex");
        }

        #endregion 打开创建页

        #region 打开编辑页

        public ActionResult GhostEdit(Guid id)
        {
            Snow Snow = SnowRepository.GetByKey(id);
            SnowViewModel form = Mapper.Map<Snow, SnowViewModel>(Snow);
            form.ResponseImageTextViewModel = Mapper.Map<ResponseImageText, ResponseImageTextViewModel>(Snow.ResponseImageText);
            return View(form);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult GhostEdit(SnowViewModel form)
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
            return Redirect("/Ghost/GhostIndex");
        }

        #endregion 打开编辑页

        #region 删除

        public ActionResult GhostDelete(Guid id)
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
            return RedirectToAction("GhostIndex");
        }

        #endregion 删除

        #region 用户列表

        public ActionResult GhostUserList(Guid SnowID, int pageid = 1, DateTime? dt1 = null, DateTime? dt2 = null)
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

                    var list1 = SnowUserRepository.FindAll(Specification<SnowUser>.Eval(o => o.SnowID == Snow.SnowID && o.AddDate > dt1 && o.AddDate < dt2)).OrderByDescending(o => o.Score).ToList();
                    var list = SnowUserRepository.FindAll(Specification<SnowUser>.Eval(o => o.SnowID == Snow.SnowID && o.AddDate > dt1 && o.AddDate < dt2)).OrderByDescending(o => o.Score).GroupBy(ic => ic.SnowUserPhone).Select(g => g.First()).ToList();
                    guesscount = list1.Count();
                    //guesspeoplecount = list.GroupBy(o => o.SnowUserPhone).ToList().Count;
                    guesspeoplecount = list.Count();
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
                    var list1 = SnowUserRepository.FindAll(Specification<SnowUser>.Eval(o => o.SnowID == Snow.SnowID && o.AddDate > Snow.StartDate && o.AddDate < EndDate)).OrderByDescending(o => o.Score).ToList();
                    var list = SnowUserRepository.FindAll(Specification<SnowUser>.Eval(o => o.SnowID == Snow.SnowID && o.AddDate > Snow.StartDate && o.AddDate < EndDate)).OrderByDescending(o => o.Score).GroupBy(ic => ic.SnowUserPhone).Select(g => g.First()).ToList();
                    //guesscount = list.Count;
                    // guesspeoplecount = list.GroupBy(o => o.SnowUserPhone).ToList().Count;
                    guesscount = list1.Count();
                    guesspeoplecount = list.Count();
                    ViewData["guesscount"] = guesscount;
                    ViewData["guesspeoplecount"] = guesspeoplecount;
                    var Pagerlist = SnowUserRepository.GetListByPages(list, pageid, 10);
                    return View(Pagerlist);
                }
            }
            catch (Exception)
            {
                return Redirect("/Ghost/GhostIndex");
            }
        }

        #endregion 用户列表

        #region 子项列表

        public ActionResult GhostItemIndex(int SnowID)
        {
            var Snow = SnowRepository.Find(Specification<Snow>.Eval(o => o.SnowID == SnowID));
            List<SnowItem> SnowItemList = Snow.SnowItems.ToList();
            return View(SnowItemList);
        }

        #endregion 子项列表

        #region 子项编辑

        public ActionResult GhostItemEdit(int SnowID, Guid? ID = null)
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
        public JsonResult GhostItemEdit(SnowItem form)
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

        #endregion 子项编辑

        #region 子项删除

        public ActionResult GhostItemDelete(Guid id)
        {
            var SnowItem = SnowItemRepository.GetByKey(id);
            int SnowID = SnowItem.Snow.SnowID;
            SnowItemRepository.Remove(SnowItem);
            SnowItemRepository.Context.Commit();
            return Redirect("/Ghost/GhostItemIndex?SnowID=" + SnowID);
        }

        #endregion 子项删除

        public ActionResult GetLink(Guid id)
        {
            try
            {
                Snow Snow = SnowRepository.GetByKey(id);
                return View(Snow);
            }
            catch (Exception)
            {
                return Redirect("/Ghost/GhostIndex");
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
                return Redirect("/Ghost/GhostIndex");
            }
        }
    }
}