using Apworks.Specifications;
using EasyWeixin.Core.Common;
using EasyWeixin.Data.Repositories;
using EasyWeixin.Helpers;
using EasyWeixin.Web.Filters;
using EasyWeixin.Web.Models;
using System;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace EasyWeixin.Web.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        private readonly IRoleRepository RoleRepository;

        private readonly IPermissionRepository PermissionRepository;
        private readonly IPermissionsInRolesRepository PermissionsInRolesRepository;
        private readonly IUserMembershipRepository UserMembershipRepository;
        private readonly IUserProfileRepository UserProfileRepository;

        public AdminController(IRoleRepository RoleRepository,
            IPermissionRepository PermissionRepository,
            IPermissionsInRolesRepository PermissionsInRolesRepository,
            IUserMembershipRepository UserMembershipRepository,
            IUserProfileRepository UserProfileRepository)
        {
            this.RoleRepository = RoleRepository;
            this.PermissionRepository = PermissionRepository;
            this.PermissionsInRolesRepository = PermissionsInRolesRepository;
            this.UserMembershipRepository = UserMembershipRepository;
            this.UserProfileRepository = UserProfileRepository;
        }

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 进入登陆页
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            try
            {
                if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
                {
                    if (!String.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return Redirect("/Admin/Index");
                    }
                }
                ModelState.AddModelError("", "用户名或密码错误");
                return View(model);
            }
            catch (HttpAntiForgeryException)
            {
                return View();
            }
        }

        /// <summary>
        /// 退出登陆
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOff()
        {
            WebSecurity.Logout();
            return Redirect("/Admin/Login");
        }

        /// <summary>
        /// 进入注册页
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// 提交注册
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            //获取四位随机数
            var fourRandomString = Function.GetRandomString(4);
            if (ModelState.IsValid)
            {
                // 尝试注册用户
                try
                {
                    WebSecurity.CreateUserAndAccount(model.UserName, model.Password);
                    WebSecurity.Login(model.UserName, model.Password);
                    EasyWeixin.Model.UserProfile user = UserProfileRepository.Find(Specification<EasyWeixin.Model.UserProfile>.Eval((e => e.UserName == model.UserName)));
                    user.Email = model.Email;
                    user.QQ = model.QQ;
                    user.Phone = model.Phone;
                    user.UserCode = user.ID.ToString().Replace("-", "");
                    user.Alt = fourRandomString;
                    user.WeixinToken = Function.MD5(user.ID.ToString().Replace("-", "") + fourRandomString);
                    user.DateCreated = DateTime.Now;
                    UserProfileRepository.Update(user);
                    UserProfileRepository.Context.Commit();
                    return Redirect("/Admin/Index");
                }
                catch (MembershipCreateUserException e)
                {
                    throw e;
                }
                catch (HttpAntiForgeryException)
                {
                    return View(model);
                }
            }

            return View(model);
        }

        /// <summary>
        /// 找回密码  这个功能还没有去实现
        /// </summary>
        /// <returns></returns>
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //[HttpPost]
        //public ActionResult ForgotPassword(ForgotPasswordViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        MembershipUserCollection users = Membership.FindUsersByEmail(model.Email);
        //        if (users.Count > 0)
        //        {
        //            foreach (MembershipUser user in users)
        //            {
        //                MailClient.SendLostPassword(model.Email, user.GetPassword());
        //            }
        //            return RedirectToAction("LogOn");
        //        }
        //    }
        //    return View(model);
        //}
        /// <summary>
        /// 是否已经存在邮箱 用于model层的远程调用
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None, NoStore = true)]   //清除缓存
        public JsonResult IsExistEmail(string Email)
        {
            bool valid = false;
            if (!UserProfileRepository.Exists(Specification<EasyWeixin.Model.UserProfile>.Eval(e => e.Email == Email)))
                valid = true;
            return Json(valid, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 是否已经存在该用户名 用于model层的远程调用
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None, NoStore = true)]   //清除缓存
        public JsonResult IsExistUserName(string UserName)
        {
            bool valid = false;
            if (!UserProfileRepository.Exists(Specification<EasyWeixin.Model.UserProfile>.Eval(e => e.UserName == UserName)))
                valid = true;
            return Json(valid, JsonRequestBehavior.AllowGet);
        }
    }
}