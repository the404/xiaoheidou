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
    [Authorize(Roles = "Coupon")]
    public class CouponController : Controller
    {
        //优惠劵
        // GET: /Coupon/

        private readonly IResponseImageTextRepository ResponseImageTextRepository;
        private readonly IUserProfileRepository UserProfileRepository;
        private readonly ICouponUserRepository CouponUserRepository;
        private readonly ICouponItemRepository CouponItemRepository;
        private readonly ICouponRepository CouponRepository;

        public CouponController(ICouponRepository CouponRepository,
            ICouponUserRepository CouponUserRepository,
            IResponseImageTextRepository ResponseImageTextRepository,
            IUserProfileRepository UserProfileRepository,
            ICouponItemRepository CouponItemRepository)
        {
            this.ResponseImageTextRepository = ResponseImageTextRepository;
            this.UserProfileRepository = UserProfileRepository;
            this.CouponUserRepository = CouponUserRepository;
            this.CouponItemRepository = CouponItemRepository;
            this.CouponRepository = CouponRepository;
        }

        #region 主页列表

        public ActionResult CouponIndex(int pageid = 1)
        {
            var UserId = WebSecurity.GetUserId(User.Identity.Name);
            var Coupon = CouponRepository.FindAll(Specification<Coupon>.Eval(o => o.UserId == UserId)).ToList();
            var Pagerlist = CouponRepository.GetListByPages(Coupon, pageid, 10);
            return View(Pagerlist);
        }

        #endregion 主页列表

        #region 打开创建页

        public ActionResult CouponCreate()
        {
            return View();
        }

        /// <summary>
        /// 添加优惠劵活动
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult CouponCreate(CouponViewModel form)
        {
            var UserId = WebSecurity.GetUserId(User.Identity.Name);
            var user = UserProfileRepository.Find(Specification<EasyWeixin.Model.UserProfile>.Eval(o => o.UserId == UserId));
            form.ResponseImageTextViewModel.ImageTextName = form.CouponTitle;
            form.ResponseImageTextViewModel.ImageTextType = 105;
            form.ResponseImageTextViewModel.UserId = WebSecurity.GetUserId(User.Identity.Name);
            form.ResponseImageTextViewModel.AddTime = DateTime.Now;
            form.UserId = WebSecurity.GetUserId(User.Identity.Name);
            form.AddDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                Coupon Coupon = Mapper.Map<CouponViewModel, Coupon>(form);
                Coupon.CouponStyle = "Coupon.css";
                Coupon.ResponseImageText = Mapper.Map<ResponseImageTextViewModel, ResponseImageText>(form.ResponseImageTextViewModel);
                CouponRepository.Add(Coupon);
                CouponRepository.Context.Commit();
                Coupon.ResponseImageText.Url = "http://" + Request.Url.Host + "/ActivityCoupon/CouponIndex?CouponID=" + Coupon.ID + "&ImageTextID=" + Coupon.ResponseImageText.ID;
                Coupon.GetURL = "http://" + Request.Url.Host + "/ActivityCoupon/CouponIndex?CouponID=" + Coupon.ID + "&ImageTextID=" + Coupon.ResponseImageText.ID + "&User_ID=" + user.ID;
                CouponRepository.Update(Coupon);
                CouponRepository.Context.Commit();
            }
            return Redirect("/Coupon/CouponIndex");
        }

        #endregion 打开创建页

        #region 打开编辑页

        public ActionResult CouponEdit(Guid id)
        {
            Coupon Coupon = CouponRepository.GetByKey(id);
            CouponViewModel form = Mapper.Map<Coupon, CouponViewModel>(Coupon);
            form.ResponseImageTextViewModel =
                Mapper.Map<ResponseImageText, ResponseImageTextViewModel>(Coupon.ResponseImageText);
            return View(form);
        }

        /// <summary>
        /// 提交编辑
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult CouponEdit(CouponViewModel form)
        {
            if (ModelState.IsValid)
            {
                Coupon Coupon = CouponRepository.GetByKey(form.ID);
                Coupon.CouponTitle = form.CouponTitle;
                Coupon.StartDate = form.StartDate;
                Coupon.EndDate = form.EndDate;
                Coupon.CouponDesc = form.CouponDesc;
                Coupon.CouponCount = form.CouponCount;
                Coupon.CouponScale = form.CouponScale;
                Coupon.ResponseImageText.ImageTextName = form.CouponTitle;
                Coupon.ResponseImageText.Content = form.ResponseImageTextViewModel.Content;
                CouponRepository.Update(Coupon);
                CouponRepository.Context.Commit();
            }
            return Redirect("/Coupon/CouponIndex");
        }

        #endregion 打开编辑页

        #region 删除

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult CouponDelete(Guid id)
        {
            var Coupon = CouponRepository.GetByKey(id);
            //当提交数据达到了
            if (Coupon.CouponUsers == null || Coupon.CouponUsers.ToList().Count < 500)
            {
                //先清除关联表数据 再清除主表数据 否则因为关联 会报错
                foreach (var item in Coupon.CouponUsers.ToList())
                {
                    CouponUserRepository.Remove(item);
                    CouponUserRepository.Context.Commit();
                }
                CouponRepository.Remove(Coupon);
                CouponRepository.Context.Commit();
            }
            return RedirectToAction("CouponIndex");
        }

        #endregion 删除

        #region 用户列表

        /// <summary>
        /// 优惠劵 用户列表
        /// </summary>
        /// <param name="CouponID"></param>
        /// <param name="pageid"></param>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <returns></returns>
        public ActionResult CouponUserList(Guid CouponID, int pageid = 1, DateTime? dt1 = null, DateTime? dt2 = null)
        {
            //开始时间和结束时间不为空的时候，进入搜索模式
            if (dt1 != null && dt2 != null && dt1 <= dt2)
            {
                int UserId = WebSecurity.GetUserId(User.Identity.Name);
                Coupon Coupon = CouponRepository.GetByKey(CouponID);
                dt2 = DateTime.Parse(dt2.ToString()).AddDays(1);
                var list = CouponUserRepository.FindAll(Specification<CouponUser>.Eval(o => o.UserId == UserId && o.CouponID == Coupon.CouponID && o.AddDate > dt1 && o.AddDate < dt2)).ToList();
                var Pagerlist = CouponUserRepository.GetListByPages(list, pageid, 10);
                return View(Pagerlist);
            }
            else
            {
                int UserId = WebSecurity.GetUserId(User.Identity.Name);
                Coupon Coupon = CouponRepository.GetByKey(CouponID);
                var EndDate = Coupon.EndDate.AddDays(1);
                var list = CouponUserRepository.FindAll(Specification<CouponUser>.Eval(o => o.UserId == UserId && o.CouponID == Coupon.CouponID && o.AddDate > Coupon.StartDate && o.AddDate < EndDate)).ToList();
                var Pagerlist = CouponUserRepository.GetListByPages(list, pageid, 10);
                return View(Pagerlist);
            }
        }

        #endregion 用户列表

        #region 子项列表

        public ActionResult CouponItemIndex(int CouponID)
        {
            var Coupon = CouponRepository.Find(Specification<Coupon>.Eval(o => o.CouponID == CouponID));
            List<CouponItem> CouponItemList = Coupon.CouponItems.ToList();
            if (CouponItemList.Count == 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    CouponItem ci = new CouponItem();
                    ci.CouponItemScale = i + 1;
                    ci.CouponItemName = (i + 1).ToString() + "等奖";
                    ci.CouponItemAward = "";
                    ci.CouponID = Coupon.CouponID;
                    ci.AddDate = DateTime.Now;
                    CouponItemRepository.Add(ci);
                    CouponItemRepository.Context.Commit();
                }
            }
            return View(CouponItemList);
        }

        #endregion 子项列表

        #region 子项编辑

        public ActionResult CouponItemEdit(int CouponID, Guid? ID = null)
        {
            if (ID == null)
            {
                CouponItem Couponitem = new CouponItem();
                Couponitem.CouponID = CouponID;
                return View(Couponitem);
            }
            else
            {
                var Couponitem = CouponItemRepository.GetByKey(ID);
                return View(Couponitem);
            }
        }

        [HttpPost]
        public JsonResult CouponItemEdit(CouponItem form)
        {
            if (form.CouponItemID == 0)
            {
                form.AddDate = DateTime.Now;
                CouponItemRepository.Add(form);
                CouponItemRepository.Context.Commit();
                return Json(form);
            }
            else
            {
                var CouponItem = CouponItemRepository.Find(Specification<CouponItem>.Eval(o => o.CouponItemID == form.CouponItemID));
                CouponItem.CouponItemAward = form.CouponItemAward;
                CouponItem.CouponItemName = form.CouponItemName;

                CouponItem.CouponItemScale = form.CouponItemScale;
                CouponItemRepository.Update(CouponItem);
                CouponItemRepository.Context.Commit();
                return Json(CouponItem);
            }
        }

        #endregion 子项编辑

        #region 子项删除

        public ActionResult CouponItemDelete(Guid id)
        {
            var CouponItem = CouponItemRepository.GetByKey(id);
            int CouponID = CouponItem.Coupon.CouponID;
            CouponItemRepository.Remove(CouponItem);
            CouponItemRepository.Context.Commit();
            return Redirect("/Coupon/CouponItemIndex?CouponID=" + CouponID);
        }

        #endregion 子项删除
    }
}