using EasyWeixin.Data;
using EasyWeixin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace EasyWeixin.Web.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class EasyWeixinAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly bool _authorize;

        private bool _isPermissionFail = false;

        public EasyWeixinAuthorizeAttribute()
        {
            if (HttpContext.Current.User != null && HttpContext.Current.User.Identity != null && HttpContext.Current.User.Identity.Name != "")
            {
                _authorize = true;
            }
            else
            {
                _authorize = false;
            }
        }

        public EasyWeixinAuthorizeAttribute(string permission)
        {
            if (HttpContext.Current.User.Identity.Name != "")
            {
                _authorize = PermissionManager.CheckUserHasPermision(HttpContext.Current.User.Identity.Name, permission);
                if (_authorize == false)
                {
                    _isPermissionFail = true;
                }
            }
            else
            {
                _authorize = false;
            }
            //_authorize = true;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return _authorize;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (_isPermissionFail)
            {
                filterContext.HttpContext.Response.Redirect("/Admin/Index");
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            if (!AuthorizeCore(filterContext.HttpContext))
            {
                HandleUnauthorizedRequest(filterContext);
            }
            else
            {
                filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            }
        }
    }

    public class PermissionManager
    {
        public static bool CheckUserHasPermision(string userName, string permissionName)
        {
            List<Role> roleList = new List<Role>();
            List<PermissionsInRoles> permissionsInRolesList = new List<PermissionsInRoles>();
            using (EasyWeixinDbContext db = new EasyWeixinDbContext())
            {
                roleList = db.Roles.AsEnumerable<Role>().ToList<Role>();
            }

            using (EasyWeixinDbContext db = new EasyWeixinDbContext())
            {
                permissionsInRolesList = db.PermissionsInRoles
                                            .Include("Permission").Include("Role")
                                            .AsEnumerable<PermissionsInRoles>().ToList<PermissionsInRoles>();
            }

            string[] currentRoles = Roles.GetRolesForUser(userName);
            foreach (var roleName in currentRoles)
            {
                List<Permission> permissionList = permissionsInRolesList.Where(e => e.Role.RoleName == roleName)
                                                                            .Select(e => e.Permission).ToList<Permission>();

                foreach (var permission in permissionList)
                {
                    if (permission.PermissionName == permissionName)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}