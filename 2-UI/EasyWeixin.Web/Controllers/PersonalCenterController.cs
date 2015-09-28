using Apworks.Specifications;
using AutoMapper;
using EasyWeixin.CommonAPIs;
using EasyWeixin.Data.Repositories;
using EasyWeixin.Web.Filters;
using EasyWeixin.Web.Models;
using System;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace EasyWeixin.Web.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class PersonalCenterController : Controller
    {
        //
        // GET: /PersonalCenter/
        private readonly IUserMembershipRepository UserMembershipRepository;

        private readonly IUserProfileRepository UserProfileRepository;

        public PersonalCenterController(IUserMembershipRepository UserMembershipRepository, IUserProfileRepository UserProfileRepository)
        {
            this.UserMembershipRepository = UserMembershipRepository;
            this.UserProfileRepository = UserProfileRepository;
        }

        public ActionResult EditPersonalInfo()
        {
            ViewData["SuccessMessage"] = "";
            int UserId = WebSecurity.GetUserId(User.Identity.Name);
            var model = UserProfileRepository.Find(Specification<EasyWeixin.Model.UserProfile>.Eval(o => o.UserId == UserId));
            var ViewModel = Mapper.Map<EasyWeixin.Model.UserProfile, PersonalInfoViewModel>(model);
            return View(ViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPersonalInfo(PersonalInfoViewModel model)
        {            
            if (ModelState.IsValid)
            {
                var command = UserProfileRepository.GetByKey(model.ID);
                command.QQ = model.QQ;
                command.Phone = model.Phone;
                command.UserName = model.UserName;
                command.Email = model.Email;
                UserProfileRepository.Update(command);
                UserProfileRepository.Context.Commit();
                ViewData["SuccessMessage"] = "  ** 提交成功";
            }
            return View(model);
        }

        public ActionResult EditPersonalWeiXin()
        {
            ViewData["SuccessMessage"] = "";
            int UserId = WebSecurity.GetUserId(User.Identity.Name);
            var model = UserProfileRepository.Find(Specification<EasyWeixin.Model.UserProfile>.Eval(o => o.UserId == UserId));
            var ViewModel = Mapper.Map<EasyWeixin.Model.UserProfile, PersonalWeiXinViewModel>(model);

            return View(ViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPersonalWeiXin(PersonalWeiXinViewModel model)
        {

            if (ModelState.IsValid)
            {
                //测试appid和appsecret是否合格
                try
                {
                    AccessTokenContainer.TryGetToken(model.AppId, model.AppSecret, true);
                }
                catch (Exception)
                {
                    ViewData["SuccessMessage"] = "**appid或appSecret填写错误";
                    return View(model);
                }
                var command = UserProfileRepository.GetByKey(model.ID);
                command.AppSecret = model.AppSecret;
                command.AppId = model.AppId;
                command.WeixinUser = model.WeixinUser;
                command.Md5Password = model.Md5Password;
                UserProfileRepository.Update(command);
                UserProfileRepository.Context.Commit();
                ViewData["SuccessMessage"] = "  ** 提交成功";

                model.WeixinToken = command.WeixinToken;
                model.UserCode = command.UserCode;
                return View(model);
            }
            return View(model);
        }

        public ActionResult EditPersonalPassword()
        {
            ViewData["SuccessMessage"] = "";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPersonalPassword(PasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                ViewData["SuccessMessage"] = "  ** 提交成功";
            }
            return View(model);
        }

        /// <summary>
        /// 是否已经存在邮箱 更改和注册时的验证不同，更改的时候，拥有userid
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None, NoStore = true)]   //清除缓存
        public JsonResult IsExistEmail(string Email)
        {
            int UserId = WebSecurity.GetUserId(User.Identity.Name);
            bool valid = false;
            if (!UserProfileRepository.Exists(Specification<EasyWeixin.Model.UserProfile>.Eval(e => e.Email == Email && e.UserId != UserId)))
                valid = true;
            return Json(valid, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 是否已经存在该用户名
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None, NoStore = true)]   //清除缓存
        public JsonResult IsExistUserName(string UserName)
        {
            int UserId = WebSecurity.GetUserId(User.Identity.Name);
            bool valid = false;
            if (!UserProfileRepository.Exists(Specification<EasyWeixin.Model.UserProfile>.Eval(e => e.UserName == UserName && e.UserId != UserId)))
                valid = true;
            return Json(valid, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 是否已经存在该AppId
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None, NoStore = true)]   //清除缓存
        public JsonResult IsExistAppId(string AppId)
        {
            int UserId = WebSecurity.GetUserId(User.Identity.Name);
            bool valid = false;
            if (!UserProfileRepository.Exists(Specification<EasyWeixin.Model.UserProfile>.Eval(e => e.AppId == AppId && e.UserId != UserId)))
                valid = true;
            return Json(valid, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 是否已经存在该AppSecret
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None, NoStore = true)]   //清除缓存
        public JsonResult IsExistAppSecret(string AppSecret)
        {
            int UserId = WebSecurity.GetUserId(User.Identity.Name);
            bool valid = false;
            if (!UserProfileRepository.Exists(Specification<EasyWeixin.Model.UserProfile>.Eval(e => e.AppSecret == AppSecret && e.UserId != UserId)))
                valid = true;
            return Json(valid, JsonRequestBehavior.AllowGet);
        }
    }
}