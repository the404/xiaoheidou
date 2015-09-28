using Apworks.Specifications;
using EasyWeixin.Data.Repositories;
using EasyWeixin.Helpers;
using EasyWeixin.Model;
using System;
using System.Linq;
using System.Web.Mvc;

namespace EasyWeixin.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserProfileRepository UserProfileRepository;
        private readonly IReadRepository ReadRepository;
        private readonly IPraiseRepository PraiseRepository;

        public HomeController(IReadRepository ReadRepository,
            IUserProfileRepository UserProfileRepository,
            IPraiseRepository PraiseRepository)
        {
            this.UserProfileRepository = UserProfileRepository;
            this.ReadRepository = ReadRepository;
            this.PraiseRepository = PraiseRepository;
        }

        public HomeController()
        {
        }

        /// <summary>
        /// 添加阅读数
        /// </summary>
        /// <returns></returns>
        public JsonResult AddReadCount(string rUrl)
        {
            //添加该链接
            Read r = new Read();
            r.ViewUrl = rUrl;
            r.CreateIp = Request.UserHostAddress;
            r.CreateDate = DateTime.Now;
            ReadRepository.Add(r);
            ReadRepository.Context.Commit();

            //获得该链接的所有阅读数
            var rr = ReadRepository.FindAll(Specification<Read>.Eval(o => o.ViewUrl.Equals(rUrl))).ToList();
            return Json(new { rCount = rr.Count() }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加点赞
        /// </summary>
        /// <returns></returns>
        public JsonResult GetZanCount(string rUrl)
        {
            string isExits = CookieHelper.GetCookie(Request, "u_z");
            if (isExits == "")
            {
                CookieHelper.AddCookie(Response, "u_z", DateTime.Now.ToString("yyyyMMddHHmmssfff"), 365, 0, 0, 0);
            }

            //获得该链接的所有点赞数
            var zz = PraiseRepository.FindAll(Specification<Praise>.Eval(o => o.ViewUrl.Equals(rUrl) && o.Cookie == isExits)).Any();
            var num = PraiseRepository.FindAll(Specification<Praise>.Eval(o => o.ViewUrl.Equals(rUrl))).ToList();
            return Json(new { IsZan = zz, rCount = num.Count }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddOrDeleteZan(string rUrl, string act)
        {
            string isExits = CookieHelper.GetCookie(Request, "u_z");
            if (isExits == "")
            {
                return null;
            }
            //判断是否已赞过
            var zz = PraiseRepository.FindAll(Specification<Praise>.Eval(o => o.ViewUrl.Equals(rUrl) && o.Cookie == isExits)).Any();
            //判断是否是点赞还是取消赞
            if (act == "1" && zz == false)
            {
                //添加赞
                Praise r = new Praise();
                r.ViewUrl = rUrl;
                r.CreateIp = Request.UserHostAddress;
                r.CreateDate = DateTime.Now;
                r.Cookie = isExits;
                PraiseRepository.Add(r);
                PraiseRepository.Context.Commit();
            }
            else
            {
                //取消赞
                var zlist = PraiseRepository.FindAll(Specification<Praise>.Eval(o => o.ViewUrl.Equals(rUrl) && o.Cookie == isExits)).ToList();
                foreach (var item in zlist)
                {
                    if (item != null)
                    {
                        PraiseRepository.Remove(item);
                        PraiseRepository.Context.Commit();
                    }
                }
            }

            var newzz = PraiseRepository.FindAll(Specification<Praise>.Eval(o => o.ViewUrl.Equals(rUrl))).ToList();
            return Json(new { rCount = newzz.Count }, JsonRequestBehavior.AllowGet);
        }
    }
}