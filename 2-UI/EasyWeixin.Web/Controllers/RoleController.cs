using Apworks.Specifications;
using EasyWeixin.Data.Repositories;
using EasyWeixin.Model;
using EasyWeixin.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace EasyWeixin.Web.Controllers
{
    // GET: /Role/
    [InitializeSimpleMembership]
    [Authorize(Roles = "Role")]
    public class RoleController : Controller
    {
        // private ApowApowrksDbContext db = new ApowApowrksDbContext();

        //
        // GET: /Role/
        private readonly IRoleRepository RoleRepository;

        private readonly IPermissionRepository PermissionRepository;
        private readonly IPermissionsInRolesRepository PermissionsInRolesRepository;

        public RoleController(IRoleRepository RoleRepository, IPermissionRepository PermissionRepository, IPermissionsInRolesRepository PermissionsInRolesRepository)
        {
            this.RoleRepository = RoleRepository;
            this.PermissionRepository = PermissionRepository;
            this.PermissionsInRolesRepository = PermissionsInRolesRepository;
        }

        #region 角色列表页

        [EasyWeixinAuthorize("/Role/Index")]
        public ActionResult Index()
        {
            return View(RoleRepository.FindAll().ToList());
        }

        #endregion 角色列表页

        #region 获取单个角色信息内容

        //
        // GET: /Role/Details/5
        public ActionResult Details(Guid id)
        {
            Role role = RoleRepository.GetByKey(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        #endregion 获取单个角色信息内容

        #region 打开创建角色页面

        //
        // GET: /Role/Create
        [EasyWeixinAuthorize("/Role/Create")]
        public ActionResult Create()
        {
            var Plist = PermissionRepository.FindAll().ToList();
            ViewData["PermissionSelectList"] = Plist;
            return View();
        }

        #endregion 打开创建角色页面

        #region 创建角色

        //
        // POST: /Role/Create
        [HttpPost]
        [EasyWeixinAuthorize("/Role/Create")]
        public ActionResult Create(Role role, string[] permissions)
        {
            var Plist = PermissionRepository.FindAll().ToList();
            if (ModelState.IsValid)
            {
                if (RoleRepository.Find(Specification<Role>.Eval(e => e.RoleName == role.RoleName)) == null)
                {
                    RoleRepository.Add(role);
                    RoleRepository.Context.Commit();
                    if (permissions != null)
                    {
                        foreach (var permissionId in permissions)
                        {
                            PermissionsInRoles pir = new PermissionsInRoles();
                            pir.RoleId = role.RoleId;
                            pir.PermissionId = int.Parse(permissionId);
                            PermissionsInRolesRepository.Add(pir);
                            PermissionsInRolesRepository.Context.Commit();
                        }
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewData["PermissionSelectList"] = Plist;
                    TempData["ErrorMessage"] = "角色名称已存在";
                }
            }
            ViewData["PermissionSelectList"] = Plist;
            return View(role);
        }

        #endregion 创建角色

        #region 打开角色编辑页面

        //
        // GET: /Role/Edit/5
        [EasyWeixinAuthorize("/Role/Edit")]
        public ActionResult Edit(Guid id)
        {
            var Plist = PermissionRepository.FindAll().ToList();
            ViewData["PermissionSelectList"] = Plist;
            Role role = RoleRepository.GetByKey(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        #endregion 打开角色编辑页面

        #region 提交角色编辑

        //
        // POST: /Role/Edit/5
        [HttpPost]
        [EasyWeixinAuthorize("/Role/Edit")]
        public ActionResult Edit(Role role, string[] permissions)
        {
            if (ModelState.IsValid)
            {
                RoleRepository.Update(role);
                RoleRepository.Context.Commit();
                if (permissions != null)
                {
                    var Sqlpir = PermissionsInRolesRepository.FindAll(Specification<PermissionsInRoles>.Eval(e => e.RoleId == role.RoleId));
                    //数据库PermissionsInRoles中原有权限ID
                    List<int> idlist = new List<int>();
                    foreach (var item in Sqlpir)
                    {
                        idlist.Add(item.PermissionId);
                    }
                    //数据库原有权限和将要提交的权限比较 若是数据库不包含将要提交的权限，那么就将此权限添加
                    foreach (var permissionId in permissions)
                    {
                        if (!idlist.Contains(int.Parse(permissionId)))
                        {
                            PermissionsInRoles pir = new PermissionsInRoles();
                            pir.RoleId = role.RoleId;
                            pir.PermissionId = int.Parse(permissionId);
                            PermissionsInRolesRepository.Add(pir);
                            PermissionsInRolesRepository.Context.Commit();
                        }
                    }
                    //
                    foreach (var item in idlist)
                    {
                        if (!permissions.ToList().Contains(item.ToString()))
                        {
                            PermissionsInRolesRepository.Remove(PermissionsInRolesRepository.Find(Specification<PermissionsInRoles>.Eval(e => e.PermissionId == item && e.RoleId == role.RoleId)));
                            PermissionsInRolesRepository.Context.Commit();
                        }
                    }
                }
                return RedirectToAction("Index");
            }
            var Plist = PermissionRepository.FindAll().ToList();
            ViewData["PermissionSelectList"] = Plist;
            return View(role);
        }

        #endregion 提交角色编辑

        #region 角色删除

        [EasyWeixinAuthorize("/Role/Delete")]
        public ActionResult Delete(Guid id)
        {
            Role role = RoleRepository.GetByKey(id);
            RoleRepository.Remove(role);
            RoleRepository.Context.Commit();
            return RedirectToAction("Index");
        }

        #endregion 角色删除
    }
}