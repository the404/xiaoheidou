using Apworks.Specifications;
using AutoMapper;
using EasyWeixin.Core.Common;
using EasyWeixin.Data.Repositories;
using EasyWeixin.Model;
using EasyWeixin.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace EasyWeixin.Web.Controllers
{
    public class ActivityCouponController : Controller
    {
        //
        // GET: /ActivityCoupon/

        private readonly IResponseImageTextRepository ResponseImageTextRepository;
        private readonly IUserProfileRepository UserProfileRepository;
        private readonly ICouponUserRepository CouponUserRepository;
        private readonly ICouponRepository CouponRepository;
        private readonly ICouponItemRepository CouponItemRepository;
        private readonly ICouponLogRepository CouponLogRepository;

        public ActivityCouponController(ICouponLogRepository CouponLogRepository,
            ICouponItemRepository CouponItemRepository,
            ICouponRepository CouponRepository,
            ICouponUserRepository CouponUserRepository,
            IResponseImageTextRepository ResponseImageTextRepository,
            IUserProfileRepository UserProfileRepository)
        {
            this.ResponseImageTextRepository = ResponseImageTextRepository;
            this.UserProfileRepository = UserProfileRepository;
            this.CouponUserRepository = CouponUserRepository;
            this.CouponRepository = CouponRepository;
            this.CouponItemRepository = CouponItemRepository;
            this.CouponLogRepository = CouponLogRepository;
        }

        #region 优惠劵

        /// <summary>
        /// 优惠劵主页
        /// </summary>
        /// <param name="ScratchID">活动的guid</param>
        /// <param name="ImageTextID">活动图文的guid</param>
        /// <param name="User_ID">企业用户的guid</param>
        /// <param name="UserWexinID">用户微信账号</param>
        /// <returns></returns>
        public ActionResult CouponIndex(Guid CouponID, Guid ImageTextID, Guid User_ID, string UserWexinID = "")
        {
            var user = UserProfileRepository.GetByKey(User_ID);
            Session["UserId"] = user.UserId;
            var Coupon = CouponRepository.GetByKey(CouponID);
            return View(Coupon);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        public JsonResult GetCouponitem(Guid CouponID, string UserWexinID = "")
        {
            var ip = Request.UserHostAddress;
            var dt = DateTime.Now.Date;
            var Coupon = CouponRepository.GetByKey(CouponID);
            //添加日志
            var CouponCode = GetCouponCode(CouponID);
            CouponCode.IP = ip;
            Session["CouponCode"] = CouponCode;
            int IsAward = CouponCode.IsAward;
            CouponItemViewModel Couponitem = new CouponItemViewModel();
            if (IsAward > 0)
            {
                var Si = Coupon.CouponItems.ToList()[IsAward - 1];
                Couponitem = Mapper.Map<CouponItem, CouponItemViewModel>(Si);
                Couponitem.CouponCode = CouponCode.CouponCode;
            }
            else
            {
                Couponitem.CouponCode = CouponCode.CouponCode;
                Couponitem.CouponItemID = IsAward;
                Couponitem.CouponItemName = "谢谢惠顾";
            }
            var log = CouponLogRepository.FindAll(Specification<CouponLog>.Eval(o => o.IP == ip && o.AddDate > dt && o.CouponID == Coupon.CouponID)).ToList();
            if (log.Count < Coupon.EveryDayTimes && Coupon.StartDate < DateTime.Now && Coupon.EndDate.AddDays(1) > DateTime.Now)
            {
                ///日志记录
                CouponLogRepository.Add(CouponCode);
                CouponLogRepository.Context.Commit();
                return Json(Couponitem);
            }
            else
            {
                return Json(new { message = "每日只能提交" + Coupon.EveryDayTimes.ToString() + "次" });
            }
        }

        /// <summary>
        /// 用户提交优惠劵的信息
        /// </summary>
        /// <param name="CouponUser">用户信息 现在提交过来的用户信息只有 一个电话号码</param>
        /// <returns></returns>
        public JsonResult CouponAdd(CouponUser CouponUser)
        {
            if (Session["CouponCode"] != null)
            {
                var CouponCode = (CouponLog)Session["CouponCode"];
                var Coupon = CouponRepository.Find(Specification<Coupon>.Eval(o => o.CouponID == CouponCode.CouponID));
                var ip = Request.UserHostAddress;
                var dt = DateTime.Now.Date;
                CouponUser.CouponItemID = Coupon.CouponItems.ToList()[CouponCode.IsAward - 1].CouponItemID;
                CouponUser.CouponID = CouponCode.CouponID;
                CouponUser.IP = Request.UserHostAddress;
                CouponUser.CouponCode = CouponCode.CouponCode;
                CouponUser.UserId = int.Parse(Session["UserId"].ToString());
                CouponUser.AddDate = DateTime.Now;
                CouponUserRepository.Add(CouponUser);
                CouponUserRepository.Context.Commit();
                return Json(new { message = "提交成功" });
            }
            else
            {
                return Json(new { message = "已超时！" });
            }
        }

        /// <summary>
        /// 获取一个随机的日志实体
        /// </summary>
        /// <param name="CouponID"></param>
        /// <returns></returns>
        public CouponLog GetCouponCode(Guid CouponID)
        {
            DateTime dt = DateTime.Now.AddHours(-1);
            var Coupon = CouponRepository.GetByKey(CouponID);
            var users = CouponUserRepository.FindAll(Specification<CouponUser>.Eval(o => o.CouponID == Coupon.CouponID)).ToList();
            //清理日志  如果用户中奖 一个小时里没有提交信息 就将日志其删除
            var logs = CouponLogRepository.FindAll(Specification<CouponLog>.Eval(o => o.CouponID == Coupon.CouponID && o.IsAward > 0 && o.AddDate < dt)).ToList();
            foreach (var item in logs)
            {
                if (!users.Exists(o => o.CouponLogID == item.CouponLogID))
                {
                    CouponLogRepository.Remove(item);
                    CouponLogRepository.Context.Commit();
                }
            }
            var userCouponCodes = users.Select(o => o.CouponCode).ToList();
            //获取奖品的概率容量
            int container = Coupon.CouponScale;
            var CouponItems = Coupon.CouponItems.ToList();

            for (int i = 0; i < CouponItems.Count; i++)
            {
                container = container - CouponItems[i].CouponItemScale;
            }
            //设定一个string动态数组 添加中奖代号
            List<CouponLog> CouponCodes = new List<CouponLog>();
            for (int i = 0; i < CouponItems.Count; i++)
            {
                //剩余奖品数量
                var k = i + 1;
                var itemlogs = CouponLogRepository.FindAll(Specification<CouponLog>.Eval(o => o.CouponID == Coupon.CouponID && o.IsAward == k)).ToList();
                var sum = CouponItems[i].CouponItemScale - itemlogs.Count;
                for (int j = 0; j < sum; j++)
                {
                    CouponLog log = new CouponLog();
                    log.CouponCode = Function.GuidTo16String(Guid.NewGuid());
                    log.IsAward = k;
                    log.AddDate = DateTime.Now;
                    log.CouponID = Coupon.CouponID;
                    CouponCodes.Add(log);
                }
            }
            if (container > 0)
            {
                for (int i = 0; i < container; i++)
                {
                    CouponLog log = new CouponLog();
                    log.CouponCode = "0";
                    log.IsAward = 0;
                    log.AddDate = DateTime.Now;
                    log.CouponID = Coupon.CouponID;
                    CouponCodes.Add(log);
                }
            }
            //获取一个随机数 随机抽奖
            Random randow = new Random();
            int ran = randow.Next(0, CouponCodes.Count - 1);
            return CouponCodes[ran];
        }

        #endregion 优惠劵
    }
}