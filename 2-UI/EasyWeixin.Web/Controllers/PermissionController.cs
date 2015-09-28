using AutoMapper;
using EasyWeixin.Data.Repositories;
using EasyWeixin.Data.Validator;
using EasyWeixin.Model;
using EasyWeixin.Web.Filters;
using EasyWeixin.Web.Framework.FluentValidate;
using EasyWeixin.Web.Models;
using FluentValidation;
using System;
using System.Linq;
using System.Web.Mvc;

namespace EasyWeixin.Web.Controllers
{
    [InitializeSimpleMembership]
    [Authorize(Roles = "Permission")]
    public class PermissionController : Controller
    {
        //
        // GET: /Permission/
        private readonly IPermissionRepository PermissionRepository;

        public PermissionController(IPermissionRepository PermissionRepository)
        {
            this.PermissionRepository = PermissionRepository;
        }

        [EasyWeixinAuthorize("/Permission/Index")]
        public ActionResult Index(int PageID = 1)
        {
            var PagerList = PermissionRepository.GetListByPages(PermissionRepository.FindAll().ToList(), PageID, 15);
            return View(PagerList);
        }

        //
        // GET: /Permission/Create
        [EasyWeixinAuthorize("/Permission/Create")]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Permission/Create

        [HttpPost]
        [EasyWeixinAuthorize("/Permission/Create")]
        public ActionResult Create(Permission permission)
        {
            // 以下是验证
            IValidator validator = new PermissionValidator();
            var results = validator.Validate(permission);
            var validationSucceeded = results.IsValid;
            var failures = results.Errors;
            ModelState.AddModelFluentErrors(failures);
            if (ModelState.IsValid)
            {
                PermissionRepository.Add(permission);
                PermissionRepository.Context.Commit();
                return RedirectToAction("Index");
            }
            return View(permission);
        }

        //
        // GET: /Permission/Edit/
        [EasyWeixinAuthorize("/Permission/Edit")]
        public ActionResult Edit(Guid id)
        {
            Permission permission = PermissionRepository.GetByKey(id);

            PermissionViewModel PermissionViewModel = Mapper.Map<Permission, PermissionViewModel>(permission);
            if (permission == null)
            {
                return HttpNotFound();
            }
            return View(PermissionViewModel);
        }

        //
        // POST: /Permission/Edit/5
        [HttpPost]
        public ActionResult Edit(PermissionViewModel PermissionViewModel)
        {
            // 以下是验证
            IValidator validator = new PermissionValidator();
            Permission permission = Mapper.Map<PermissionViewModel, Permission>(PermissionViewModel);
            var results = validator.Validate(permission);
            var validationSucceeded = results.IsValid;
            var failures = results.Errors;
            ModelState.AddModelFluentErrors(failures);
            if (ModelState.IsValid)
            {
                PermissionRepository.Update(permission);
                PermissionRepository.Context.Commit();
                return RedirectToAction("Index");
            }
            return View(permission);
        }

        //
        // GET: /Permission/Delete/5
        [EasyWeixinAuthorize("/Permission/Delete")]
        public ActionResult Delete(Guid id)
        {
            Permission permission = PermissionRepository.GetByKey(id);
            PermissionRepository.Remove(permission);
            PermissionRepository.Context.Commit();
            return RedirectToAction("Index");
        }
    }
}