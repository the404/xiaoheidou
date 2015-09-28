using Apworks.Specifications;
using EasyWeixin.Data.Repositories;
using EasyWeixin.Model;
using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace EasyWeixin.Web.Controllers
{
    public class ActivityActController : Controller
    {
        //活动显示界面
        // GET: /ActivityAct/
        private readonly IUserProfileRepository UserProfileRepository;

        private readonly IActRepository ActRepository;

        public ActivityActController(IActRepository ActRepository,
            IUserProfileRepository UserProfileRepository)
        {
            this.UserProfileRepository = UserProfileRepository;
            this.ActRepository = ActRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        #region 上海

        public ActionResult ActIndex(Guid ActID, string UserWexinID = "")
        {
            var act = ActRepository.GetByKey(ActID);
            act.Clicks++;
            ActRepository.Update(act);
            ActRepository.Context.Commit();
            // var EndDate = prefer.EndDate.AddDays(1);
            //var Leader = SnowUserRepository.FindAll(Specification<SnowUser>.Eval(o => o.SnowID == Snow.SnowID && o.AddDate > Snow.StartDate && o.AddDate < EndDate)).OrderByDescending(o => o.Score).Take(15).ToList();
            var Leader = ActRepository.FindAll(Specification<Act>.Eval(o => o.UserId == act.UserId))
                .OrderByDescending(obj => obj.IsTop)
                .ThenByDescending(obj => obj.TopTime)
                .ThenByDescending(obj => obj.AddDate).ToList();
            return View(Leader);
        }

        public ActionResult ActDetail(Guid ActID, string UserWexinID = "")
        {
            Act pp = ActRepository.GetByKey(ActID);
            return View(pp);
        }

        #endregion 上海

        #region 天津

        public ActionResult TActIndex(Guid ActID, string UserWexinID = "")
        {
            var act = ActRepository.GetByKey(ActID);
            act.Clicks++;
            ActRepository.Update(act);
            ActRepository.Context.Commit();
            // var EndDate = prefer.EndDate.AddDays(1);
            //var Leader = SnowUserRepository.FindAll(Specification<SnowUser>.Eval(o => o.SnowID == Snow.SnowID && o.AddDate > Snow.StartDate && o.AddDate < EndDate)).OrderByDescending(o => o.Score).Take(15).ToList();
            var Leader = ActRepository.FindAll(Specification<Act>.Eval(o => o.UserId == act.UserId))
                .OrderByDescending(obj => obj.IsTop)
                .ThenByDescending(obj => obj.TopTime)
                .ThenByDescending(obj => obj.AddDate).ToList();
            return View(Leader);
        }

        public ActionResult TActDetail(Guid ActID, string UserWexinID = "")
        {
            Act pp = ActRepository.GetByKey(ActID);
            return View(pp);
        }

        #endregion 天津

        #region 武汉

        public ActionResult WActIndex(Guid ActID, string UserWexinID = "")
        {
            var act = ActRepository.GetByKey(ActID);
            act.Clicks++;
            ActRepository.Update(act);
            ActRepository.Context.Commit();
            // var EndDate = prefer.EndDate.AddDays(1);
            //var Leader = SnowUserRepository.FindAll(Specification<SnowUser>.Eval(o => o.SnowID == Snow.SnowID && o.AddDate > Snow.StartDate && o.AddDate < EndDate)).OrderByDescending(o => o.Score).Take(15).ToList();
            var Leader = ActRepository.FindAll(Specification<Act>.Eval(o => o.UserId == act.UserId))
                .OrderByDescending(obj => obj.IsTop)
                .ThenByDescending(obj => obj.TopTime)
                .ThenByDescending(obj => obj.AddDate).ToList();
            return View(Leader);
        }

        public ActionResult WActDetail(Guid ActID, string UserWexinID = "")
        {
            Act pp = ActRepository.GetByKey(ActID);
            return View(pp);
        }

        #endregion 武汉

        #region 东部

        public ActionResult DActIndex(Guid ActID, string UserWexinID = "")
        {
            var act = ActRepository.GetByKey(ActID);
            act.Clicks++;
            ActRepository.Update(act);
            ActRepository.Context.Commit();
            var Leader = ActRepository.FindAll(Specification<Act>.Eval(o => o.UserId == act.UserId))
                .OrderByDescending(obj => obj.IsTop)
                .ThenByDescending(obj => obj.TopTime)
                .ThenByDescending(obj => obj.AddDate).ToList();
            return View(Leader);
        }

        public ActionResult DActDetail(Guid ActID, string UserWexinID = "")
        {
            Act pp = ActRepository.GetByKey(ActID);
            return View(pp);
        }

        #endregion 东部

        #region 云南

        public ActionResult YActIndex(Guid ActID, string UserWexinID = "")
        {
            var act = ActRepository.GetByKey(ActID);
            act.Clicks++;
            ActRepository.Update(act);
            ActRepository.Context.Commit();
            // var EndDate = prefer.EndDate.AddDays(1);
            //var Leader = SnowUserRepository.FindAll(Specification<SnowUser>.Eval(o => o.SnowID == Snow.SnowID && o.AddDate > Snow.StartDate && o.AddDate < EndDate)).OrderByDescending(o => o.Score).Take(15).ToList();
            var Leader = ActRepository.FindAll(Specification<Act>.Eval(o => o.UserId == act.UserId))
                .OrderByDescending(obj => obj.IsTop)
                .ThenByDescending(obj => obj.TopTime)
                .ThenByDescending(obj => obj.AddDate).ToList();
            return View(Leader);
        }

        public ActionResult YActDetail(Guid ActID, string UserWexinID = "")
        {
            Act pp = ActRepository.GetByKey(ActID);
            return View(pp);
        }

        #endregion 云南

        #region 泰州

        public ActionResult ZActIndex(Guid ActID, string UserWexinID = "")
        {
            var act = ActRepository.GetByKey(ActID);
            act.Clicks++;
            ActRepository.Update(act);
            ActRepository.Context.Commit();
            // var EndDate = prefer.EndDate.AddDays(1);
            //var Leader = SnowUserRepository.FindAll(Specification<SnowUser>.Eval(o => o.SnowID == Snow.SnowID && o.AddDate > Snow.StartDate && o.AddDate < EndDate)).OrderByDescending(o => o.Score).Take(15).ToList();
            var Leader = ActRepository.FindAll(Specification<Act>.Eval(o => o.UserId == act.UserId))
                .OrderByDescending(obj => obj.IsTop)
                .ThenByDescending(obj => obj.TopTime)
                .ThenByDescending(obj => obj.AddDate).ToList();
            return View(Leader);
        }

        public ActionResult ZActDetail(Guid ActID, string UserWexinID = "")
        {
            Act pp = ActRepository.GetByKey(ActID);
            return View(pp);
        }

        #endregion 泰州
    }
}