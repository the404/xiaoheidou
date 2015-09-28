using EasyWeixin.Data.Repositories;
using System;
using System.Web.Mvc;

namespace EasyWeixin.Web.Controllers
{
    public class ActivityVoteController : Controller
    {
        //微投票
        // GET: /ActivityVote/
        private readonly IVoteUserRepository VoteUserRepository;

        private readonly IVoteRepository VoteRepository;
        private readonly IResponseImageTextRepository ResponseImageTextRepository;
        private readonly IUserProfileRepository UserProfileRepository;

        public ActivityVoteController(
            IVoteUserRepository VoteUserRepository,
            IVoteRepository VoteRepository,
            IResponseImageTextRepository ResponseImageTextRepository,
            IUserProfileRepository UserProfileRepository)
        {
            this.ResponseImageTextRepository = ResponseImageTextRepository;
            this.UserProfileRepository = UserProfileRepository;
            this.VoteUserRepository = VoteUserRepository;
            this.VoteRepository = VoteRepository;
        }

        public ActionResult VoteIndex(Guid VoteID, Guid ImageTextID, Guid User_ID, string UserWexinID = "")
        {
            var Vote = VoteRepository.GetByKey(VoteID);
            return View(Vote);
        }

        [HttpPost]
        public JsonResult AddVote()
        {
            return Json(new { });
        }
    }
}