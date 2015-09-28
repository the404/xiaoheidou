using Apworks.Specifications;
using AutoMapper;
using EasyWeixin.Data.Repositories;
using EasyWeixin.Model;
using EasyWeixin.Web.Filters;
using EasyWeixin.Web.Models;
using Senparc.Weixin.MP.Entities;
using System;
using System.Linq;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace EasyWeixin.Web.Controllers
{
    [InitializeSimpleMembership]
    [Authorize(Roles = "Weixin")]
    public class ResponseMessageController : Controller
    {
        private readonly IResponseMessageRepository _responseMessageRepository;
        private readonly IResponseKeyRepository _responseKeyRepository;
        private readonly IResponseKeyRuleRepository _responseKeyRuleRepository;
        private readonly IResponseImageRepository _responseImageRepository;
        private readonly IResponseImageTextRepository _responseImageTextRepository;
        private readonly IResponseMusicRepository _responseMusicRepository;
        private readonly IResponseVideoRepository _responseVideoRepository;

        public ResponseMessageController(IResponseImageRepository responseImageRepository,
            IResponseImageTextRepository responseImageTextRepository, IResponseMusicRepository responseMusicRepository,
            IResponseVideoRepository responseVideoRepository, IResponseKeyRuleRepository responseKeyRuleRepository,
            IResponseMessageRepository responseMessageRepository, IResponseKeyRepository responseKeyRepository)
        {
            this._responseImageRepository = responseImageRepository;
            this._responseImageTextRepository = responseImageTextRepository;
            this._responseMusicRepository = responseMusicRepository;
            this._responseVideoRepository = responseVideoRepository;
            this._responseKeyRuleRepository = responseKeyRuleRepository;
            this._responseMessageRepository = responseMessageRepository;
            this._responseKeyRepository = responseKeyRepository;
        }

        public JsonResult CreateOrEdit(ResponseMessage ResponseMessage)
        {
            if (!String.IsNullOrEmpty(ResponseMessage.ResponseMessageID.ToString()) &&
                ResponseMessage.ResponseMessageID != 0)
            {
                ResponseMessage RM =
                    _responseMessageRepository.Find(
                        Specification<ResponseMessage>.Eval(
                            o => o.ResponseMessageID == ResponseMessage.ResponseMessageID));
                RM.ResponseVideoID = ResponseMessage.ResponseVideoID;
                RM.ResponseImageID = ResponseMessage.ResponseImageID;
                RM.ResponseImageTextID = ResponseMessage.ResponseImageTextID;
                RM.ResponseMusicID = ResponseMessage.ResponseMusicID;
                RM.Link = ResponseMessage.Link;
                RM.Content = ResponseMessage.Content;
                RM.AddTime = DateTime.Now;
                RM.ResponseType = ResponseMessage.ResponseType;
                RM.ButtonType = ResponseMessage.ButtonType;
                _responseMessageRepository.Update(RM);
                _responseMessageRepository.Context.Commit();
            }
            else
            {
                ResponseMessage.AddTime = DateTime.Now;
                _responseMessageRepository.Add(ResponseMessage);
                _responseMessageRepository.Context.Commit();
            }
            return Json(ResponseMessage);
        }

        public JsonResult Edit(ResponseMessage ResponseMessage)
        {
            ResponseMessage.AddTime = DateTime.Now;
            _responseMessageRepository.Add(ResponseMessage);
            _responseMessageRepository.Context.Commit();
            return Json(ResponseMessage);
        }

        public JsonResult GetResponseMessage(int ResponseMessageID)
        {
            var data =
                _responseMessageRepository.Find(
                    Specification<ResponseMessage>.Eval(o => o.ResponseMessageID == ResponseMessageID));
            ResponseMessageViewModel ResponseMessage = new ResponseMessageViewModel();
            ResponseMessage.ResponseImage = Mapper.Map<ResponseImage, ResponseImageViewModel>(data.ResponseImage);
            ResponseMessage.ResponseImageText =
                Mapper.Map<ResponseImageText, ResponseImageTextViewModel>(data.ResponseImageText);
            ResponseMessage.ResponseMusic = Mapper.Map<ResponseMusic, ResponseMusicViewModel>(data.ResponseMusic);
            ResponseMessage.ResponseVideo = Mapper.Map<ResponseVideo, ResponseVideoViewModel>(data.ResponseVideo);
            return Json(ResponseMessage);
        }

        #region 自动消息回复

        public ActionResult MessageAutoResponse()
        {
            var data =
                _responseMessageRepository.Find(
                    Specification<ResponseMessage>.Eval(
                        o => o.ResponseType == 2 && o.UserId == WebSecurity.GetUserId(User.Identity.Name)));
            ResponseMessageViewModel ResponseMessage = Mapper.Map<ResponseMessage, ResponseMessageViewModel>(data);
            return View(ResponseMessage);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult MessageAutoResponse(ResponseMessageViewModel form)
        {
            //2表示自动回复信息 每个用户在数据库只有一条对应的自动回复的信息
            var data =
                _responseMessageRepository.Find(
                    Specification<ResponseMessage>.Eval(
                        o => o.ResponseType == 2 && o.UserId == WebSecurity.GetUserId(User.Identity.Name)));
            form.AddTime = DateTime.Now;
            form.UserId = WebSecurity.GetUserId(User.Identity.Name);
            form.ResponseType = 2;
            if (form.ButtonType != 0)
            {
                form.Content = "";
            }
            if (form.ButtonType != 1)
            {
                form.ResponseMusicID = null;
            }
            if (form.ButtonType != 2)
            {
                form.ResponseImageID = null;
            }
            if (form.ButtonType != 3)
            {
                form.ResponseVideoID = null;
            }
            if (form.ButtonType != 4)
            {
                form.ResponseImageTextID = null;
            }
            if (form.ButtonType != 5)
            {
                form.Link = "";
            }

            ResponseMessage ResponseMessage = Mapper.Map<ResponseMessageViewModel, ResponseMessage>(form);
            if (form.ResponseMessageID == 0 && data == null)
            {
                _responseMessageRepository.Add(ResponseMessage);
                _responseMessageRepository.Context.Commit();
                form = Mapper.Map<ResponseMessage, ResponseMessageViewModel>(ResponseMessage);
            }
            else
            {
                data.ButtonType = form.ButtonType;
                data.AddTime = DateTime.Now;
                data.ResponseImageTextID = form.ResponseImageTextID;
                data.ResponseImageID = form.ResponseImageID;
                data.ResponseMusicID = form.ResponseMusicID;
                data.ResponseVideoID = form.ResponseVideoID;
                form.Content = form.Content ?? "";
                data.Content = form.Content.Trim();
                data.Link = form.Link;
                _responseMessageRepository.Update(data);
                _responseMessageRepository.Context.Commit();
                form = Mapper.Map<ResponseMessage, ResponseMessageViewModel>(data);
            }
            return View(form);
        }

        #endregion 自动消息回复

        #region 关注时自动回复

        public ActionResult AddedMessageAutoResponse()
        {
            ViewData["AddSuccessMessage"] = "";
            var data =
                _responseMessageRepository.Find(
                    Specification<ResponseMessage>.Eval(
                        o => o.ResponseType == 1 && o.UserId == WebSecurity.GetUserId(User.Identity.Name)));
            ResponseMessageViewModel ResponseMessage = Mapper.Map<ResponseMessage, ResponseMessageViewModel>(data);
            return View(ResponseMessage);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddedMessageAutoResponse(ResponseMessageViewModel form)
        {
            ViewData["AddSuccessMessage"] = "";
            try
            {
                //1表示被添加自动回复信息 每个用户在数据库只有一条对应的被添加自动回复的信息
                var data =
                    _responseMessageRepository.Find(
                        Specification<ResponseMessage>.Eval(
                            o => o.ResponseType == 1 && o.UserId == WebSecurity.GetUserId(User.Identity.Name)));
                form.AddTime = DateTime.Now;
                form.UserId = WebSecurity.GetUserId(User.Identity.Name);
                form.ResponseType = 1;
                if (form.ButtonType != 0)
                {
                    form.Content = "";
                }
                if (form.ButtonType != 1)
                {
                    form.ResponseMusicID = null;
                }
                if (form.ButtonType != 2)
                {
                    form.ResponseImageID = null;
                }
                if (form.ButtonType != 3)
                {
                    form.ResponseVideoID = null;
                }
                if (form.ButtonType != 4)
                {
                    form.ResponseImageTextID = null;
                }
                if (form.ButtonType != 5)
                {
                    form.Link = "";
                }

                ResponseMessage ResponseMessage = Mapper.Map<ResponseMessageViewModel, ResponseMessage>(form);
                if (form.ResponseMessageID == 0 && data == null)
                {
                    _responseMessageRepository.Add(ResponseMessage);
                    _responseMessageRepository.Context.Commit();
                    form = Mapper.Map<ResponseMessage, ResponseMessageViewModel>(ResponseMessage);
                }
                else
                {
                    data.ButtonType = form.ButtonType;
                    data.AddTime = DateTime.Now;
                    data.ResponseImageTextID = form.ResponseImageTextID;
                    data.ResponseImageID = form.ResponseImageID;
                    data.ResponseMusicID = form.ResponseMusicID;
                    data.ResponseVideoID = form.ResponseVideoID;
                    data.Content = form.Content;
                    data.Link = form.Link;
                    _responseMessageRepository.Update(data);
                    _responseMessageRepository.Context.Commit();
                    form = Mapper.Map<ResponseMessage, ResponseMessageViewModel>(data);
                    ViewData["AddSuccessMessage"] = "  ** 提交成功";
                }
                return View(form);
            }
            catch (Exception ex)
            {
                return Json(new { ErrorMessage = ex.Message });
            }
        }

        #endregion 关注时自动回复

        #region 关键字回复

        public ActionResult KeyMessageAutoResponse()
        {
            int userID = WebSecurity.GetUserId(User.Identity.Name);
            var data =
                _responseKeyRuleRepository.FindAll(Specification<ResponseKeyRule>.Eval(o => o.UserId == userID))
                    .OrderByDescending(o => o.ResponseKeyRuleID);
            return View(data.ToList());
        }

        /// <summary>
        /// 编辑规则返回部分视图
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult KeyRuleEdit(string datas, int keyindex)
        {
            ViewData["keyindex"] = keyindex;
            ResponseKeyRule form = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseKeyRule>(datas);
            int userID = WebSecurity.GetUserId(User.Identity.Name);

            if (form.ResponseKeyRuleID == 0)
            {
                form.IsOrder = 0;
                form.AddTime = DateTime.Now;
                form.UserId = userID;

                foreach (var item in form.ResponseMessages)
                {
                    item.ResponseType = 3;
                    item.AddTime = DateTime.Now;
                    item.UserId = userID;
                    item.Content = Server.UrlDecode(item.Content);
                }

                foreach (var item in form.ResponseKeys)
                {
                    item.AddTime = DateTime.Now;
                    item.IsOrder = 0;
                    item.UserId = userID;
                }
                _responseKeyRuleRepository.Add(form);
                _responseKeyRuleRepository.Context.Commit();
                foreach (var item in form.ResponseMessages)
                {
                    if (item.ResponseImageID != null)
                    {
                        item.ResponseImage =
                            _responseImageRepository.Find(
                                Specification<ResponseImage>.Eval(o => o.ResponseImageID == item.ResponseImageID));
                    }
                    if (item.ResponseImageTextID != null)
                    {
                        item.ResponseImageText =
                            _responseImageTextRepository.Find(
                                Specification<ResponseImageText>.Eval(
                                    o => o.ResponseImageTextID == item.ResponseImageTextID));
                    }
                    if (item.ResponseMusicID != null)
                    {
                        item.ResponseMusic =
                            _responseMusicRepository.Find(
                                Specification<ResponseMusic>.Eval(o => o.ResponseMusicID == item.ResponseMusicID));
                    }
                    if (item.ResponseVideoID != null)
                    {
                        item.ResponseVideo =
                            _responseVideoRepository.Find(
                                Specification<ResponseVideo>.Eval(o => o.ResponseVideoID == item.ResponseVideoID));
                    }
                }
                return PartialView("KeyRuleEdit", form);
            }
            else
            {
                foreach (var item in form.ResponseMessages)
                {
                    if (item.ResponseMessageID == 0)
                    {
                        item.ResponseType = 3;
                        item.AddTime = DateTime.Now;
                        item.ResponseKeyRuleID = form.ResponseKeyRuleID;
                        item.UserId = userID;
                        item.Content = Server.UrlDecode(item.Content);
                        _responseMessageRepository.Add(item);
                    }
                    _responseMessageRepository.Context.Commit();
                }
                //添加更改关键字
                foreach (var item in form.ResponseKeys)
                {
                    if (item.ResponseKeyID == 0)
                    {
                        item.AddTime = DateTime.Now;
                        item.IsOrder = 0;
                        item.UserId = userID;
                        item.ResponseKeyRuleID = form.ResponseKeyRuleID;
                        _responseKeyRepository.Add(item);
                    }
                    else
                    {
                        var ResponseKey =
                            _responseKeyRepository.Find(
                                Specification<ResponseKey>.Eval(o => o.ResponseKeyID == item.ResponseKeyID));
                        ResponseKey.Key = item.Key;
                        ResponseKey.IsFullMatch = item.IsFullMatch;
                        _responseKeyRepository.Update(ResponseKey);
                    }
                    _responseKeyRepository.Context.Commit();
                }
                var command =
                    _responseKeyRuleRepository.Find(
                        Specification<ResponseKeyRule>.Eval(o => o.ResponseKeyRuleID == form.ResponseKeyRuleID));
                command.RuleName = form.RuleName;
                _responseKeyRuleRepository.Update(command);
                _responseKeyRuleRepository.Context.Commit();
                //获取信息数据
                var ResponseMessages =
                    _responseMessageRepository.FindAll(
                        Specification<ResponseMessage>.Eval(o => o.ResponseKeyRuleID == form.ResponseKeyRuleID));
                //添加信息 不存在更改信息
                //删除信息
                foreach (var item in ResponseMessages)
                {
                    if (
                        form.ResponseMessages.Where(o => o.ResponseMessageID == item.ResponseMessageID).ToList().Count ==
                        0)
                    {
                        _responseMessageRepository.Remove(item);
                        _responseMessageRepository.Context.Commit();
                    }
                }

                var ResponseKeys =
                    _responseKeyRepository.FindAll(
                        Specification<ResponseKey>.Eval(o => o.ResponseKeyRuleID == form.ResponseKeyRuleID));
                //删除关键字
                foreach (var item in ResponseKeys)
                {
                    if (form.ResponseKeys.Where(o => o.ResponseKeyID == item.ResponseKeyID).ToList().Count == 0)
                    {
                        _responseKeyRepository.Remove(item);
                        _responseKeyRepository.Context.Commit();
                    }
                }
                foreach (var item in command.ResponseMessages)
                {
                    if (item.ResponseImageID != null)
                    {
                        item.ResponseImage =
                            _responseImageRepository.Find(
                                Specification<ResponseImage>.Eval(o => o.ResponseImageID == item.ResponseImageID));
                    }
                    if (item.ResponseImageTextID != null)
                    {
                        item.ResponseImageText =
                            _responseImageTextRepository.Find(
                                Specification<ResponseImageText>.Eval(
                                    o => o.ResponseImageTextID == item.ResponseImageTextID));
                    }
                    if (item.ResponseMusicID != null)
                    {
                        item.ResponseMusic =
                            _responseMusicRepository.Find(
                                Specification<ResponseMusic>.Eval(o => o.ResponseMusicID == item.ResponseMusicID));
                    }
                    if (item.ResponseVideoID != null)
                    {
                        item.ResponseVideo =
                            _responseVideoRepository.Find(
                                Specification<ResponseVideo>.Eval(o => o.ResponseVideoID == item.ResponseVideoID));
                    }
                }
                return PartialView("KeyRuleEdit", command);
            }
        }

        [HttpPost]
        public JsonResult KeyRuleDel(int ResponseKeyRuleID)
        {
            if (ResponseKeyRuleID != 0)
            {
                var command =
                    _responseKeyRuleRepository.Find(
                        Specification<ResponseKeyRule>.Eval(o => o.ResponseKeyRuleID == ResponseKeyRuleID));
                //获取信息数据
                var ResponseMessages =
                    _responseMessageRepository.FindAll(
                        Specification<ResponseMessage>.Eval(o => o.ResponseKeyRuleID == ResponseKeyRuleID));
                //删除信息
                foreach (var item in ResponseMessages)
                {
                    _responseMessageRepository.Remove(item);
                    _responseMessageRepository.Context.Commit();
                }

                var ResponseKeys =
                    _responseKeyRepository.FindAll(
                        Specification<ResponseKey>.Eval(o => o.ResponseKeyRuleID == ResponseKeyRuleID));
                //删除关键字
                foreach (var item in ResponseKeys)
                {
                    _responseKeyRepository.Remove(item);
                    _responseKeyRepository.Context.Commit();
                }
                _responseKeyRuleRepository.Remove(command);
                _responseKeyRuleRepository.Context.Commit();
            }
            return Json("");
        }

        /// <summary>
        /// 返回添加规则的部分视图
        /// </summary>
        /// <returns></returns>
        public ActionResult KeyRuleAdd()
        {
            return PartialView("KeyRuleAdd");
        }

        /// <summary>
        /// 返回添加关键字的视图
        /// </summary>
        /// <returns></returns>
        public ActionResult DialogKeyName(ResponseKey ResponseKey)
        {
            return View(ResponseKey);
        }

        public ActionResult DialogContent()
        {
            return View();
        }

        public ActionResult text()
        {
            var datas = _responseKeyRepository.FindAll().FirstOrDefault(o => o.Key == "250");
            if (datas != null && datas.ResponseKeyRules != null &&
                datas.ResponseKeyRules.ResponseMessages.ToList().Count > 0)
            {
                //返回类型Text
                if (datas.ResponseKeyRules.ResponseMessages.Where(o => o.ButtonType == 0).ToList().Count > 0)
                {
                    var responseMessage = new ResponseMessageText();
                    responseMessage.Content = datas.ResponseKeyRules.ResponseMessages.ElementAt(0).Content;
                    return View();
                }
                else if (datas.ResponseKeyRules.ResponseMessages.Where(o => o.ButtonType == 1).ToList().Count > 0)
                {
                    var responseMessage = new ResponseMessageMusic();
                    responseMessage.Music.Title =
                        datas.ResponseKeyRules.ResponseMessages.ElementAt(0).ResponseMusic.MusicName;
                    responseMessage.Music.HQMusicUrl =
                        datas.ResponseKeyRules.ResponseMessages.ElementAt(0).ResponseMusic.HQMusicUrl;
                    responseMessage.Music.MusicUrl =
                        datas.ResponseKeyRules.ResponseMessages.ElementAt(0).ResponseMusic.MusicUrl;
                    responseMessage.Music.Description = "";
                    return View();
                }
                else if (datas.ResponseKeyRules.ResponseMessages.Where(o => o.ButtonType == 4).ToList().Count > 0)
                {
                    var responseMessage = new ResponseMessageNews();
                    foreach (var item in datas.ResponseKeyRules.ResponseMessages)
                    {
                        responseMessage.Articles.Add(new Article()
                        {
                            Description = item.ResponseImageText.Content,
                            Title = item.ResponseImageText.ImageTextName,
                            PicUrl = "",
                            Url = "www.baidu.com"
                        });
                    }
                    return View();
                }
                else
                {
                    var errorMessage = new ResponseMessageText();
                    errorMessage.Content = "小妹纸不懂官人的意思，到我网址<a href=\"http://home.ipow.cn\">home.ipow.cn</a>看看吧1";
                    return View();
                }
            }
            else
            {
                var errorMessage = new ResponseMessageText();
                errorMessage.Content = "小妹纸不懂官人的意思，到我网址<a href=\"http://home.ipow.cn\">home.ipow.cn</a>看看吧2";
                return View();
            }
        }

        #endregion 关键字回复
    }
}