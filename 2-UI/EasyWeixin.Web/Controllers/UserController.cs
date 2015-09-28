using Apworks.Specifications;
using EasyWeixin.Core.Common;
using EasyWeixin.Data.Repositories;
using EasyWeixin.Model;
using EasyWeixin.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;

namespace EasyWeixin.Web.Controllers
{
    [InitializeSimpleMembership]
    [Authorize(Roles = "User")]
    public class UserController : Controller
    {
        //  private ApowApowrksDbContext db = new ApowApowrksDbContext();
        //
        // GET: /User/
        private readonly IRoleRepository RoleRepository;

        private readonly IPermissionRepository PermissionRepository;
        private readonly IPermissionsInRolesRepository PermissionsInRolesRepository;
        private readonly IUserMembershipRepository UserMembershipRepository;
        private readonly IUserProfileRepository UserProfileRepository;

        public UserController(IRoleRepository RoleRepository, IPermissionRepository PermissionRepository, IPermissionsInRolesRepository PermissionsInRolesRepository, IUserMembershipRepository UserMembershipRepository, IUserProfileRepository UserProfileRepository)
        {
            this.RoleRepository = RoleRepository;
            this.PermissionRepository = PermissionRepository;
            this.PermissionsInRolesRepository = PermissionsInRolesRepository;
            this.UserMembershipRepository = UserMembershipRepository;
            this.UserProfileRepository = UserProfileRepository;
        }

        [EasyWeixinAuthorize("/User/Index")]
        public ActionResult Index()
        {
            List<EasyWeixin.Web.Models.UserViewModel> userList = new List<EasyWeixin.Web.Models.UserViewModel>();
            var list = UserProfileRepository.FindAll().ToList();
            foreach (var bm in list)
            {
                EasyWeixin.Web.Models.UserViewModel user = new EasyWeixin.Web.Models.UserViewModel();
                user.User_ID = bm.UserId;
                user.User_Name = bm.UserName;
                user.Email = bm.Email;
                user.ID = bm.ID;
                var roleNameString = "";
                var UserMembership = UserMembershipRepository.Find(Specification<UserMembership>.Eval(e => e.UserId == bm.UserId));
                if (UserMembership != null && UserMembership.Roles != null)
                {
                    var Roles = UserMembership.Roles.ToList();
                    for (int i = 0; i < Roles.Count; i++)
                    {
                        if (i == 0)
                        {
                            roleNameString = Roles[i].RoleChineseName.ToString();
                        }
                        else
                        {
                            roleNameString = roleNameString + "," + Roles[i].RoleName.ToString();
                        }
                    }
                }
                user.Role_Name = roleNameString;
                userList.Add(user);
            }
            ViewData["UserList"] = userList;
            return View(userList);
        }

        [EasyWeixinAuthorize("/User/Create")]
        public ActionResult Create()
        {
            var rolelist = RoleRepository.FindAll().ToList();
            ViewData["SelectList"] = rolelist;
            return View();
        }

        //
        // POST: /User/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection, string[] roles)
        {
            if (ModelState.IsValid)
            {
                string userName = collection.Get("User_Name");
                string password = collection.Get("Password");
                string email = collection.Get("Email");
                if (UserProfileRepository.Find(Specification<UserProfile>.Eval(e => e.UserName == userName)) == null)
                {
                    WebSecurity.CreateUserAndAccount(userName, password);
                    if (roles != null)
                    {
                        foreach (var roleName in roles)
                        {
                            Roles.AddUserToRole(userName, roleName);
                        }
                    }
                    //以下是.......
                    UserProfile user = UserProfileRepository.Find(Specification<UserProfile>.Eval((e => e.UserName == userName)));
                    user.Email = email;

                    //by tianxiu 2014-3-20
                    if (user.WeixinToken == null)
                    {
                        //获取四位随机数
                        var fourRandomString = Function.GetRandomString(4);
                        user.UserCode = user.ID.ToString().Replace("-", "");
                        user.Alt = fourRandomString;
                        user.WeixinToken = Function.MD5(user.ID.ToString().Replace("-", "") + fourRandomString);
                        user.DateCreated = DateTime.Now;
                    }

                    UserProfileRepository.Update(user);
                    UserProfileRepository.Context.Commit();
                    return RedirectToAction("Index");
                }
                else
                {
                    var rolelist = RoleRepository.FindAll().ToList();
                    ViewData["SelectList"] = rolelist;
                    TempData["ErrorMessage"] = "用户名已存在";
                }
            }

            return View();
        }

        //
        // GET: /User/Edit/5
        [EasyWeixinAuthorize("/User/Edit")]
        public ActionResult Edit(Guid id)
        {
            EasyWeixin.Web.Models.UserViewModel vModel = new EasyWeixin.Web.Models.UserViewModel();
            var roleList = RoleRepository.FindAll();
            ViewData["SelectList"] = roleList;
            var user = UserProfileRepository.GetByKey(id);
            UserMembership UserMembership = UserMembershipRepository.Find(Specification<UserMembership>.Eval(e => e.UserId == user.UserId));
            vModel.User_ID = user.UserId;
            vModel.User_Name = user.UserName;
            vModel.Email = user.Email;
            vModel.Roles = UserMembership.Roles;
            return View(vModel);
        }

        //
        // POST: /User/Edit/5

        [HttpPost]
        public ActionResult Edit(FormCollection collection)
        {
            int UserID = int.Parse(collection.Get("User_ID"));
            string userName = collection.Get("User_Name");
            string email = collection.Get("Email");
            Guid ID = Guid.Parse(collection.Get("ID"));
            UserProfile user = UserProfileRepository.GetByKey(ID);
            user.UserName = userName;
            user.Email = email;
            UserProfileRepository.Update(user);
            UserProfileRepository.Context.Commit();
            return RedirectToAction("Index");
        }

        [EasyWeixinAuthorize("/User/EditRole")]
        public ActionResult EditRole(Guid id)
        {
            EasyWeixin.Web.Models.UserViewModel vModel = new EasyWeixin.Web.Models.UserViewModel();
            var roleList = RoleRepository.FindAll();
            ViewData["SelectList"] = roleList;
            var user = UserProfileRepository.GetByKey(id);
            UserMembership UserMembership = UserMembershipRepository.Find(Specification<UserMembership>.Eval(e => e.UserId == user.UserId));
            vModel.User_ID = user.UserId;
            vModel.User_Name = user.UserName;
            vModel.Email = user.Email;
            vModel.Roles = UserMembership.Roles;
            return View(vModel);
        }

        [HttpPost]
        public ActionResult EditRole(FormCollection collection, string[] roles)
        {
            int UserID = int.Parse(collection.Get("User_ID"));
            Guid ID = Guid.Parse(collection.Get("ID"));
            UserProfile user = UserProfileRepository.GetByKey(ID);
            UserMembership UserMembership = UserMembershipRepository.Find(Specification<UserMembership>.Eval(e => e.UserId == UserID));
            //数据库拥有的权限
            var sqlRoles = UserMembership.Roles;
            List<string> sqlRoleNameList = new List<string>();
            foreach (var item in sqlRoles)
            {
                sqlRoleNameList.Add(item.RoleName);
            }
            foreach (var roleName in roles)
            {
                if (!sqlRoleNameList.Contains(roleName))
                {
                    Roles.AddUserToRole(user.UserName, roleName);
                }
            }
            //
            foreach (var item in sqlRoleNameList)
            {
                if (!roles.ToList().Contains(item.ToString()))
                {
                    Roles.RemoveUserFromRole(user.UserName, item);
                }
            }
            return RedirectToAction("Index");
        }

        //
        // GET: /User/Delete/5
        [EasyWeixinAuthorize("/User/Delete")]
        public ActionResult Delete(Guid id)
        {
            var user = UserProfileRepository.GetByKey(id);
            var UserMembership = UserMembershipRepository.Find(Specification<UserMembership>.Eval(e => e.UserId == user.UserId));
            UserMembershipRepository.Remove(UserMembership);
            Membership.DeleteUser(user.UserName);
            UserMembershipRepository.Context.Commit();
            return RedirectToAction("Index");
        }

        [EasyWeixinAuthorize("/User/ResetPassword")]
        public ActionResult ResetPassword(Guid id)
        {
            UserProfile user = UserProfileRepository.GetByKey(id);
            EasyWeixin.Web.Models.UserViewModel vModel = new EasyWeixin.Web.Models.UserViewModel();
            vModel.User_ID = user.UserId;
            vModel.User_Name = user.UserName;
            vModel.Email = user.Email;
            return View(vModel);
        }

        [HttpPost]
        public ActionResult ResetPassword(FormCollection collection)
        {
            int User_ID = int.Parse(collection.Get("User_ID"));
            Guid ID = Guid.Parse(collection.Get("ID"));
            string newPassword = collection.Get("NewPassword");
            UserProfile user = UserProfileRepository.GetByKey(ID);
            UserMembership membership = UserMembershipRepository.Find(Specification<UserMembership>.Eval(e => e.UserId == User_ID));
            string passwordResetToken = WebSecurity.GeneratePasswordResetToken(user.UserName, 1440);
            membership.PasswordResetToken = passwordResetToken;
            WebSecurity.ResetPassword(passwordResetToken, newPassword);
            TempData["SuccessMessage"] = "密码修改成功！";
            return View();
        }
    }
}