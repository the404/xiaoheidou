using Apworks.Specifications;
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
    public class ActivityEggController : Controller
    {
        //云南华侨城在线支付成功后，砸金蛋
        // GET: /ActivityEgg/
        private readonly IResponseImageTextRepository ResponseImageTextRepository;

        private readonly IUserProfileRepository UserProfileRepository;
        private readonly IWheelUserRepository WheelUserRepository;
        private readonly IWheelRepository WheelRepository;
        private readonly IWheelItemRepository WheelItemRepository;
        private readonly IWheelLogRepository WheelLogRepository;
        private readonly IPayCustomerRepository PayCustomerRepository;

        public ActivityEggController(IWheelLogRepository WheelLogRepository,
            IWheelItemRepository WheelItemRepository,
            IWheelRepository WheelRepository,
            IWheelUserRepository WheelUserRepository,
            IResponseImageTextRepository ResponseImageTextRepository,
            IUserProfileRepository UserProfileRepository,
            IPayCustomerRepository PayCustomerRepository)
        {
            this.ResponseImageTextRepository = ResponseImageTextRepository;
            this.UserProfileRepository = UserProfileRepository;
            this.WheelUserRepository = WheelUserRepository;
            this.WheelRepository = WheelRepository;
            this.WheelItemRepository = WheelItemRepository;
            this.WheelLogRepository = WheelLogRepository;
            this.PayCustomerRepository = PayCustomerRepository;
        }

        #region 砸金蛋

        /// <summary>
        /// 砸金蛋首页
        /// </summary>
        /// <param name="WheelID"></param>
        /// <param name="ImageTextID"></param>
        /// <param name="User_ID"></param>
        /// <param name="UserWexinID"></param>
        /// <returns></returns>
        public ActionResult EggIndex(Guid WheelID, Guid ImageTextID, Guid User_ID, string UserWexinID = "")
        {
            var user = UserProfileRepository.GetByKey(User_ID);
            Session["UserId"] = user.UserId;
            Session["openId"] = UserWexinID;
            var wheel = WheelRepository.GetByKey(WheelID);
            var zan = PayCustomerRepository.FindAll(Specification<PayCustomer>.Eval(o => o.OpendId == UserWexinID && o.IsAward == 0)).SingleOrDefault();
            var za = PayCustomerRepository.FindAll(Specification<PayCustomer>.Eval(o => o.OpendId == UserWexinID)).ToList();

            if (DateTime.Now < wheel.StartDate)
            {
                ViewData["EggStatus"] = "活动暂未开始，敬请期待！";
            }
            else if (DateTime.Now > wheel.EndDate.AddDays(1))
            {
                ViewData["EggStatus"] = "活动已经结束了!";
            }
            else
            {
                ViewData["EggStatus"] = "进行中";

                if (zan != null)
                {
                    ViewBag.Isza = 1;
                }
                else if (za.Count == 0)
                {
                    ViewBag.Isza = -1;
                }
                else
                {
                    ViewBag.Isza = 0;
                }
            }

            var Wheel = WheelRepository.GetByKey(WheelID);
            return View(Wheel);
        }

        public ActionResult Eggwinning(string sn, string nickname, string awardname)
        {
            ViewBag.nickname = nickname;
            ViewBag.awardname = awardname;
            ViewBag.uSn = sn;
            return View();
        }

        public ActionResult Eggno_winning(string nickname)
        {
            ViewBag.nickname = nickname;
            return View();
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        public JsonResult GetEggitem(Guid WheelID, string UserWexinID = "")
        {
            var ip = Request.UserHostAddress;
            var dt = DateTime.Now.Date;
            var Wheel = WheelRepository.GetByKey(WheelID);
            GetEggCode(WheelID);//调用此方法是为了去检查是否有砸中了有奖金蛋没有提交用户信息，然而删除，空出名额
            //添加日志
            WheelLog WheelCode;
            string sdate = DateTime.Now.ToString("yyyy-MM-dd");
            DateTime ddate = Convert.ToDateTime(sdate);

            //获取中奖人数
            var logs = WheelLogRepository.FindAll(Specification<WheelLog>.Eval(o => o.WheelID == Wheel.WheelID && o.IsAward > 0)).ToList();

            //循环相加获取出后台设置好的奖品数量
            int num = 0;
            foreach (var item in Wheel.WheelItems)
            {
                num += item.WheelItemScale;
            }

            //var IsChou = WheelLogRepository.FindAll(Specification<WheelLog>.Eval(o => o.WheelID == Wheel.WheelID && o.WheelUserWexinID == UserWexinID)).ToList();
            var weiUser = PayCustomerRepository.FindAll(Specification<PayCustomer>.Eval(o => o.OpendId == UserWexinID)).ToList();
            var IsAwad = PayCustomerRepository.FindAll(Specification<PayCustomer>.Eval(o => o.OpendId == UserWexinID && o.IsAward == 0)).SingleOrDefault();

            //判断是否支付成功从来没有抽过
            if ((weiUser.Count > 0 && IsAwad != null))
            {
                //判断奖品数量是否超过指定
                if (logs.Count < num)
                {
                    //防止并发
                    Random rm = new Random();
                    Thread.Sleep(rm.Next(500, 1000));
                    WheelCode = GetEggCode(WheelID);
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
            EggItemViewModel Eggitem = new EggItemViewModel();

            if (IsAward > 0)
            {
                var Si = Wheel.WheelItems.ToList()[IsAward - 1];
                //Eggitem = Mapper.Map<WheelItem, WheelItemViewModel>(Si);
                Eggitem.nickname = IsAwad.nickname;
                Eggitem.WheelItemName = Si.WheelItemName;
                Eggitem.WheelItemAward = Si.WheelItemAward;
                Eggitem.WheelCode = WheelCode.WheelCode;
                Eggitem.WheelItemID = IsAward;
            }
            else
            {
                Eggitem.WheelCode = WheelCode.WheelCode;
                Eggitem.WheelItemID = IsAward;
                Eggitem.WheelItemName = "没有砸中!";
                Eggitem.nickname = IsAwad.nickname;
            }

            if (IsAwad != null && Wheel.StartDate < DateTime.Now && Wheel.EndDate.AddDays(1) > DateTime.Now)
            {
                //支付记录修改
                IsAwad.IsAward = 1;
                PayCustomerRepository.Update(IsAwad);
                PayCustomerRepository.Context.Commit();
                ///日志记录
                WheelLogRepository.Add(WheelCode);
                WheelLogRepository.Context.Commit();
                return Json(Eggitem);
            }
            else if (logs.Count > 1)
            {
                return Json(new { message = "只有一次砸金蛋的机会！" }, JsonRequestBehavior.AllowGet);
            }
            //else if (DateTime.Now < Wheel.StartDate)
            //{
            //    return Json(new { message = "活动暂未开始，敬请期待！" }, JsonRequestBehavior.AllowGet);

            //}
            //else if (DateTime.Now > Wheel.EndDate.AddDays(1))
            //{
            //    return Json(new { message = "活动已经结束了!" }, JsonRequestBehavior.AllowGet);
            //}
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 用户提交数据
        /// </summary>
        /// <param name="WheelUser">金蛋玩家实体，实体中只传递过来玩家的手机号码</param>
        /// <returns></returns>
        public JsonResult EggAdd(WheelUser WheelUser)
        {
            if (Session["WheelCode"] != null)
            {
                var WheelCode = (WheelLog)Session["WheelCode"];
                var Wheel = WheelRepository.Find(Specification<Wheel>.Eval(o => o.WheelID == WheelCode.WheelID));

                //循环判断是否已存在相同的手机号码,同样的微信号
                var users = WheelUserRepository.FindAll(Specification<WheelUser>.Eval(o => o.WheelID == Wheel.WheelID)).ToList();
                foreach (var item in users)
                {
                    if (item.WheelUserWexinID.Equals(WheelUser.WheelUserWexinID))
                    {
                        return Json(new { message = "每个ID最多可赢一张欢乐谷门票!" });
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
        public WheelLog GetEggCode(Guid WheelID)
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
                    log.WheelUserWexinID = Session["openId"].ToString();
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

        #endregion 砸金蛋
    }
}