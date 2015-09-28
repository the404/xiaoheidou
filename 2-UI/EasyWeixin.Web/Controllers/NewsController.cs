using Apworks.Specifications;
using AutoMapper;
using EasyWeixin.Core.Common;
using EasyWeixin.Data.Repositories;
using EasyWeixin.Model;
using EasyWeixin.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace EasyWeixin.Web.Controllers
{
    public class NewsController : Controller
    {
        //
        // GET: /News/
        private readonly IResponseImageRepository ResponseImageRepository;

        private readonly IResponseImageTextRepository ResponseImageTextRepository;
        private readonly IGuessUserRepository GuessUserRepository;
        private readonly IGuessRepository GuessRepository;
        private readonly IUserProfileRepository UserProfileRepository;

        public NewsController(IGuessRepository GuessRepository, IUserProfileRepository UserProfileRepository, IResponseImageRepository ResponseImageRepository, IResponseImageTextRepository ResponseImageTextRepository, IGuessUserRepository GuessUserRepository)
        {
            this.ResponseImageRepository = ResponseImageRepository;
            this.ResponseImageTextRepository = ResponseImageTextRepository;
            this.GuessUserRepository = GuessUserRepository;
            this.UserProfileRepository = UserProfileRepository;
            this.GuessRepository = GuessRepository;
        }

        public ActionResult Index(Guid ImageTextID, Guid User_ID, string UserWexinID = "")
        {
            var ResponseImageText = ResponseImageTextRepository.GetByKey(ImageTextID);
            return View(ResponseImageText);
        }

        //public ActionResult Index(Guid ImageTextID, Guid User_ID, string openId = "", string signature = "", string timestamp = "", string nickname = "")
        //{
        //    var ResponseImageText = ResponseImageTextRepository.GetByKey(ImageTextID);
        //    return View(ResponseImageText);
        //}
        public ActionResult GuessNews(Guid ImageTextID, Guid User_ID, string UserWexinID = "")
        {
            if (Session["GuessNewsModel"] == null)
            {
                GuessNewsViewModel ViewModel = new GuessNewsViewModel();
                Session["Answer"] = getRandom();
                ViewModel.AddDate = DateTime.Now;
                var user = UserProfileRepository.GetByKey(User_ID);
                ViewModel.User_ID = User_ID;
                ViewModel.UserId = user.UserId;
                ViewModel.isGuess = 0;
                ViewModel.IP = Request.UserHostAddress;
                ViewModel.GuessUserWexinID = UserWexinID;
                ViewModel.ImageTextID = ImageTextID;
                ViewModel.CompanyName = user.CompanyName;

                var imagetext = ResponseImageTextRepository.GetByKey(ImageTextID);
                var guess = imagetext.Guesses.ElementAt(0);
                ViewModel.GuessID = guess.GuessID;
                ViewModel.GuessDesc = guess.GuessDesc;
                ViewModel.GuessStyle = guess.GuessStyle;
                ViewModel.GuessTitle = guess.GuessTitle;
                Session["GuessNewsModel"] = ViewModel;
                Session.Timeout = 1440;
                return View(ViewModel);
            }
            else
            {
                GuessNewsViewModel model = Session["GuessNewsModel"] as GuessNewsViewModel;
                GuessNewsViewModel ViewModel1 = model;
                ViewModel1.Answer = 0;
                return View(ViewModel1);
            }
        }

        [HttpPost]
        public JsonResult GuessNews(GuessNewsViewModel ViewModel)
        {
            if (Session["GuessNewsModel"] != null)
            {
                GuessNewsViewModel model = Session["GuessNewsModel"] as GuessNewsViewModel;

                model.GuessProcess = ViewModel.GuessProcess;
                model.GuessTimes = ViewModel.GuessTimes;
                Session["GuessNewsModel"] = model;
                Session.Timeout = 1440;
                return Json(model);
            }
            return Json(ViewModel);
        }

        public ActionResult GuessNewsUserInfo(GuessNewsViewModel model)
        {
            if (Session["GuessNewsModel"] != null)
            {
                model = Session["GuessNewsModel"] as GuessNewsViewModel;
                if (model.isGuess == 0)
                {
                    return Redirect("/News/GuessNews?User_ID=" + model.User_ID + "&ImageTextID=" + model.ImageTextID + "&UserWexinID=" + model.GuessUserWexinID);
                }
            }
            return View(model);
        }

        public JsonResult GuessNewsCompare(GuessNewsViewModel model)
        {
            if (Session["GuessNewsModel"] != null)
            {
                var CurrentAnswer = model.CurrentAnswer;
                GuessNewsViewModel GuessNewsModel = Session["GuessNewsModel"] as GuessNewsViewModel;
                var imagetext = ResponseImageTextRepository.GetByKey(GuessNewsModel.ImageTextID);
                var guess = imagetext.Guesses.ElementAt(0);
                if (System.DateTime.Now < guess.StartDate)
                {
                    GuessNewsModel.isGuess = 2;
                    GuessNewsModel.Message = "活动未开始";
                    Session["GuessNewsModel"] = GuessNewsModel;
                    return Json(GuessNewsModel);
                }
                if (System.DateTime.Now > guess.EndDate.AddDays(1))
                {
                    GuessNewsModel.isGuess = 3;
                    GuessNewsModel.Message = "活动已结束";
                    Session["GuessNewsModel"] = GuessNewsModel;
                    return Json(GuessNewsModel);
                }
                if (GuessNewsModel.isGuess == 1)
                {
                    GuessNewsModel.Message = "您已经猜中";
                    Session["GuessNewsModel"] = GuessNewsModel;
                    return Json(GuessNewsModel);
                }
                var Answer = int.Parse(Session["Answer"].ToString());
                GuessNewsModel.GuessTimes += 1;
                GuessNewsModel.CurrentAnswer = CurrentAnswer;
                if (CurrentAnswer > 999 || CurrentAnswer < 0)
                {
                    GuessNewsModel.Message = "超出范围!";
                }
                if (CurrentAnswer > 0 && CurrentAnswer < Answer)
                {
                    GuessNewsModel.GuessProcess += "|" + CurrentAnswer.ToString();
                    GuessNewsModel.Message = "数字偏小!";
                }
                if (CurrentAnswer > 0 && CurrentAnswer > Answer && CurrentAnswer <= 999)
                {
                    GuessNewsModel.GuessProcess += "|" + CurrentAnswer.ToString();
                    GuessNewsModel.Message = "数字偏大!";
                }
                if (CurrentAnswer > 0 && CurrentAnswer == Answer)
                {
                    GuessNewsModel.GuessProcess += "|" + CurrentAnswer.ToString();
                    GuessNewsModel.Message = "猜中了!";
                    GuessNewsModel.isGuess = 1;
                    //GuessNewsModel.Answer = getRandom();
                }
                Session["GuessNewsModel"] = GuessNewsModel;
                return Json(GuessNewsModel);
            }
            else
            {
                GuessNewsViewModel GuessNewsModel = new GuessNewsViewModel();
                GuessNewsModel.Message = "无数据";
                return Json(GuessNewsModel);
            }
        }

        [HttpPost]
        public ActionResult GuessNewsUserInfoAdd(GuessNewsViewModel model)
        {
            if (Session["GuessNewsModel"] != null)
            {
                GuessNewsViewModel GuessNewsModel = Session["GuessNewsModel"] as GuessNewsViewModel;

                if (GuessNewsModel.isGuess == 1)
                {
                    if (ModelState.IsValid)
                    {
                        GuessNewsModel.Sex = model.Sex;
                        GuessNewsModel.GuessUserName = model.GuessUserName;
                        GuessNewsModel.GuessUserPhone = model.GuessUserPhone;
                        GuessNewsModel.Answer = int.Parse(Session["Answer"].ToString());
                        //GuessNewsModel.UserId = 1;
                        GuessUser gu = Mapper.Map<GuessNewsViewModel, GuessUser>(GuessNewsModel);
                        GuessUserRepository.Add(gu);
                        GuessUserRepository.Context.Commit();

                        GuessNewsViewModel ViewModel = new GuessNewsViewModel();
                        Session["Answer"] = getRandom();
                        ViewModel.AddDate = DateTime.Now;
                        ViewModel.User_ID = GuessNewsModel.User_ID;
                        ViewModel.UserId = GuessNewsModel.UserId;
                        ViewModel.GuessUserWexinID = GuessNewsModel.GuessUserWexinID;
                        ViewModel.CompanyName = GuessNewsModel.CompanyName;

                        ViewModel.isGuess = 0;
                        ViewModel.ImageTextID = GuessNewsModel.ImageTextID;
                        ViewModel.IP = Request.UserHostAddress;
                        ViewModel.GuessID = GuessNewsModel.GuessID;
                        ViewModel.GuessDesc = GuessNewsModel.GuessDesc;
                        ViewModel.GuessStyle = GuessNewsModel.GuessStyle;
                        ViewModel.GuessTitle = GuessNewsModel.GuessTitle;
                        Session["GuessNewsModel"] = ViewModel;
                        Session.Timeout = 1440;
                        return Redirect("/News/LeaderBoard?User_ID=" + GuessNewsModel.User_ID + "&ImageTextID=" + GuessNewsModel.ImageTextID + "&UserWexinID=" + GuessNewsModel.GuessUserWexinID);
                    }
                }
                else
                {
                    GuessNewsViewModel ViewModel = new GuessNewsViewModel();
                    Session["Answer"] = getRandom();
                    ViewModel.AddDate = DateTime.Now;
                    ViewModel.User_ID = GuessNewsModel.User_ID;
                    ViewModel.UserId = GuessNewsModel.UserId;
                    ViewModel.GuessUserWexinID = GuessNewsModel.GuessUserWexinID;
                    ViewModel.ImageTextID = GuessNewsModel.ImageTextID;
                    ViewModel.IP = Request.UserHostAddress;
                    ViewModel.CompanyName = GuessNewsModel.CompanyName;
                    ViewModel.isGuess = 0;
                    ViewModel.GuessID = GuessNewsModel.GuessID;
                    ViewModel.GuessDesc = GuessNewsModel.GuessDesc;
                    ViewModel.GuessStyle = GuessNewsModel.GuessStyle;
                    ViewModel.GuessTitle = GuessNewsModel.GuessTitle;
                    Session["GuessNewsModel"] = ViewModel;
                    Session.Timeout = 1440;
                    return Redirect("/News/GuessNews?User_ID=" + ViewModel.User_ID + "&ImageTextID=" + ViewModel.ImageTextID + "&UserWexinID=" + ViewModel.GuessUserWexinID);
                }
            }
            return RedirectToAction("GuessNewsUserInfo", model);
        }

        public ActionResult LeaderBoard(Guid ImageTextID, Guid User_ID, string UserWexinID = "")
        {
            var imagetext = ResponseImageTextRepository.GetByKey(ImageTextID);
            var guess = imagetext.Guesses.ElementAt(0);
            var UserProfile = UserProfileRepository.GetByKey(User_ID);
            var EndDate = guess.EndDate.AddDays(1);
            var Leader = GuessUserRepository.FindAll(Specification<GuessUser>.Eval(o => o.UserId == UserProfile.UserId && o.GuessID == guess.GuessID && o.AddDate > guess.StartDate && o.AddDate < EndDate)).OrderBy(o => o.GuessTimes).Take(10).ToList();
            return View(Leader);
        }

        public JsonResult LeaderBoardGetDatas(Guid ImageTextID, Guid User_ID, string UserWexinID = "", int pageid = 1)
        {
            var imagetext = ResponseImageTextRepository.GetByKey(ImageTextID);
            var guess = imagetext.Guesses.ElementAt(0);
            var UserProfile = UserProfileRepository.GetByKey(User_ID);
            var EndDate = guess.EndDate.AddDays(1);
            var Leader = GuessUserRepository.FindAll(Specification<GuessUser>.Eval(o => o.UserId == UserProfile.UserId && o.GuessID == guess.GuessID && o.AddDate > guess.StartDate && o.AddDate < EndDate)).OrderBy(o => o.GuessTimes).ToList();

            var Leaderlist = GuessUserRepository.GetListByPages(Leader, pageid, 10);
            //防止序列化类型为“System.Data.Entity.DynamicProxies.Button_0214FECA2FF67FF722EBBE9B2369AAFDA8089ED129A153DEBA132DB9BBE1F842”的对象时检测到循环引用的发生
            List<GuessNewsViewModel> gvlist = new List<GuessNewsViewModel>();
            foreach (var item in Leaderlist)
            {
                //GuessNewsViewModel gv = Mapper.Map<GuessUser, GuessNewsViewModel>(item);
                GuessNewsViewModel gv = new GuessNewsViewModel();
                gv.GuessTimes = item.GuessTimes;
                gv.GuessUserName = item.GuessUserName;
                gvlist.Add(gv);
            }
            return Json(gvlist);
        }

        public int getRandom()
        {
            Random randow = new Random();
            return randow.Next(1, 999);
        }

        public ActionResult text()
        {
            var ResponseImageText = ResponseImageTextRepository.GetByKey(Guid.Parse("a1d76af3-2fef-4894-86c9-d25f3b544def"));
            var Content = ResponseImageText.Content;
            var url = HtmlStringHelper.GetHtmlImageUrlList(Content).Length > 0 ? HtmlStringHelper.GetHtmlImageUrlList(Content)[0] : "";
            ViewData["url"] = url;
            return View();
        }
    }
}