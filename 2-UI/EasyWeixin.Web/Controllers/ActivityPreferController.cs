using Apworks.Specifications;
using EasyWeixin.Data.Repositories;
using EasyWeixin.Model;
using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace EasyWeixin.Web.Controllers
{
    public class ActivityPreferController : Controller
    {
        //优惠显示界面
        // GET: /ActivityPrefer/
        private readonly IUserProfileRepository UserProfileRepository;

        private readonly IPreferRepository PreferRepository;

        public ActivityPreferController(IPreferRepository PreferRepository,
            IUserProfileRepository UserProfileRepository)
        {
            this.UserProfileRepository = UserProfileRepository;
            this.PreferRepository = PreferRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        #region 上海

        public ActionResult PreferIndex(Guid PreferID, string UserWexinID = "")
        {
            var perfer = PreferRepository.GetByKey(PreferID);
            perfer.Clicks++;
            PreferRepository.Update(perfer);
            PreferRepository.Context.Commit();
            // var EndDate = prefer.EndDate.AddDays(1);
            //var Leader = SnowUserRepository.FindAll(Specification<SnowUser>.Eval(o => o.SnowID == Snow.SnowID && o.AddDate > Snow.StartDate && o.AddDate < EndDate)).OrderByDescending(o => o.Score).Take(15).ToList();
            var Leader = PreferRepository.FindAll(Specification<Prefer>.Eval(o => o.UserId == perfer.UserId))
                .OrderByDescending(obj => obj.IsTop)
                .ThenByDescending(obj => obj.TopTime)
                .ThenByDescending(obj => obj.AddDate).ToList();
            return View(Leader);
        }

        public ActionResult PreferDetail(Guid PreID, string UserWexinID = "")
        {
            Prefer pp = PreferRepository.GetByKey(PreID);
            return View(pp);
        }

        #endregion 上海

        #region 天津

        public ActionResult TPreferIndex(Guid PreferID, string UserWexinID = "")
        {
            var perfer = PreferRepository.GetByKey(PreferID);
            perfer.Clicks++;
            PreferRepository.Update(perfer);
            PreferRepository.Context.Commit();
            // var EndDate = prefer.EndDate.AddDays(1);
            //var Leader = SnowUserRepository.FindAll(Specification<SnowUser>.Eval(o => o.SnowID == Snow.SnowID && o.AddDate > Snow.StartDate && o.AddDate < EndDate)).OrderByDescending(o => o.Score).Take(15).ToList();
            var Leader = PreferRepository.FindAll(Specification<Prefer>.Eval(o => o.UserId == perfer.UserId))
                .OrderByDescending(obj => obj.IsTop)
                .ThenByDescending(obj => obj.TopTime)
                .ThenByDescending(obj => obj.AddDate).ToList();
            return View(Leader);
        }

        public ActionResult TPreferDetail(Guid PreID, string UserWexinID = "")
        {
            Prefer pp = PreferRepository.GetByKey(PreID);
            return View(pp);
        }

        #endregion 天津

        #region 武汉

        public ActionResult WPreferIndex(Guid PreferID, string UserWexinID = "")
        {
            var perfer = PreferRepository.GetByKey(PreferID);
            perfer.Clicks++;
            PreferRepository.Update(perfer);
            PreferRepository.Context.Commit();
            // var EndDate = prefer.EndDate.AddDays(1);
            //var Leader = SnowUserRepository.FindAll(Specification<SnowUser>.Eval(o => o.SnowID == Snow.SnowID && o.AddDate > Snow.StartDate && o.AddDate < EndDate)).OrderByDescending(o => o.Score).Take(15).ToList();
            var Leader = PreferRepository.FindAll(Specification<Prefer>.Eval(o => o.UserId == perfer.UserId))
                .OrderByDescending(obj => obj.IsTop)
                .ThenByDescending(obj => obj.TopTime)
                .ThenByDescending(obj => obj.AddDate).ToList();
            return View(Leader);
        }

        public ActionResult WPreferDetail(Guid PreID, string UserWexinID = "")
        {
            Prefer pp = PreferRepository.GetByKey(PreID);
            return View(pp);
        }

        #endregion 武汉

        #region 云南

        public ActionResult YPreferIndex(Guid PreferID, string UserWexinID = "")
        {
            var perfer = PreferRepository.GetByKey(PreferID);
            perfer.Clicks++;
            PreferRepository.Update(perfer);
            PreferRepository.Context.Commit();
            // var EndDate = prefer.EndDate.AddDays(1);
            //var Leader = SnowUserRepository.FindAll(Specification<SnowUser>.Eval(o => o.SnowID == Snow.SnowID && o.AddDate > Snow.StartDate && o.AddDate < EndDate)).OrderByDescending(o => o.Score).Take(15).ToList();
            var Leader = PreferRepository.FindAll(Specification<Prefer>.Eval(o => o.UserId == perfer.UserId))
                .OrderByDescending(obj => obj.IsTop)
                .ThenByDescending(obj => obj.TopTime)
                .ThenByDescending(obj => obj.AddDate).ToList();
            return View(Leader);
        }

        public ActionResult YPreferDetail(Guid PreID, string UserWexinID = "")
        {
            Prefer pp = PreferRepository.GetByKey(PreID);
            return View(pp);
        }

        #endregion 云南

        #region 泰州

        public ActionResult ZPreferIndex(Guid PreferID, string UserWexinID = "")
        {
            var perfer = PreferRepository.GetByKey(PreferID);
            perfer.Clicks++;
            PreferRepository.Update(perfer);
            PreferRepository.Context.Commit();
            // var EndDate = prefer.EndDate.AddDays(1);
            //var Leader = SnowUserRepository.FindAll(Specification<SnowUser>.Eval(o => o.SnowID == Snow.SnowID && o.AddDate > Snow.StartDate && o.AddDate < EndDate)).OrderByDescending(o => o.Score).Take(15).ToList();
            var Leader = PreferRepository.FindAll(Specification<Prefer>.Eval(o => o.UserId == perfer.UserId))
                .OrderByDescending(obj => obj.IsTop)
                .ThenByDescending(obj => obj.TopTime)
                .ThenByDescending(obj => obj.AddDate).ToList();
            return View(Leader);
        }

        public ActionResult ZPreferDetail(Guid PreID, string UserWexinID = "")
        {
            Prefer pp = PreferRepository.GetByKey(PreID);
            return View(pp);
        }

        #endregion 泰州
    }
}