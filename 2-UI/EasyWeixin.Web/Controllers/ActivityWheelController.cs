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
    public class ActivityWheelController : Controller
    {
        //微信大转盘活动
        // GET: /ActivityWheel/

        private readonly IResponseImageTextRepository ResponseImageTextRepository;
        private readonly IUserProfileRepository UserProfileRepository;
        private readonly IWheelUserRepository WheelUserRepository;
        private readonly IWheelRepository WheelRepository;
        private readonly IWheelItemRepository WheelItemRepository;
        private readonly IWheelLogRepository WheelLogRepository;

        public ActivityWheelController(IWheelLogRepository WheelLogRepository,
            IWheelItemRepository WheelItemRepository,
            IWheelRepository WheelRepository,
            IWheelUserRepository WheelUserRepository,
            IResponseImageTextRepository ResponseImageTextRepository,
            IUserProfileRepository UserProfileRepository)
        {
            this.ResponseImageTextRepository = ResponseImageTextRepository;
            this.UserProfileRepository = UserProfileRepository;
            this.WheelUserRepository = WheelUserRepository;
            this.WheelRepository = WheelRepository;
            this.WheelItemRepository = WheelItemRepository;
            this.WheelLogRepository = WheelLogRepository;
        }

        #region 大转盘

        /// <summary>
        /// 大转盘首页
        /// </summary>
        /// <param name="WheelID"></param>
        /// <param name="ImageTextID"></param>
        /// <param name="User_ID"></param>
        /// <param name="UserWexinID"></param>
        /// <returns></returns>
        public ActionResult WheelIndex(Guid WheelID, Guid ImageTextID, Guid User_ID, string UserWexinID = "")
        {
            var user = UserProfileRepository.GetByKey(User_ID);
            Session["UserId"] = user.UserId;
            var Wheel = WheelRepository.GetByKey(WheelID);
            return View(Wheel);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        public JsonResult GetWheelitem(Guid WheelID, string UserWexinID = "")
        {
            var Wheel = WheelRepository.GetByKey(WheelID);
            if (Request.UserAgent.ToLower().IndexOf("micromessenger") == -1)
            {
                return Json(new { message = "每日只能提交" + Wheel.EveryDayTimes.ToString() + "次." }, JsonRequestBehavior.AllowGet);
            }

            var ip = Request.UserHostAddress;
            var dt = DateTime.Now.Date;
            //清理过期未领取的奖品
            ClearExpiredAward(WheelID);
            //GetWheelCode(WheelID, UserWexinID);
            //添加日志
            WheelLog WheelCode;
            var logs = WheelLogRepository.FindAll(Specification<WheelLog>.Eval(o => o.WheelID == Wheel.WheelID && o.IsAward > 0)).ToList();
            // var users = WheelUserRepository.FindAll(Specification<WheelUser>.Eval(o => o.WheelID == Wheel.WheelID)).ToList();
            int num = 0;
            foreach (var item in Wheel.WheelItems)
            {
                num += item.WheelItemScale;
            }

            // if (logs.Count >= num)
            if (logs.Count >= num)
            {
                WheelCode = new WheelLog();
                WheelCode.WheelCode = "0";
                WheelCode.IsAward = 0;
                WheelCode.AddDate = DateTime.Now;
                WheelCode.WheelID = Wheel.WheelID;
                WheelCode.WheelUserWexinID = UserWexinID;
            }
            else
            {
                WheelCode = GetWheelCode(WheelID, UserWexinID);
            }

            //var WheelCode = GetWheelCode(WheelID);
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
                //由于大转盘有十二个位置点，按照索引从0开始到11，其中，0,4,8为中将点，所以设置一个没有0，4，8的不中间数组，而后进行随机处理
                int[] arr = { 1, 2, 3, 5, 6, 7, 9, 10, 11 };
                Random randow = new Random();
                int ran = randow.Next(0, arr.Length - 1);
                string Angle = GetAngle(arr[ran], 12, 5);
                Wheelitem.WheelCode = WheelCode.WheelCode;
                Wheelitem.WheelItemID = IsAward;
                Wheelitem.MaxAngle = Angle.Split('|')[1];
                Wheelitem.MinAngle = Angle.Split('|')[0];
                Wheelitem.WheelItemName = "谢谢惠顾";
            }

            var log = WheelLogRepository.FindAll(Specification<WheelLog>.Eval(o => o.WheelUserWexinID == UserWexinID && o.AddDate > dt && o.WheelID == Wheel.WheelID)).ToList();
            if (log.Count < Wheel.EveryDayTimes && Wheel.StartDate < DateTime.Now && Wheel.EndDate.AddDays(1) > DateTime.Now)
            {
                ///日志记录
                WheelLogRepository.Add(WheelCode);
                WheelLogRepository.Context.Commit();
                return Json(Wheelitem);
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
                return Json(new { message = "每日只能提交" + Wheel.EveryDayTimes.ToString() + "次" }, JsonRequestBehavior.AllowGet);
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
        public JsonResult WheelAdd(WheelUser WheelUser)
        {
            if (Session["WheelCode"] != null)
            {
                var WheelCode = (WheelLog)Session["WheelCode"];
                var Wheel = WheelRepository.Find(Specification<Wheel>.Eval(o => o.WheelID == WheelCode.WheelID));

                //循环判断是否已存在相同的手机号码
                var users = WheelUserRepository.FindAll(Specification<WheelUser>.Eval(o => o.WheelID == Wheel.WheelID)).ToList();
                foreach (var item in users)
                {
                    if (item.WheelUserPhone.Equals(WheelUser.WheelUserPhone))
                    {
                        return Json(new { message = "您提交的手机号码已经中过奖了!" });
                    }
                }

                //var log = WheelLogRepository.FindAll(Specification<WheelLog>.Eval(o => o.WheelID == Wheel.WheelID)).ToList();

                //foreach (var m in log)
                //{
                //    var wheelusers = WheelUserRepository.FindAll(Specification<WheelUser>.Eval(o => o.WheelID == Wheel.WheelID && o.WheelLogID == m.WheelLogID)).ToList();
                //    if (wheelusers.FirstOrDefault() == null)
                //    {
                //        //删除因为输入相同手机号码录入的日志信息
                //        WheelLogRepository.Remove(m);
                //        WheelLogRepository.Context.Commit();
                //    }

                //}

                var ip = Request.UserHostAddress;
                var dt = DateTime.Now.Date;
                WheelUser.WheelItemID = Wheel.WheelItems.ToList()[WheelCode.IsAward - 1].WheelItemID;
                WheelUser.WheelID = WheelCode.WheelID;
                WheelUser.IP = Request.UserHostAddress;
                WheelUser.WheelCode = WheelCode.WheelCode;
                WheelUser.UserId = int.Parse(Session["UserId"].ToString());
                WheelUser.AddDate = DateTime.Now;
                WheelUser.WheelLogID = WheelCode.WheelLogID;
                WheelUserRepository.Add(WheelUser);
                WheelUserRepository.Context.Commit();
                Session["WheelCode"] = null;
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
        public WheelLog GetWheelCode(Guid WheelID, string UserWexinID)
        {
            var Wheel = WheelRepository.GetByKey(WheelID);
            var users = WheelUserRepository.FindAll(Specification<WheelUser>.Eval(o => o.WheelID == Wheel.WheelID)).ToList();
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
                    log.WheelUserWexinID = UserWexinID;
                    WheelCodes.Add(log);
                }
            }

            int AwardingNum = WheelCodes.Count();
            Random rd = new Random();
            int rdnum = rd.Next(1, container + AwardingNum);
            if (rdnum <= AwardingNum)
            {
                return WheelCodes[rdnum - 1];
            }
            else
            {
                return new WheelLog()
                {
                    WheelUserWexinID = UserWexinID,
                    IsAward = 0,
                    AddDate = DateTime.Now,
                    WheelID = Wheel.WheelID,
                    WheelCode = "0"
                };
            }
        }

        #endregion 大转盘

        /// <summary>
        /// 清理过期未领取的奖品
        /// </summary>
        /// <param name="WheelID"></param>
        private void ClearExpiredAward(Guid WheelID)
        {
            DateTime dt = DateTime.Now.AddMinutes(-10);
            var Wheel = WheelRepository.GetByKey(WheelID);
            var users = WheelUserRepository.FindAll(Specification<WheelUser>.Eval(o => o.WheelID == Wheel.WheelID)).ToList();
            //清理日志  如果用户中奖 10分钟里没有提交信息 就将日志其删除
            var logs = WheelLogRepository.FindAll(Specification<WheelLog>.Eval(o => o.WheelID == Wheel.WheelID && o.IsAward > 0 && o.AddDate < dt)).ToList();
            foreach (var item in logs)
            {
                if (!users.Exists(o => o.WheelLogID == item.WheelLogID))
                {
                    WheelLogRepository.Remove(item);
                    WheelLogRepository.Context.Commit();
                }
            }
        }
    }
}