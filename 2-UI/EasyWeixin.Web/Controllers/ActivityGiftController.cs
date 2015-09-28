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
using System.Threading;
using System.Web.Mvc;

namespace EasyWeixin.Web.Controllers
{
    public class ActivityGiftController : Controller
    {
        //抢红包
        // GET: /ActivityGift/
        private readonly IResponseImageTextRepository ResponseImageTextRepository;

        private readonly IUserProfileRepository UserProfileRepository;
        private readonly IWheelUserRepository WheelUserRepository;
        private readonly IWheelRepository WheelRepository;
        private readonly IWheelItemRepository WheelItemRepository;
        private readonly IWheelLogRepository WheelLogRepository;
        private readonly IRecordWUserRepository RecordWUserRepository;

        public ActivityGiftController(IWheelLogRepository WheelLogRepository,
            IWheelItemRepository WheelItemRepository,
            IWheelRepository WheelRepository,
            IWheelUserRepository WheelUserRepository,
            IResponseImageTextRepository ResponseImageTextRepository,
            IUserProfileRepository UserProfileRepository,
            IRecordWUserRepository RecordWUserRepository)
        {
            this.ResponseImageTextRepository = ResponseImageTextRepository;
            this.UserProfileRepository = UserProfileRepository;
            this.WheelUserRepository = WheelUserRepository;
            this.WheelRepository = WheelRepository;
            this.WheelItemRepository = WheelItemRepository;
            this.WheelLogRepository = WheelLogRepository;
            this.RecordWUserRepository = RecordWUserRepository;
        }

        #region 抢红包

        /// <summary>
        /// 抢红包首页
        /// </summary>
        /// <param name="WheelID"></param>
        /// <param name="ImageTextID"></param>
        /// <param name="User_ID"></param>
        /// <param name="UserWexinID"></param>
        /// <returns></returns>
        public ActionResult GiftIndex(Guid WheelID, Guid ImageTextID, Guid User_ID, string UserWexinID = "")
        {
            var user = UserProfileRepository.GetByKey(User_ID);
            Session["UserId"] = user.UserId;
            Session["openId"] = UserWexinID;
            var Wheel = WheelRepository.GetByKey(WheelID);
            return View(Wheel);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        public JsonResult GetGiftitem(Guid WheelID, string UserWexinID = "")
        {
            var ip = Request.UserHostAddress;
            var dt = DateTime.Now.Date;
            var Wheel = WheelRepository.GetByKey(WheelID);
            GetGiftCode(WheelID);
            //添加日志
            WheelLog WheelCode;
            string sdate = DateTime.Now.ToString("yyyy-MM-dd");
            DateTime ddate = Convert.ToDateTime(sdate);
            var logs = WheelLogRepository.FindAll(Specification<WheelLog>.Eval(o => o.WheelID == Wheel.WheelID && o.IsAward > 0 && o.AddDate >= ddate && o.AddDate < ddate.AddDays(1))).ToList();

            var IsChou = WheelLogRepository.FindAll(Specification<WheelLog>.Eval(o => o.WheelID == Wheel.WheelID && o.WheelUserWexinID == UserWexinID)).ToList();
            int num = 0;
            int mm = 0;
            foreach (var item in IsChou)
            {
                if (item.IsShare > 0)
                {
                    mm += item.IsShare;
                }
            }

            foreach (var item in Wheel.WheelItems)
            {
                num += item.WheelItemScale;
            }

            if (logs.Count >= num)
            {
                WheelCode = new WheelLog();
                WheelCode.WheelCode = "0";
                WheelCode.IsAward = 0;
                WheelCode.AddDate = DateTime.Now;
                WheelCode.WheelID = Wheel.WheelID;
            }
            else
            {
                WheelCode = GetGiftCode(WheelID);
            }

            WheelCode.IP = ip;
            Session["WheelCode"] = WheelCode;

            int IsAward = WheelCode.IsAward;
            WheelItemViewModel Wheelitem = new WheelItemViewModel();
            if (IsAward > 0)
            {
                var Si = Wheel.WheelItems.ToList()[IsAward - 1];
                Wheelitem = Mapper.Map<WheelItem, WheelItemViewModel>(Si);
                Wheelitem.WheelCode = WheelCode.WheelCode;
            }
            else
            {
                Wheelitem.WheelCode = WheelCode.WheelCode;
                Wheelitem.WheelItemID = IsAward;
                Wheelitem.MaxAngle = "";
                Wheelitem.MinAngle = "";
                Wheelitem.WheelItemName = "没有抽中";
            }

            var log = WheelLogRepository.FindAll(Specification<WheelLog>.Eval(o => o.WheelUserWexinID == UserWexinID && o.AddDate > dt && o.WheelID == Wheel.WheelID)).ToList();
            if (log.Count < Wheel.EveryDayTimes && Wheel.StartDate < DateTime.Now && Wheel.EndDate.AddDays(1) > DateTime.Now)
            {
                ///日志记录
                WheelLogRepository.Add(WheelCode);
                WheelLogRepository.Context.Commit();
                return Json(Wheelitem);
            }
            else if (logs.Count > 20)
            {
                return Json(new { message = "今日红包已经抢完！" }, JsonRequestBehavior.AllowGet);
            }
            else if (DateTime.Now < Wheel.StartDate)
            {
                return Json(new { message = "活动暂未开始，敬请期待！" }, JsonRequestBehavior.AllowGet);
            }
            else if (DateTime.Now > Wheel.EndDate.AddDays(1))
            {
                return Json(new { message = "活动已经结束了!" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { message = "每日最多抢两次" + Wheel.EveryDayTimes.ToString() + "次" }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetGift(Guid WheelID, string UserWexinID = "")
        {
            var ip = Request.UserHostAddress;
            var dt = DateTime.Now.Date;
            var Wheel = WheelRepository.GetByKey(WheelID);
            GetGiftCode(WheelID);//调用此方法是为了去检查是否有抢到红包没有提交用户信息，然而删除，空出名额
            //添加日志
            WheelLog WheelCode;
            string sdate = DateTime.Now.ToString("yyyy-MM-dd");
            DateTime ddate = Convert.ToDateTime(sdate);

            //获取当天中奖人数
            var logs = WheelLogRepository.FindAll(Specification<WheelLog>.Eval(o => o.WheelID == Wheel.WheelID && o.IsAward > 0 && o.AddDate >= ddate && o.AddDate < ddate.AddDays(1))).ToList();

            //循环相加获取出后台设置好的奖品数量
            int num = 0;
            foreach (var item in Wheel.WheelItems)
            {
                num += item.WheelItemScale;
            }

            var IsChou = WheelLogRepository.FindAll(Specification<WheelLog>.Eval(o => o.WheelID == Wheel.WheelID && o.WheelUserWexinID == UserWexinID && o.AddDate >= ddate && o.AddDate < ddate.AddDays(1))).ToList();
            var IsShare = WheelLogRepository.FindAll(Specification<WheelLog>.Eval(o => o.WheelID == Wheel.WheelID && o.WheelUserWexinID == UserWexinID && o.IsShare == 1 && o.AddDate >= ddate && o.AddDate < ddate.AddDays(1))).ToList();
            var IsAwards = WheelUserRepository.FindAll(Specification<WheelUser>.Eval(o => o.WheelID == Wheel.WheelID && o.WheelUserWexinID == UserWexinID)).ToList();
            var weiUser = RecordWUserRepository.FindAll(Specification<RecordWUser>.Eval(o => o.FromUserName == UserWexinID)).ToList();
            //判断是否关注从来没有抽过，或者是抽过一次，但是分享了
            if ((weiUser.Count > 0 && IsChou.Count < 0) || (IsChou.Count == 1 && IsShare.Count > 0))
            {
                //判断中过多少次奖
                if (IsAwards.Count < 2)
                {
                    //判断奖品数量是否超过指定
                    if (logs.Count < num)
                    {
                        //防止并发
                        Random rm = new Random();
                        Thread.Sleep(rm.Next(500, 1000));
                        WheelCode = GetGiftCode(WheelID);
                    }
                    else
                    {
                        WheelCode = new WheelLog();
                        WheelCode.WheelCode = "0";
                        WheelCode.IsAward = 0;
                        WheelCode.AddDate = DateTime.Now;
                        WheelCode.WheelID = Wheel.WheelID;
                    }
                }
                else
                {
                    WheelCode = new WheelLog();
                    WheelCode.WheelCode = "0";
                    WheelCode.IsAward = 0;
                    WheelCode.AddDate = DateTime.Now;
                    WheelCode.WheelID = Wheel.WheelID;
                }
            }
            else
            {
                WheelCode = new WheelLog();
                WheelCode.WheelCode = "0";
                WheelCode.IsAward = 0;
                WheelCode.AddDate = DateTime.Now;
                WheelCode.WheelID = Wheel.WheelID;
            }

            WheelCode.IP = ip;
            Session["WheelCode"] = WheelCode;

            //判断随机产生的日志实体是否中奖
            int IsAward = WheelCode.IsAward;
            WheelItemViewModel Wheelitem = new WheelItemViewModel();
            if (IsAward > 0)
            {
                var Si = Wheel.WheelItems.ToList()[IsAward - 1];
                Wheelitem = Mapper.Map<WheelItem, WheelItemViewModel>(Si);
                Wheelitem.WheelCode = WheelCode.WheelCode;
            }
            else
            {
                Wheelitem.WheelCode = WheelCode.WheelCode;
                Wheelitem.WheelItemID = IsAward;
                Wheelitem.MaxAngle = "";
                Wheelitem.MinAngle = "";
                Wheelitem.WheelItemName = "没有抽中";
            }

            if (IsChou.Count < Wheel.EveryDayTimes && Wheel.StartDate < DateTime.Now && Wheel.EndDate.AddDays(1) > DateTime.Now)
            {
                ///日志记录
                WheelLogRepository.Add(WheelCode);
                WheelLogRepository.Context.Commit();
                return Json(Wheelitem);
            }
            else if (logs.Count > 20)
            {
                return Json(new { message = "今日红包已经抢完！" }, JsonRequestBehavior.AllowGet);
            }
            else if (DateTime.Now < Wheel.StartDate)
            {
                return Json(new { message = "活动暂未开始，敬请期待！" }, JsonRequestBehavior.AllowGet);
            }
            else if (DateTime.Now > Wheel.EndDate.AddDays(1))
            {
                return Json(new { message = "活动已经结束了!" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { message = "每日最多抢" + Wheel.EveryDayTimes.ToString() + "次" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 获取一个角度的最大值和最小值字符串
        /// </summary>
        /// <param name="i">角度索引</param>
        /// <param name="num">角度平均分块</param>
        /// <param name="f">角度波动</param>
        /// <returns></returns>
        public string GetAngle(int i, int num, int f)
        {
            //获取平均角度区域
            var average = 360 / num;
            var averagehalf = 360 / (num * 2);
            var half = i * average;
            var str = (half - f).ToString() + "|" + (half + f).ToString();
            return str;
        }

        /// <summary>
        /// 用户提交数据
        /// </summary>
        /// <param name="WheelUser">大转盘玩家实体，实体中只传递过来玩家的手机号码</param>
        /// <returns></returns>
        public JsonResult GiftAdd(WheelUser WheelUser)
        {
            if (Session["WheelCode"] != null)
            {
                var WheelCode = (WheelLog)Session["WheelCode"];
                var Wheel = WheelRepository.Find(Specification<Wheel>.Eval(o => o.WheelID == WheelCode.WheelID));

                //循环判断是否已存在相同的手机号码,同样的微信号
                var users = WheelUserRepository.FindAll(Specification<WheelUser>.Eval(o => o.WheelID == Wheel.WheelID)).ToList();
                foreach (var item in users)
                {
                    if (item.WheelUserPhone.Equals(WheelUser.WheelUserPhone) && item.WheelUserWexinID.Equals(WheelUser.WheelUserWexinID) && users.Count > 2)
                    {
                        return Json(new { message = "每个ID最多可赢得两张欢乐谷门票!" });
                    }
                }

                var ip = Request.UserHostAddress;
                var dt = DateTime.Now.Date;
                WheelUser.WheelItemID = Wheel.WheelItems.ToList()[WheelCode.IsAward - 1].WheelItemID;
                WheelUser.WheelID = WheelCode.WheelID;
                WheelUser.IP = Request.UserHostAddress;
                WheelUser.WheelCode = WheelCode.WheelCode;
                WheelUser.UserId = int.Parse(Session["UserId"].ToString());
                WheelUser.AddDate = DateTime.Now;
                WheelUser.WheelLogID = WheelCode.WheelLogID;
                WheelUser.WheelUserWexinID = Session["openId"].ToString();
                WheelUserRepository.Add(WheelUser);
                WheelUserRepository.Context.Commit();
                Session["WheelCode"] = null;
                Session["openId"] = null;
                return Json(new { message = "提交成功" });
            }
            else
            {
                return Json(new { message = "已超时！" });
            }
        }

        /// <summary>
        /// 获取一个随机日志实体
        /// </summary>
        /// <param name="WheelID"></param>
        /// <returns></returns>
        public WheelLog GetGiftCode(Guid WheelID)
        {
            //DateTime dt = DateTime.Now.AddHours(-1);
            DateTime dt = DateTime.Now.AddMinutes(-10);
            var Wheel = WheelRepository.GetByKey(WheelID);
            var users = WheelUserRepository.FindAll(Specification<WheelUser>.Eval(o => o.WheelID == Wheel.WheelID)).ToList();
            //清理日志  如果用户中奖 两个小时里没有提交信息 就将日志其删除
            var logs = WheelLogRepository.FindAll(Specification<WheelLog>.Eval(o => o.WheelID == Wheel.WheelID && o.IsAward > 0 && o.AddDate < dt)).ToList();
            foreach (var item in logs)
            {
                if (!users.Exists(o => o.WheelLogID == item.WheelLogID))
                {
                    WheelLogRepository.Remove(item);
                    WheelLogRepository.Context.Commit();
                }
            }
            var userWheelCodes = users.Select(o => o.WheelCode).ToList();
            //获取奖品的概率容量
            int container = Wheel.WheelScale;
            var WheelItems = Wheel.WheelItems.ToList();

            for (int i = 0; i < WheelItems.Count; i++)
            {
                container = container - WheelItems[i].WheelItemScale;
            }
            //设定一个string动态数组 添加中奖代号
            List<WheelLog> WheelCodes = new List<WheelLog>();
            for (int i = 0; i < WheelItems.Count; i++)
            {
                //剩余奖品数量
                var k = i + 1;
                var itemlogs = WheelLogRepository.FindAll(Specification<WheelLog>.Eval(o => o.WheelID == Wheel.WheelID && o.IsAward == k)).ToList();
                var sum = WheelItems[i].WheelItemScale - itemlogs.Count;
                for (int j = 0; j < sum; j++)
                {
                    WheelLog log = new WheelLog();
                    log.WheelCode = Function.GuidTo16String(Guid.NewGuid());
                    log.IsAward = k;
                    log.AddDate = DateTime.Now;
                    log.WheelID = Wheel.WheelID;
                    WheelCodes.Add(log);
                }
            }
            if (container > 0)
            {
                for (int i = 0; i < container; i++)
                {
                    WheelLog log = new WheelLog();
                    log.WheelCode = "0";
                    log.IsAward = 0;
                    log.AddDate = DateTime.Now;
                    log.WheelID = Wheel.WheelID;
                    WheelCodes.Add(log);
                }
            }
            //获取一个随机数 随机抽奖
            Random randow = new Random();
            int ran = randow.Next(0, WheelCodes.Count - 1);
            return WheelCodes[ran];
        }

        #endregion 抢红包
    }
}