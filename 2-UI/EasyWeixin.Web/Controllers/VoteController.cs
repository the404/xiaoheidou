using Apworks.Specifications;
using AutoMapper;
using EasyWeixin.Data.Repositories;
using EasyWeixin.Model;
using EasyWeixin.Web.Filters;
using EasyWeixin.Web.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace EasyWeixin.Web.Controllers
{
    [InitializeSimpleMembership]
    [Authorize(Roles = "Vote")]
    public class VoteController : Controller
    {
        //
        // GET: /Vote/
        private readonly IResponseImageTextRepository ResponseImageTextRepository;

        private readonly IUserProfileRepository UserProfileRepository;
        private readonly IVoteUserRepository VoteUserRepository;
        private readonly IVoteRepository VoteRepository;

        public VoteController(IVoteRepository VoteRepository,
            IVoteUserRepository VoteUserRepository,
            IResponseImageTextRepository ResponseImageTextRepository,
            IUserProfileRepository UserProfileRepository)
        {
            this.ResponseImageTextRepository = ResponseImageTextRepository;
            this.UserProfileRepository = UserProfileRepository;
            this.VoteUserRepository = VoteUserRepository;
            this.VoteRepository = VoteRepository;
        }

        #region 主页列表

        public ActionResult VoteIndex(int pageid = 1)
        {
            var UserId = WebSecurity.GetUserId(User.Identity.Name);
            var Vote = VoteRepository.FindAll(Specification<Vote>.Eval(o => o.UserId == UserId)).ToList();
            var Pagerlist = VoteRepository.GetListByPages(Vote, pageid, 10);
            return View(Pagerlist);
        }

        #endregion 主页列表

        #region 打开创建页

        public ActionResult VoteCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult VoteCreate(VoteViewModel form)
        {
            var UserId = WebSecurity.GetUserId(User.Identity.Name);
            var user = UserProfileRepository.Find(Specification<EasyWeixin.Model.UserProfile>.Eval(o => o.UserId == UserId));
            form.ResponseImageTextViewModel.ImageTextName = form.VoteTitle;
            form.ResponseImageTextViewModel.ImageTextType = 105;
            form.ResponseImageTextViewModel.UserId = WebSecurity.GetUserId(User.Identity.Name);
            form.ResponseImageTextViewModel.AddTime = DateTime.Now;
            form.UserId = WebSecurity.GetUserId(User.Identity.Name);
            form.AddDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                Vote Vote = Mapper.Map<VoteViewModel, Vote>(form);
                Vote.VoteStyle = "Vote.css";
                Vote.ResponseImageText = Mapper.Map<ResponseImageTextViewModel, ResponseImageText>(form.ResponseImageTextViewModel);
                VoteRepository.Add(Vote);
                VoteRepository.Context.Commit();
                Vote.ResponseImageText.Url = "http://" + Request.Url.Host + "/Activity/VoteIndex?VoteID=" + Vote.ID + "&ImageTextID=" + Vote.ResponseImageText.ID;
                Vote.GetURL = "http://" + Request.Url.Host + "/Activity/VoteIndex?VoteID=" + Vote.ID + "&ImageTextID=" + Vote.ResponseImageText.ID + "&User_ID=" + user.ID;
                VoteRepository.Update(Vote);
                VoteRepository.Context.Commit();
            }
            return Redirect("/Vote/VoteIndex");
        }

        #endregion 打开创建页

        #region 打开编辑页

        public ActionResult VoteEdit(Guid id)
        {
            Vote Vote = VoteRepository.GetByKey(id);
            VoteViewModel form = Mapper.Map<Vote, VoteViewModel>(Vote);
            form.ResponseImageTextViewModel = Mapper.Map<ResponseImageText, ResponseImageTextViewModel>(Vote.ResponseImageText);
            return View(form);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult VoteEdit(VoteViewModel form)
        {
            if (ModelState.IsValid)
            {
                Vote Vote = VoteRepository.GetByKey(form.ID);
                Vote.VoteTitle = form.VoteTitle;
                Vote.StartDate = form.StartDate;
                Vote.EndDate = form.EndDate;
                Vote.VoteDesc = form.VoteDesc;
                Vote.VoteAnswer = form.VoteAnswer;
                Vote.VoteType = form.VoteType;
                Vote.ResponseImageText.ImageTextName = form.VoteTitle;
                Vote.ResponseImageText.Content = form.ResponseImageTextViewModel.Content;
                VoteRepository.Update(Vote);
                VoteRepository.Context.Commit();
            }
            return Redirect("/Vote/VoteIndex");
        }

        #endregion 打开编辑页

        #region 删除

        public ActionResult VoteDelete(Guid id)
        {
            var Vote = VoteRepository.GetByKey(id);
            if (Vote.VoteUsers.ToList().Count < 500)
            {
                foreach (var item in Vote.VoteUsers.ToList())
                {
                    VoteUserRepository.Remove(item);
                    VoteUserRepository.Context.Commit();
                }
                VoteRepository.Remove(Vote);
                VoteRepository.Context.Commit();
            }
            return RedirectToAction("VoteIndex");
        }

        #endregion 删除

        #region 投票用户列表

        public ActionResult VoteUserList(Guid VoteID, int pageid = 1, DateTime? dt1 = null, DateTime? dt2 = null)
        {
            if (dt1 != null && dt2 != null && dt1 <= dt2)
            {
                int UserId = WebSecurity.GetUserId(User.Identity.Name);
                Vote Vote = VoteRepository.GetByKey(VoteID);
                dt2 = DateTime.Parse(dt2.ToString()).AddDays(1);
                var list = VoteUserRepository.FindAll(Specification<VoteUser>.Eval(o => o.UserId == UserId && o.VoteID == Vote.VoteID && o.AddDate > dt1 && o.AddDate < dt2)).ToList();

                var Pagerlist = VoteUserRepository.GetListByPages(list, pageid, 10);
                return View(Pagerlist);
            }
            else
            {
                int UserId = WebSecurity.GetUserId(User.Identity.Name);
                Vote Vote = VoteRepository.GetByKey(VoteID);
                var EndDate = Vote.EndDate.AddDays(1);
                var list = VoteUserRepository.FindAll(Specification<VoteUser>.Eval(o => o.UserId == UserId && o.VoteID == Vote.VoteID && o.AddDate > Vote.StartDate && o.AddDate < EndDate)).ToList();
                var Pagerlist = VoteUserRepository.GetListByPages(list, pageid, 10);
                return View(Pagerlist);
            }
        }

        #endregion 投票用户列表
    }
}