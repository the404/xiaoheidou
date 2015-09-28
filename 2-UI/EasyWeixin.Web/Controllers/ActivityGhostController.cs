using Apworks.Specifications;
using EasyWeixin.Data.Repositories;
using EasyWeixin.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace EasyWeixin.Web.Controllers
{
    public class ActivityGhostController : Controller
    {
        //天津欢乐谷恶魔快跑
        // GET: /ActivityGhost/
        private readonly IResponseImageTextRepository ResponseImageTextRepository;

        private readonly IUserProfileRepository UserProfileRepository;
        private readonly ISnowUserRepository SnowUserRepository;
        private readonly ISnowRepository SnowRepository;
        private readonly ISnowItemRepository SnowItemRepository;
        private readonly ISnowLogRepository SnowLogRepository;
        private readonly ISnowErrorLogRepository SnowErrorLogRepository;

        public ActivityGhostController(ISnowLogRepository SnowLogRepository,
            ISnowItemRepository SnowItemRepository,
            ISnowRepository SnowRepository,
            ISnowUserRepository SnowUserRepository,
            ISnowErrorLogRepository SnowErrorLogRepository,
            IResponseImageTextRepository ResponseImageTextRepository,
            IUserProfileRepository UserProfileRepository)
        {
            this.ResponseImageTextRepository = ResponseImageTextRepository;
            this.UserProfileRepository = UserProfileRepository;
            this.SnowUserRepository = SnowUserRepository;
            this.SnowRepository = SnowRepository;
            this.SnowItemRepository = SnowItemRepository;
            this.SnowLogRepository = SnowLogRepository;
            this.SnowErrorLogRepository = SnowErrorLogRepository;
        }

        /// <summary>
        /// 恶魔快跑游戏开始页
        /// </summary>
        /// <param name="SnowID"></param>
        /// <param name="UserWexinID"></param>
        /// <returns></returns>
        public ActionResult GhostIndex(Guid SnowID, string UserWexinID = "")
        {
            var Snow = SnowRepository.GetByKey(SnowID);
            if (DateTime.Now < Snow.StartDate || DateTime.Now > Snow.EndDate.AddDays(1))
            {
                ViewData["SnowStatus"] = "活动已经结束！";
            }
            else
            {
                ViewData["SnowStatus"] = null;
            }
            return View();
        }

        /// <summary>
        /// 恶魔快跑游戏排行榜
        /// </summary>
        /// <param name="SnowID"></param>
        /// <param name="UserWexinID">微信账号，也就是微信接口中返回来的openid字段，可以为空 因为这个</param>
        /// <returns></returns>
        public ActionResult GhostRank(Guid SnowID, string UserWexinID = "")
        {
            var Snow = SnowRepository.GetByKey(SnowID);
            var EndDate = Snow.EndDate.AddDays(1);
            //var Leader = SnowUserRepository.FindAll(Specification<SnowUser>.Eval(o => o.SnowID == Snow.SnowID && o.AddDate > Snow.StartDate && o.AddDate < EndDate)).OrderByDescending(o => o.Score).Take(15).ToList();
            var Leader = SnowUserRepository.FindAll(Specification<SnowUser>.Eval(o => o.SnowID == Snow.SnowID && o.AddDate > Snow.StartDate && o.AddDate < EndDate)).OrderByDescending(o => o.Score).GroupBy(ic => ic.SnowUserPhone).Select(g => g.First()).Take(15).ToList();
            return View(Leader);
        }

        /// <summary>
        /// 恶魔快跑游戏界面
        /// 由于奖品起初设定的是一个月，奖品的数量正好是一天两个，所以将日票和夜票归于一类，杯子和报纸归于一类
        /// </summary>
        /// <param name="SnowID"></param>
        /// <param name="UserWexinID"></param>
        /// <returns></returns>
        public ActionResult GhostGame(Guid SnowID, string UserWexinID = "")
        {
            var snow = SnowRepository.GetByKey(SnowID);
            if (DateTime.Now < snow.StartDate || DateTime.Now > snow.EndDate.AddDays(1))
            {
                ViewData["SnowDatas"] = null;
                return View();
            }
            var nowdate = DateTime.Now.Date;
            var now = DateTime.Now;
            var items = snow.SnowItems.ToList();
            var SnowItemDatas = new List<FruitItemData>();
            var IsAward = 0;
            //获取当日中奖日志数
            var startdate = snow.StartDate;
            var enddate = snow.EndDate;
            //查询是否有人中奖了，但未提交信息的
            var SLogs = SnowLogRepository.FindAll(Specification<SnowLog>.Eval(o => o.IsAward > 0 && o.IsAward < 100 && o.SnowID == snow.SnowID && o.AddDate > nowdate)).ToList();
            foreach (var item in SLogs)
            {
                if (item.AddDate.AddMinutes(30) < now)
                {
                    var SnowUsers = SnowUserRepository.FindAll(Specification<SnowUser>.Eval(o => o.SnowLogID == item.SnowLogID)).ToList();
                    if (SnowUsers.Count == 0)
                    {
                        SnowLogRepository.Remove(item);
                        SnowLogRepository.Context.Commit();
                    }
                }
            }
            //第一种奖励
            //var SnowUsers1 = snow.SnowUsers.Where(o => o.IsAward == 1 && o.AddDate < nowdate && o.AddDate > snow.StartDate).ToList();
            //var count1 = ((now - startdate).Days) - SnowUsers1.Count + 1;
            //var SnowLogs1 = SnowLogRepository.FindAll(Specification<SnowLog>.Eval(o => o.IsAward == 1 && o.SnowID == snow.SnowID && o.AddDate > nowdate)).ToList();
            //if (SnowLogs1.Count < count1 && count1 > 0)
            //{
            //    //设置奖励发放时间
            //    if (now > DateTime.Parse(nowdate.ToShortDateString() + " 8:00:00"))
            //    {
            //        IsAward = 1;
            //    }
            //}
            //第二种奖励
            //var SnowUsers2 = snow.SnowUsers.Where(o => o.IsAward == 2 && o.AddDate < nowdate && o.AddDate > snow.StartDate).ToList();
            //var count2 = ((now - startdate).Days) - SnowUsers2.Count;
            //var SnowLogs2 = SnowLogRepository.FindAll(Specification<SnowLog>.Eval(o => o.IsAward == 2 && o.SnowID == snow.SnowID && o.AddDate > nowdate)).ToList();
            //if (SnowLogs2.Count < count2 && count2 > 0)
            //{
            //    //设置奖励发放时间
            //    if (now > DateTime.Parse(nowdate.ToShortDateString() + " 12:00:00") && now > startdate.AddDays(1))
            //    {
            //        IsAward = 2;
            //    }
            //}
            //提交日志
            SnowLog sl = new SnowLog();
            sl.AddDate = DateTime.Now;
            sl.IP = Request.UserHostAddress;
            sl.IsAward = IsAward;
            //  sl.SnowData = NewSnowItemDatas.ToString();
            sl.SnowID = snow.SnowID;
            SnowLogRepository.Add(sl);
            SnowLogRepository.Context.Commit();
            //防止并发
            //if (IsAward == 1)
            //{
            //    //获取当日中奖日志数
            //    var SnowLogs = SnowLogRepository.FindAll(Specification<SnowLog>.Eval(o => o.IsAward == 1 && o.SnowID == snow.SnowID && o.AddDate > nowdate)).ToList();
            //    if (SnowLogs.Count > count1)
            //    {
            //        for (int i = 0; i < SnowLogs.Count; i++)
            //        {
            //            if (i > (count1 - 1) && SnowLogs[i].SnowLogID == sl.SnowLogID)
            //            {
            //                IsAward = 0;
            //                sl.IsAward = 0;
            //                SnowLogRepository.Update(sl);
            //                SnowLogRepository.Context.Commit();
            //            }
            //        }
            //    }
            //}
            //if (IsAward == 2)
            //{
            //    //获取当日中奖日志数
            //    var SnowLogs = SnowLogRepository.FindAll(Specification<SnowLog>.Eval(o => o.IsAward == 2 && o.SnowID == snow.SnowID && o.AddDate > nowdate)).ToList();
            //    if (SnowLogs.Count > count2)
            //    {
            //        for (int i = 0; i < SnowLogs.Count; i++)
            //        {
            //            if (i > (count2 - 1) && SnowLogs[i].SnowLogID == sl.SnowLogID)
            //            {
            //                IsAward = 0;
            //                sl.IsAward = 0;
            //                SnowLogRepository.Update(sl);
            //                SnowLogRepository.Context.Commit();
            //            }
            //        }
            //    }
            //}
            Session["SnowLog"] = sl;
            //
            for (int j = 0; j < items.Count; j++)
            {
                for (int i = 0; i < items[j].SnowItemScale; i++)
                {
                    var s = new FruitItemData();
                    s.DataID = Guid.NewGuid();
                    s.Score = items[j].SnowScore;
                    s.IsAward = IsAward;
                    SnowItemDatas.Add(s);
                }
            }
            while (SnowItemDatas.Count < snow.SnowScale)
            {
                var s = new FruitItemData();
                s.DataID = Guid.NewGuid();
                s.Score = 100;
                s.IsAward = IsAward;
                SnowItemDatas.Add(s);
            }
            var NewSnowItemDatas = new List<FruitItemData>();
            for (int i = 0; i < 10; i++)
            {
                int index = getRandow(SnowItemDatas.Count - 1);
                var item = SnowItemDatas[index];
                NewSnowItemDatas.Add(item);
                SnowItemDatas.Remove(item);
            }
            //保存session
            Session["SnowItemDatas"] = NewSnowItemDatas;
            ViewData["SnowDatas"] = JsonConvert.SerializeObject(NewSnowItemDatas);
            return View();
        }

        /// <summary>
        /// 保存游戏数据
        /// </summary>
        /// <param name="datas">为一个josn字符串</param>
        /// <param name="UserName">用户名</param>
        /// <param name="Phone">电话</param>
        /// <param name="SnowID">活动的guid</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveSnowGameData(string datas, string UserName, string Phone, string CardId, Guid SnowID)
        {
            SnowErrorLog sel = new SnowErrorLog();
            var snow = SnowRepository.GetByKey(SnowID);
            var SaveDatas = JsonConvert.DeserializeObject<List<FruitItemData>>(datas);
            //防止重复提交
            if (Session["SnowItemDatas"] == null || Session["SnowLog"] == null)
            {
                sel.Message = "重复提交！";
                sel.Action = "SaveSnowGameData";
                sel.IP = Request.UserHostAddress;
                sel.AddDate = DateTime.Now;
                sel.SnowID = snow.SnowID;
                SnowErrorLogRepository.Add(sel);
                SnowErrorLogRepository.Context.Commit();
                return Json(new { Message = "提交数据失败，请勿重复提交！！！" });
            }
            var SessionDatas = (List<FruitItemData>)Session["SnowItemDatas"];
            var SessionLog = (SnowLog)Session["SnowLog"];
            //验证数据
            var Progress = "";
            //记录玩的过程
            foreach (var item in SaveDatas)
            {
                if (item != null)
                {
                    Progress += item.Score + "|";
                }
                else
                {
                    SaveDatas.Remove(item);
                }
            }
            //限制提交作弊
            if (SaveDatas.Where(o => o.DataID != Guid.Empty).ToList().Count > 10 && SaveDatas.Count > 80 && !SaveDatas.Exists(o => o.Score > 100 && o.DataID == Guid.Empty) && !SaveDatas.Exists(o => o.Score > 500))
            {
                sel.Message = "提交大数据偏多！";
                sel.Action = "SaveSnowGameData";
                sel.IP = Request.UserHostAddress;
                sel.AddDate = DateTime.Now;
                sel.SnowID = snow.SnowID;
                SnowErrorLogRepository.Add(sel);
                SnowErrorLogRepository.Context.Commit();
                return Json(new { Message = "提交数据失败！！！" });
            }
            var SumScore = SaveDatas.Sum(o => o.Score);
            SnowUser sn = new SnowUser();
            sn.AddDate = DateTime.Now;
            sn.IP = Request.UserHostAddress;
            sn.IsAward = SessionDatas[0].IsAward;
            sn.SnowUserPhone = Phone;
            sn.SnowUserName = UserName;

            //身份证，砸金蛋不需要
            sn.Identification = CardId;

            sn.SnowProgress = Progress;
            sn.Score = SumScore;
            sn.SnowLogID = SessionLog.SnowLogID;
            sn.UserId = SessionLog.Snow.UserProfile.UserId;
            sn.SnowID = SessionLog.SnowID;
            SnowUserRepository.Add(sn);
            SnowUserRepository.Context.Commit();
            Session["SnowItemDatas"] = null;
            Session["SnowLog"] = null;
            return Json(new { Message = "提交数据成功！！！" });
        }

        public int getRandow(int count)
        {
            //获取一个随机数 随机抽奖
            Random randow = new Random();
            int ran = randow.Next(0, count);
            return ran;
        }
    }

    public class GhostItemData
    {
        public Guid DataID { set; get; }

        public int Score { set; get; }

        public int IsAward { set; get; }
    }
}