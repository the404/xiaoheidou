using Apworks.Specifications;
using AutoMapper;
using EasyWeixin.CommonAPIs;
using EasyWeixin.Data.Repositories;
using EasyWeixin.Entities.JsonResult;
using EasyWeixin.Model;
using EasyWeixin.Web.Helpers;
using EasyWeixin.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using UserProfile = EasyWeixin.Model.UserProfile;

namespace EasyWeixin.Web.Controllers
{
    public class ActivityScratchController : Controller
    {
        #region Ctor and Fields

        /// <summary>
        /// 现在时间-10分钟(10分钟前)
        /// </summary>
        private static readonly DateTime ExpirationTime = DateTime.Now.AddMinutes(-10);

        private readonly object _object = new object();
        private readonly IScratchRepository _scratchRepository;
        private readonly IScratchUserRepository _scratchUserRepository;
        private readonly IWeixinUserInActivityRepository _weixinUserInActivityRepository;
        private readonly IWeixinUserInUsersRepository _weixinUserInUsersRepository;
        private readonly IWeixinUserRepository _weixinUserRepository;

        public ActivityScratchController(
            IScratchItemRepository scratchItemRepository,
            IScratchRepository scratchRepository,
            IScratchUserRepository scratchUserRepository,
            IResponseImageTextRepository responseImageTextRepository,
            IUserProfileRepository userProfileRepository,
            IWeixinUserRepository weixinUserRepository,
            IWeixinUserInUsersRepository weixinUserInUsersRepository,
            IWeixinUserInActivityRepository weixinUserInActivityRepository)
        {
            _weixinUserRepository = weixinUserRepository;
            _weixinUserInActivityRepository = weixinUserInActivityRepository;
            _weixinUserInUsersRepository = weixinUserInUsersRepository;
            _scratchUserRepository = scratchUserRepository;
            _scratchRepository = scratchRepository;
        }

        #endregion Ctor and Fields

        #region 首页
        public ActionResult Index(Guid? scratchId, string code = "")
        {
            #region 判断传递参数是否符合

            if (scratchId == null) return View();
            var scratch = _scratchRepository.GetByKey(scratchId);
            if (scratch == null) return View();

            #endregion 判断传递参数是否符合

            #region 活动尚未开始或接受的时候应该都有一个404页面的

            if (!CheckValid(new Guid(Convert.ToString(scratchId))))
            {
            }

            #endregion 活动尚未开始或接受的时候应该都有一个404页面的

            var scratchViewModel = Mapper.Map<Scratch, ScratchViewModel>(scratch);
            ViewData["ScratchId"] = scratchId;
            ViewData["ActPic"] = scratch.PicUrl;
            var userProfile = scratch.UserProfile;
            // ReSharper disable once PossibleNullReferenceException
            var shareUrl = Request.Url.AbsoluteUri;
            var redirectUrl = string.Format(CommonApi.Snsapi_baseUrl, userProfile.AppId, shareUrl);//用于微信第三方登陆的地址

            #region 绑定微信jssdk

            var result = CommonApi.GetWxConfigResult(scratch.UserProfile.AppId, scratch.UserProfile.AppSecret, shareUrl);
            ViewData["appid"] = result.appid;
            ViewData["timestamp"] = result.timestamp;
            ViewData["noncestr"] = result.noncestr;
            ViewData["signature"] = result.signature;

            #endregion 绑定微信jssdk

            #region 现在缓存这个是为了提高效率

            if (Session["OAuthWeixinUser"] != null)
            {
                ViewData["HeadUrl"] = ((OAuthWeixinUserInfoResult)Session["OAuthWeixinUser"]).headimgurl;
                ViewData["NickName"] = ((OAuthWeixinUserInfoResult)Session["OAuthWeixinUser"]).nickname;
                if (Session["WeixinUserId"] == null)
                {
                    var singleOrDefault = _weixinUserRepository.FindAll().SingleOrDefault(
                        s => s.Openid == ((OAuthWeixinUserInfoResult)Session["OAuthWeixinUser"]).openid);
                    if (singleOrDefault != null)
                        Session["WeixinUserId"] = singleOrDefault.WeixinUserId;
                }
                var userInActivity = _weixinUserInActivityRepository.FindAll().SingleOrDefault(
                    s => s.WeixinUserId == (int)Session["WeixinUserId"] &&
                         s.ActType == ActType.Scratch && s.ActId == scratch.ScratchID);
                if (userInActivity != null)
                {
                    ViewData["wiaId"] = userInActivity.ID;
                    ViewData["Count"] = userInActivity.Count;
                }
                List<ScratchUserViewModel> list = scratchViewModel.ScratchUsers.Where(s =>
                        s.WeixinUserId == (int)Session["WeixinUserId"] &&
                        !string.IsNullOrWhiteSpace(s.Phone) &&
                        s.ScratchID == scratch.ScratchID)
                        .OrderByDescending(s => s.IsAward).ToList();
                scratchViewModel.MyAwards = list;
                return View(scratchViewModel);
            }

            #endregion 现在缓存这个是为了提高效率

            if (!string.IsNullOrEmpty(code))
            {
                #region 使用code来获取OAuthToken

                OAuthAccessTokenResult oAuthAccessToken;
                try
                {
                    oAuthAccessToken = CommonApi.GetOAuthToken(userProfile.AppId, userProfile.AppSecret, code);
                }
                catch
                {
                    return Redirect(redirectUrl);
                }
                var flag = CommonApi.CheckAccessToken(oAuthAccessToken.access_token, oAuthAccessToken.openid);
                if (flag)
                {
                    var oAuthWeixinUser = CommonApi.GetOAuthWeixinUserInfo(oAuthAccessToken.access_token,
                        oAuthAccessToken.openid);
                    Session["OAuthWeixinUser"] = oAuthWeixinUser;
                    ViewData["HeadUrl"] = oAuthWeixinUser.headimgurl;
                    ViewData["NickName"] = oAuthWeixinUser.nickname;
                    var weixinUser = Mapper.Map<OAuthWeixinUserInfoResult, WeixinUser>(oAuthWeixinUser);

                    #region 判断是否存在微信用户信息
                    _weixinUserRepository.AddWeixinUser(oAuthWeixinUser, weixinUser);
                    _weixinUserInUsersRepository.AddWeixinUserInUsers(userProfile, weixinUser);

                    #endregion 判断是否存在微信用户信息

                    #region 判断是否存在活动信息

                    int count;
                    var weixinUserInActivity = new WeixinUserInActivity
                    {
                        WeixinUserId = weixinUser.WeixinUserId,
                        ActType = ActType.Scratch,
                        ActId = scratch.ScratchID,
                        Count = 2,
                        AddDate = DateTime.Now,
                        UpdateDate = DateTime.Now
                    };
                    var userInActivity = _weixinUserInActivityRepository.FindAll().SingleOrDefault(
                            s => s.WeixinUserId == weixinUser.WeixinUserId &&
                                 s.ActType == ActType.Scratch && s.ActId == scratch.ScratchID);
                    if (userInActivity != null)
                    {
                        count = userInActivity.Count;
                        ViewData["wiaId"] = userInActivity.ID;
                    }
                    else
                    {
                        _weixinUserInActivityRepository.Add(weixinUserInActivity);
                        _weixinUserInActivityRepository.Context.Commit();
                        count = weixinUserInActivity.Count;
                        ViewData["wiaId"] = weixinUserInActivity.ID;
                    }

                    #endregion 判断是否存在活动信息

                    List<ScratchUserViewModel> list = scratchViewModel.ScratchUsers.Where(s =>
                        s.WeixinUserId == weixinUser.WeixinUserId &&
                        !string.IsNullOrWhiteSpace(s.Phone) &&
                        s.ScratchID == scratch.ScratchID)
                        .OrderByDescending(s => s.IsAward).ToList();
                    scratchViewModel.MyAwards = list;
                    ViewData["Count"] = count > 0 ? count : 0;
                    Session["WeixinUserId"] = weixinUser.WeixinUserId;
                }
                else
                {
                    ModelState.AddModelError("CheckFail", "AccessToken验证失败,请刷新重试");
                    return View(scratchViewModel);
                }

                #endregion 使用code来获取OAuthToken
            }
            else
            {
                return Redirect(redirectUrl);
            }

            return View(scratchViewModel);
        }
        #endregion 首页

        #region 点击抽奖

        /// <summary>
        /// </summary>
        /// <param name="scratchId">活动ID</param>
        /// <param name="wiaId">UserInActityID</param>
        /// <returns></returns>
        public JsonResult StartScratch(Guid scratchId, Guid wiaId)
        {
            lock (_object)
            {
                var scratch = _scratchRepository.FindAll().FirstOrDefault(s => s.ID == scratchId);
                if (scratch == null) return Json(new JsonError { errorcode = "error", message = "请稍后重试!" });

                #region 活动时间判断

                if (DateTime.Now < scratch.StartDate)
                {
                    return Json(new JsonError { errorcode = "error", message = "活动暂未开始，敬请期待！" }, JsonRequestBehavior.AllowGet);
                }
                if (DateTime.Now > scratch.EndDate.AddDays(1))
                {
                    return Json(new JsonError { errorcode = "error", message = "活动已经结束了!" }, JsonRequestBehavior.AllowGet);
                }

                #endregion 活动时间判断

                #region 判断是否还有抽奖次数

                var weixinUserInActivity =
                    _weixinUserInActivityRepository.FindAll().SingleOrDefault(s => s.ID == wiaId);
                if (weixinUserInActivity != null && weixinUserInActivity.Count > 0)
                {
                    weixinUserInActivity.SumCount++;
                    weixinUserInActivity.UpdateDate = DateTime.Now;
                    weixinUserInActivity.Count--;
                    ViewData["Count"] = weixinUserInActivity.Count;
                    _weixinUserInActivityRepository.Update(weixinUserInActivity);
                    _weixinUserInActivityRepository.Context.Commit();
                }
                else
                {
                    ViewData["Count"] = 0;
                    return Json(new JsonError { errorcode = "error", message = "抽奖机会为0" });
                }

                Session["Count"] = ViewData["Count"];

                #endregion 判断是否还有抽奖次数

                #region 抽奖

                var scratchItemId = GetScratchItemId(scratch);
                Object result = null;
                if (scratchItemId > 0)
                {
                    var scratchItem = scratch.ScratchItems.SingleOrDefault(s => s.ScratchItemID == scratchItemId);
                    if (scratchItem != null)
                    {
                        if (!string.IsNullOrEmpty(scratchItem.ScratchItemAward))
                            result = new { errorcode = "ok", ScratchItemName = scratchItem.ScratchItemAward, IsAward = true };
                        else
                            // ReSharper disable once RedundantAnonymousTypePropertyName
                            result = new { errorcode = "ok", ScratchItemName = scratchItem.ScratchItemName, IsAward = true };
                    }

                    #region 添加中奖用户到数据库中

                    var scratchUser = new ScratchUser
                    {
                        ScratchID = scratch.ScratchID,
                        ScratchItemID = scratchItemId,
                        WeixinUserId = weixinUserInActivity.WeixinUserId,
                        IP = Request.UserHostAddress,
                        AddDate = DateTime.Now,
                    };
                    _scratchUserRepository.Add(scratchUser);
                    _scratchUserRepository.Context.Commit();
                    Session["scratchUserId"] = scratchUser.ID;

                    #endregion 添加中奖用户到数据库中
                }
                else
                {
                    result = new
                    {
                        errorcode = "ok",
                        ScratchItemName = "没有中奖<br/>邀请好友再来一次",
                        IsAward = false
                    };
                }

                #endregion 抽奖

                return Json(result);
            }
        }

        #endregion 点击抽奖

        #region 获取用户Name Phone

        /// <summary>
        /// 用户提交信息到中奖信息中
        /// </summary>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        public JsonResult GetUserInfo(string name, string phone)
        {
            lock (_object)
            {
                if (string.IsNullOrEmpty(name))
                    return Json(new JsonError { errorcode = "error", message = "请输入姓名!" });
                if (!RegexHelpers.IsCNMobileNum(phone))
                    return Json(new JsonError { errorcode = "error", message = "请核对手机格式!" });
                if (_scratchUserRepository.Exists(Specification<ScratchUser>.Eval(s => s.Phone == phone)))
                    return Json(new JsonError { errorcode = "error", message = "该手机号已经参与过活动,<br/>请尝试使用其他手机号!" });
                try
                {
                    var scratchUserId = (Guid)Session["scratchUserId"];
                    var scratchuser = _scratchUserRepository.GetByKey(scratchUserId);
                    if (scratchuser != null)
                    {
                        scratchuser.Name = name;
                        scratchuser.Phone = phone;
                        _scratchUserRepository.Update(scratchuser);
                        _scratchUserRepository.Context.Commit();

                        return Json(new JsonError { errorcode = "ok", message = "" });
                    }
                    if (Request.Url != null) Redirect(Request.Url.AbsolutePath);
                    return Json(new JsonError { errorcode = "reload", message = "找不到中奖用户" });
                }
                catch (Exception)
                {
                    return Json(new JsonError { errorcode = "reload", message = "找不到中奖用户" });
                }
            }
        }

        #endregion 获取用户Name Phone

        #region 用户放弃奖品

        public JsonResult DelUserInfo()
        {
            lock (_object)
            {
                try
                {
                    if (Session["scratchUserId"] == null)
                    {
                        return Json(Json(new JsonError { errorcode = "ok" }));
                    }
                    var scratchUserId = (Guid)Session["scratchUserId"];
                    var scratchuser = _scratchUserRepository.GetByKey(scratchUserId);
                    if (scratchuser != null)
                    {
                        _scratchUserRepository.Remove(scratchuser);
                        _scratchUserRepository.Context.Commit();
                        return Json(new JsonError { errorcode = "ok", message = "中奖信息清除成功!" });
                    }
                    return Json(new JsonError { errorcode = "error", message = "中奖信息清除失败!" });
                }
                catch (Exception ex)
                {
                    return Json(new JsonError { errorcode = "error", message = "没有删除!" + ex.Message });
                }
            }
        }

        #endregion 用户放弃奖品

        #region 删除过期的中奖用户(可能是那种中奖后又没有刮开,然后退出页面的情况)

        /// <summary>
        /// 删除过期信息不全的中奖用户
        /// </summary>
        private void DleExpirationUser()
        {
            var scratchusers = _scratchUserRepository.FindAll().Where(s => s.AddDate <= ExpirationTime && string.IsNullOrWhiteSpace(s.Phone) && string.IsNullOrWhiteSpace(s.Name));
            foreach (var item in scratchusers)
            {
                _scratchUserRepository.Remove(item);
                _scratchUserRepository.Context.Commit();
            }
        }

        #endregion 删除过期的中奖用户(可能是那种中奖后又没有刮开,然后退出页面的情况)

        #region 分享后添加一次抽奖机会

        /// <summary>
        /// 分享后获得额外机会
        /// </summary>
        /// <returns></returns>
        public JsonResult AddShare(Guid scratchId, Guid wiaId)
        {
            lock (_object)
            {
                if (!CheckValid(new Guid(Convert.ToString(scratchId))))
                {
                    return Json(new JsonError { errorcode = "error", message = "活动已经结束,下回再来吧!" });
                }
                const int maxShare = 2;
                var weixinUserInActivity =
                        _weixinUserInActivityRepository.FindAll().SingleOrDefault(s => s.ID == wiaId);

                #region 判断是否每日分享的次数是否超过了限度

                if (weixinUserInActivity != null && weixinUserInActivity.Today == DateTime.Now.ToShortDateString())
                {
                    if (weixinUserInActivity.TodayAdd >= maxShare)
                    {
                        return Json(new JsonError { errorcode = "error", message = string.Format("你今天已经分享超过{0}次<br/>明天再来吧!", maxShare) });
                    }
                }

                #endregion 判断是否每日分享的次数是否超过了限度

                try
                {
                    if (weixinUserInActivity != null)
                    {
                        //如果不是当天，需要清零
                        if (weixinUserInActivity.Today != DateTime.Now.ToShortDateString())
                            weixinUserInActivity.TodayAdd = 0;
                        weixinUserInActivity.UpdateDate = DateTime.Now;
                        weixinUserInActivity.TodayAdd++;
                        weixinUserInActivity.Today = DateTime.Now.ToShortDateString();
                        weixinUserInActivity.Count++;
                        ViewData["Count"] = weixinUserInActivity.Count;
                        _weixinUserInActivityRepository.Update(weixinUserInActivity);
                        _weixinUserInActivityRepository.Context.Commit();
                        return Json(new JsonError { errorcode = "ok", message = "恭喜获得一次抽奖机会" });
                    }
                }
                catch (Exception)
                {
                    return Json(new JsonError { errorcode = "error", message = "请稍后重试！" });
                }
                return Json(new JsonError { errorcode = "ok", message = "请稍后重试." });
            }
        }

        #endregion 分享后添加一次抽奖机会

        #region 封装了抽奖的具体算法

        /// <summary>
        ///     抽奖算法,返回中奖项的id
        /// </summary>
        /// <param name="scratch"></param>
        /// <returns></returns>
        private int GetScratchItemId(Scratch scratch)
        {
            lock (_object)
            {
                DleExpirationUser();
                var random = new Random(DateTime.Now.Millisecond);
                var num1 = scratch.ScratchItems.Sum(s => s.ScratchItemScale);
                var num2 = scratch.ScratchScale;
                var maxValue = Math.Max(num1, num2);
                var randomNum = random.Next(1, maxValue);
                //1.给每个奖项计算当前剩余名额
                //2.通过log的isAward的这个字段来判断，这个字段其实可以改成item的ItemId(奖品ID)
                var scratchItems = scratch.ScratchItems.ToList();

                var sum = 0;
                var result = 0;
                //eg: 3个奖项分别剩余5,10,20，也就是说1~35都是中奖，然后大于36的话isAward还是0，不中奖
                foreach (var item in scratchItems)
                {
                    var scratchUsers = _scratchUserRepository.FindAll()
                        .Count(s => s.ScratchItemID == item.ScratchItemID);

                    sum += item.ScratchItemScale > scratchUsers ? item.ScratchItemScale - scratchUsers : 0;
                    if (randomNum <= sum)
                    {
                        result = item.ScratchItemID;
                        break;
                    }
                }
                return result;
            }
        }

        #endregion 封装了抽奖的具体算法

        #region 进入页面先判断活动是否过期,如果过期,转到404页面

        /// <summary>
        /// 检测活动是否过期
        /// </summary>
        public bool CheckValid(Guid scratchId)
        {
            try
            {
                var scratch = _scratchRepository.FindAll().FirstOrDefault(s => s.ID == scratchId);
                if (scratch == null)
                    return false;

                if (DateTime.Now < scratch.StartDate)
                {
                    return false;
                }
                if (DateTime.Now > scratch.EndDate.AddDays(1))
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion 进入页面先判断活动是否过期,如果过期,转到404页面

        public ActionResult Test()
        {
            Scratch scratch = _scratchRepository.FindAll().FirstOrDefault();
            if (Request.Url != null)
            {
                var shareUrl = Request.Url.AbsoluteUri;
                var result = CommonApi.GetWxConfigResult(scratch.UserProfile.AppId, scratch.UserProfile.AppSecret, shareUrl);
                @ViewData["ticket"] = result.ticket;
                @ViewData["url"] = result.url;

                ViewData["appid"] = result.appid;
                ViewData["timestamp"] = result.timestamp;
                ViewData["noncestr"] = result.noncestr;
                ViewData["signature"] = result.signature;
            }
            return View();
        }
    }
}