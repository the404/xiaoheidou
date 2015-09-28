using EasyWeixin.Data.Repositories;
using System.Web.Mvc;

namespace EasyWeixin.Web.Controllers
{
    public class GuessController : Controller
    {
        //
        // GET: /Guess/
        private readonly IResponseImageTextRepository ResponseImageTextRepository;

        private readonly IUserProfileRepository UserProfileRepository;
        private readonly IGuessUserRepository GuessUserRepository;
        private readonly IGuessRepository GuessRepository;

        public GuessController(IGuessRepository GuessRepository, IGuessUserRepository GuessUserRepository, IResponseImageTextRepository ResponseImageTextRepository, IUserProfileRepository UserProfileRepository)
        {
            this.ResponseImageTextRepository = ResponseImageTextRepository;
            this.UserProfileRepository = UserProfileRepository;
            this.GuessUserRepository = GuessUserRepository;
            this.GuessRepository = GuessRepository;
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}