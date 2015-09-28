using Apworks.Specifications;
using AutoMapper;
using EasyWeixin.CommonAPIs;
using EasyWeixin.Data.Repositories;
using EasyWeixin.Entities.JsonResult;
using EasyWeixin.Entities.Request;
using EasyWeixin.Exceptions;
using EasyWeixin.Model;
using EasyWeixin.Web.Models;
using Senparc.Weixin.MP.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace EasyWeixin.Web.Controllers
{
    [Authorize(Roles = "Weixin")]
    public class SelfDefiningMenuController : Controller
    {
        #region ctor

        private readonly IButtonRepository _buttonRepository;
        private readonly ISubButtonRepository _subButtonRepository;
        private readonly IResponseMessageRepository _responseMessageRepository;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IResponseImageTextRepository _responseImageTextRepository;

        public SelfDefiningMenuController(IResponseImageTextRepository responseImageTextRepository,
            IUserProfileRepository userProfileRepository, IButtonRepository buttonRepository,
            ISubButtonRepository subButtonRepository, IResponseMessageRepository responseMessageRepository)
        {
            this._userProfileRepository = userProfileRepository;
            this._buttonRepository = buttonRepository;
            this._subButtonRepository = subButtonRepository;
            this._responseMessageRepository = responseMessageRepository;
            this._responseImageTextRepository = responseImageTextRepository;
        }

        #endregion ctor

        #region 打开主界面
        public ActionResult Index()
        {
            return
                View(
                    _buttonRepository.FindAll()
                        .Where(o => o.UserId == WebSecurity.GetUserId(User.Identity.Name))
                        .OrderBy(o => o.ButtonID)
                        .ToList());
        }

        #endregion 打开主界面

        #region 菜单操作

        #region 创建菜单

        #region 创建菜单弹出层
        public ActionResult Create()
        {
            return View();
        }

        #endregion 创建菜单弹出层

        #region 创建主菜单

        [HttpPost]
        public JsonResult CreateButton(ButtonViewModel buttonViewModel)
        {
            if (!IsButtonSum())
            {
                return Json(new { ErrorMessage = "主菜单按钮个数必须小于等于三个" });
            }
            Button button = new Button
            {
                AddTime = DateTime.Now,
                UserId = WebSecurity.GetUserId(User.Identity.Name),
                type = "click",
                IsOrder = 0,
                name = buttonViewModel.name
            };
            if (ModelState.IsValid)
            {
                _buttonRepository.Add(button);
                _buttonRepository.Context.Commit();
                button.key = "Button_click_" + button.ButtonID;
                _buttonRepository.Update(button);
                _buttonRepository.Context.Commit();
                buttonViewModel = Mapper.Map<ButtonViewModel>(button);
                return Json(buttonViewModel);
            }
            return Json(new { ErrorMessage = ModelState.Values.ToList()[0].Errors.ToList()[0].ErrorMessage });
        }

        #endregion 创建主菜单

        #region 创建子菜单

        [HttpPost]
        public JsonResult CreateSubButton(SubButtonViewModel subButtonViewModel)
        {
            if (!IsSubButtonSum(subButtonViewModel.ButtonID))
            {
                return Json(new { ErrorMessage = "子菜单按钮个数必须小于等于五个" });
            }
            SubButton subButton = new SubButton
            {
                ButtonID = subButtonViewModel.ButtonID,
                name = subButtonViewModel.name,
                AddTime = DateTime.Now,
                IsOrder = 0,
                type = "click"
            };

            if (ModelState.IsValid)
            {
                _subButtonRepository.Add(subButton);
                _subButtonRepository.Context.Commit();
                subButton.key = "SubButton_click_" + subButton.SubButtonID.ToString();
                _subButtonRepository.Update(subButton);
                _subButtonRepository.Context.Commit();
                subButtonViewModel = Mapper.Map<SubButtonViewModel>(subButton);
                return Json(subButtonViewModel);
            }
            return Json(new { ErrorMessage = ModelState.Values.ToList()[0].Errors.ToList()[0].ErrorMessage });
        }

        #endregion 创建子菜单

        #endregion 创建菜单

        #region 编辑菜单名称

        #region 打开编辑主菜单

        public ActionResult EditButton(Guid id)
        {
            var Button = _buttonRepository.GetByKey(id);
            if (Button == null)
            {
                return HttpNotFound();
            }
            return View(Button);
        }

        #endregion 打开编辑主菜单

        #region 打开编辑子菜单

        public ActionResult EditSubButton(Guid id)
        {
            var SubButton = _subButtonRepository.GetByKey(id);
            if (SubButton == null)
            {
                return HttpNotFound();
            }
            return View(SubButton);
        }

        #endregion 打开编辑子菜单

        #region 提交编辑主菜单

        [HttpPost]
        public JsonResult EditButton(ButtonViewModel buttonViewModel)
        {
            try
            {
                Button Button = _buttonRepository.GetByKey(buttonViewModel.ID);
                if (!String.IsNullOrEmpty(buttonViewModel.name))
                {
                    Button.name = buttonViewModel.name;
                }

                Button.AddTime = DateTime.Now;
                Button.UserId = WebSecurity.GetUserId(User.Identity.Name);
                Button.IsOrder = 0;
                Button.key = "Button_click_" + Button.ButtonID.ToString();
                Button.type = "click";
                if (ModelState.IsValid)
                {
                    _buttonRepository.Update(Button);
                    _buttonRepository.Context.Commit();
                    buttonViewModel = Mapper.Map<Button, ButtonViewModel>(Button);
                    return Json(buttonViewModel);
                }
                else
                {
                    return Json(new { ErrorMessage = ModelState.Values.ToList()[0].Errors.ToList()[0].ErrorMessage });
                }
            }
            catch (ErrorJsonResultException ex)
            {
                return Json(new { ErrorMessage = ex.JsonResult.errcode + "," + ex.JsonResult.errmsg });
            }
        }

        #endregion 提交编辑主菜单

        #region 提交编辑子菜单

        [HttpPost]
        public JsonResult EditSubButton(SubButtonViewModel subButtonViewModel)
        {
            try
            {
                SubButton SubButton = _subButtonRepository.GetByKey(subButtonViewModel.ID);
                if (!String.IsNullOrEmpty(subButtonViewModel.name))
                {
                    SubButton.name = subButtonViewModel.name;
                }
                SubButton.AddTime = DateTime.Now;
                SubButton.IsOrder = 0;
                SubButton.key = "SubButton_click_" + SubButton.SubButtonID.ToString();
                SubButton.type = "click";
                if (ModelState.IsValid)
                {
                    _subButtonRepository.Update(SubButton);
                    _subButtonRepository.Context.Commit();
                    subButtonViewModel = Mapper.Map<SubButton, SubButtonViewModel>(SubButton);
                    return Json(subButtonViewModel);
                }
                else
                {
                    return Json(new { ErrorMessage = ModelState.Values.ToList()[0].Errors.ToList()[0].ErrorMessage });
                }
            }
            catch (ErrorJsonResultException ex)
            {
                return Json(new { ErrorMessage = ex.JsonResult.errcode + "," + ex.JsonResult.errmsg });
            }
        }

        #endregion 提交编辑子菜单

        #endregion 编辑菜单名称

        #region 删除菜单

        #region 删除主菜单

        [HttpPost]
        public JsonResult DeleteButton(Guid id)
        {
            Button Button = _buttonRepository.GetByKey(id);
            foreach (var item in Button.ResponseMessages.ToList())
            {
                _responseMessageRepository.Remove(item);
                _responseMessageRepository.Context.Commit();
            }
            _buttonRepository.Remove(Button);
            _buttonRepository.Context.Commit();
            return Json(new { ErrorMessage = "删除成功" });
        }

        #endregion 删除主菜单

        #region 删除子菜单

        [HttpPost]
        public JsonResult DeleteSubButton(Guid id)
        {
            SubButton SubButton = _subButtonRepository.GetByKey(id);
            foreach (var item in SubButton.ResponseMessages.ToList())
            {
                _responseMessageRepository.Remove(item);
                _responseMessageRepository.Context.Commit();
            }
            _subButtonRepository.Remove(SubButton);
            _subButtonRepository.Context.Commit();
            return Json(new { ErrorMessage = "删除成功" });
        }

        #endregion 删除子菜单

        #endregion 删除菜单

        #endregion 菜单操作

        #region 右侧编辑

        #region 打开右侧编辑,返回部分视图

        /// <summary>
        /// 返回右侧编辑的部分视图
        /// </summary>
        /// <returns></returns>
        public ActionResult ResponseMessageEdit(int buttonId, int subButtonId)
        {
            var buttonData =
                _responseMessageRepository.FindAll().Where(s => s.ButtonID == buttonId).ToList();
            var defaultData = buttonData; //防止报错,默认值
            var subButtonData =
                _responseMessageRepository.FindAll().Where(s => s.SubButtonID == subButtonId).ToList();

            if (buttonId != 0)
            {
                return PartialView("ResponseMessageEdit", buttonData);
            }
            if (subButtonId != 0)
            {
                return PartialView("ResponseMessageEdit", subButtonData);
            }
            return PartialView("ResponseMessageEdit", defaultData);
        }

        #endregion 打开右侧编辑,返回部分视图

        #region 返回右侧展示的部分视图

        /// <summary>
        /// 返回右侧展示的部分视图
        /// </summary>
        /// <returns></returns>
        public ActionResult ResponseMessageIndex(int buttonId, int subButtonId)
        {
            var buttonData =
                _responseMessageRepository.FindAll().Where(s => s.ButtonID == buttonId).ToList();
            var defaultData = buttonData; //防止报错,默认值
            var subButtonData =
                _responseMessageRepository.FindAll().Where(s => s.SubButtonID == subButtonId).ToList();
            if (buttonId != 0)
            {
                return PartialView("ResponseMessageIndex", buttonData);
            }
            if (subButtonId != 0)
            {
                return PartialView("ResponseMessageIndex", subButtonData);
            }
            return PartialView("ResponseMessageIndex", defaultData);
        }

        #endregion 返回右侧展示的部分视图

        #region 提交右侧编辑,返回部分视图

        /// <summary>
        /// 提交右侧编辑,返回部分视图
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ResponseMessageSubmit(string datas)
        {
            List<ResponseMessage> responseMessages =
                Newtonsoft.Json.JsonConvert.DeserializeObject<List<ResponseMessage>>(datas);
            List<ResponseMessage> rms;
            if (responseMessages.Count == 0)
            {
                return PartialView("ResponseMessageEdit");
            }
            if (responseMessages[0].ButtonID > 0)
            {
                var buttonId = int.Parse(responseMessages[0].ButtonID.ToString());
                rms =
                    _responseMessageRepository.FindAll().Where(s => s.ButtonID == buttonId).ToList();
            }
            else
            {
                var subButtonId = int.Parse(responseMessages[0].SubButtonID.ToString());
                rms =
                    _responseMessageRepository.FindAll().Where(s => s.SubButtonID == subButtonId).ToList();
            }
            foreach (var item in rms)
            {
                _responseMessageRepository.Remove(item);
                _responseMessageRepository.Context.Commit();
            }
            var rms2 = new List<ResponseMessage>();
            foreach (var item in responseMessages)
            {
                if (item.ButtonType != 4)
                {
                    var data = new ResponseMessage
                    {
                        ButtonType = item.ButtonType,
                        AddTime = DateTime.Now,
                        ResponseImageTextID = item.ResponseImageTextID,
                        ResponseImageID = item.ResponseImageID,
                        ResponseMusicID = item.ResponseMusicID,
                        ResponseVideoID = item.ResponseVideoID,
                        Content = item.Content,
                        Link = item.Link,
                        ButtonID = item.ButtonID == 0 ? null : item.ButtonID,
                        SubButtonID = item.SubButtonID == 0 ? null : item.SubButtonID
                    };
                    _responseMessageRepository.Add(data);
                    _responseMessageRepository.Context.Commit();
                    rms2.Add(data);
                }
                else
                {
                    item.ID = Guid.Empty;
                    item.ButtonID = item.ButtonID == 0 ? null : item.ButtonID;
                    item.SubButtonID = item.SubButtonID == 0 ? null : item.SubButtonID;
                    item.ResponseMessageID = 0;
                    item.AddTime = DateTime.Now;
                    _responseMessageRepository.Add(item);
                    _responseMessageRepository.Context.Commit();
                    item.ResponseImageText =
                        _responseImageTextRepository.Find(
                            Specification<ResponseImageText>.Eval(o => o.ResponseImageTextID == item.ResponseImageTextID));

                    rms2.Add(item);
                }
            }

            return PartialView("ResponseMessageIndex", rms2);
        }

        #endregion 提交右侧编辑,返回部分视图

        #endregion 右侧编辑

        #region 提交数据到微信接口

        public JsonResult UpdateMenu()
        {
            try
            {
                string str = UpdateWeiXinMenu();
                //采用映射输出 防止序列化类型为XX的对象时检测到循环引用的错误发生。
                return Json(new { ErrorMessage = str });
            }
            catch (Exception ex)
            {
                var exception = ex as ErrorJsonResultException;
                if (exception != null)
                {
                    var e = exception;
                    return Json(new { ErrorMessage = e.JsonResult.errcode + "," + e.JsonResult.errmsg });
                }
                else
                {
                    return Json(new { ErrorMessage = ex.Message });
                }
            }
        }

        /// <summary>
        /// 创建微信菜单
        /// </summary>
        private string UpdateWeiXinMenu()
        {
            RequestMessageBase rm = new RequestMessageBase();
            var count =
                _buttonRepository.FindAll()
                    .Where(o => o.UserId == WebSecurity.GetUserId(User.Identity.Name))
                    .ToList()
                    .Count;
            //主菜单为2~3个 否则报错
            if (count > 3 || count < 2)
            {
                return "主菜单按钮个数必须是两个或者三个";
            }
            ///获取用户数据
            var user =
                _userProfileRepository.FindAll().Single(s => s.UserId == WebSecurity.CurrentUserId);
            ///创建微信菜单实例
            EasyWeixin.Entities.Menu.ButtonGroup buttonGroup = new EasyWeixin.Entities.Menu.ButtonGroup();
            ///提取数据库中菜单数据的前三行数据
            var ButtonList =
                _buttonRepository.FindAll(Specification<Button>.Eval(o => o.UserId == WebSecurity.CurrentUserId))
                    .Take(3);
            //WeixinController wc = new WeixinController();
            ///将数据库中数据添加到微信菜单实例中
            foreach (var item in ButtonList)
            {
                if (item.SubButtons != null && item.SubButtons.ToList().Count > 0)
                {
                    EasyWeixin.Entities.Menu.SubButton SubButton = new EasyWeixin.Entities.Menu.SubButton();
                    SubButton.name = item.name;
                    //SubButton.key = "click";
                    foreach (var Subitem in item.SubButtons.Take(5))
                    {
                        if (Subitem.ResponseMessages.ToList().Count > 0 &&
                            Subitem.ResponseMessages.ToList()[0].ButtonType == 5)
                        {
                            EasyWeixin.Entities.Menu.SingleViewButton SingleButton =
                                new EasyWeixin.Entities.Menu.SingleViewButton();
                            SingleButton.name = Subitem.name;
                            SingleButton.url = Subitem.ResponseMessages.ToList()[0].Link;
                            SingleButton.type = "view";
                            SubButton.sub_button.Add(SingleButton);
                        }
                        else
                        {
                            EasyWeixin.Entities.Menu.SingleClickButton SingleButton =
                                new EasyWeixin.Entities.Menu.SingleClickButton();
                            SingleButton.name = Subitem.name;
                            SingleButton.key = "SubButton_click_" + Subitem.SubButtonID.ToString();
                            SingleButton.type = "click";
                            //2014/6/4 tianxiu
                            //if (Subitem.name=="授权")
                            //{
                            //    string MyAppid = user.AppId;
                            //    string RedirectUri = "http://" + Request.Url.Host +"/SelfDefiningMenu/AuthRedirectUri";
                            //    string URL = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + MyAppid + "&redirect_uri=" + RedirectUri + "&response_type=code&scope=snsapi_userinfo&state=STATE#wechat_redirect";
                            //    string Str = "<a href='" + URL + "'>授权页面</a>";
                            //    string res = sendTextMessage(rm, Str);
                            //    Response.Write(res);
                            //}

                            SubButton.sub_button.Add(SingleButton);
                        }
                    }
                    buttonGroup.button.Add(SubButton);
                }
                else
                {
                    if (item.ResponseMessages.ToList().Count > 0 && item.ResponseMessages.ToList()[0].ButtonType == 5)
                    {
                        EasyWeixin.Entities.Menu.SingleViewButton SingleButton =
                            new EasyWeixin.Entities.Menu.SingleViewButton();
                        SingleButton.name = item.name;
                        SingleButton.url = item.ResponseMessages.ToList()[0].Link;
                        SingleButton.type = "view";
                        buttonGroup.button.Add(SingleButton);
                    }
                    else
                    {
                        EasyWeixin.Entities.Menu.SingleClickButton SingleButton =
                            new EasyWeixin.Entities.Menu.SingleClickButton();
                        SingleButton.name = item.name;
                        SingleButton.key = "Button_click_" + item.ButtonID.ToString();
                        SingleButton.type = "click";
                        buttonGroup.button.Add(SingleButton);
                    }
                }
            }
            var token = AccessTokenContainer.TryGetToken(user.AppId, user.AppSecret);
            string str = Newtonsoft.Json.JsonConvert.SerializeObject(buttonGroup);
            //   CommonApi.GetMenuFromJson(token);
            WxJsonResult wx = CommonApi.CreateMenu(token, buttonGroup);

            return "更新成功";
        }

        public bool IsButtonSum()
        {
            var num =
                _buttonRepository.FindAll()
                    .Where(o => o.UserId == WebSecurity.GetUserId(User.Identity.Name))
                    .OrderBy(o => o.ButtonID)
                    .ToList()
                    .Count;
            if (num < 3)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //public JsonResult GetCustomerMsg(string opentid, string resultmsg)
        //{
        //    var msg = "";
        //    ///获取用户数据
        //    var user = UserProfileRepository.Find(Specification<EasyWeixin.Model.UserProfile>.Eval(o => o.UserId == WebSecurity.CurrentUserId));
        //    AccessTokenResult Result = CommonApi.GetToken(user.AppId, user.AppSecret);
        //    string token = Result.access_token;
        //    string jsonR="{";
        //        jsonR+="'touser':'"+opentid+"'";
        //        jsonR+="'msgtype':'text'";
        //        jsonR+="'text':";
        //        jsonR+="{";
        //        jsonR+= "'content':'"+resultmsg+"'";
        //        jsonR+="}}";

        //    WxJsonResult wx = CommonApi.SendCustomerMsg(token,jsonR);
        //    if (wx.errcode == 0)
        //    {
        //        msg = "发送客服消息成功!";
        //    }
        //    else
        //    {
        //        msg = "发送客服消息失败!";
        //    }
        //    return Json(new { success = msg }, JsonRequestBehavior.AllowGet);

        //}

        /// <summary>
        /// 发送文字消息
        /// </summary>
        /// <param name="wx">获取的收发者信息</param>
        /// <param name="content">内容</param>
        /// <returns></returns>
        private string sendTextMessage(RequestMessageBase rm, string content)
        {
            string res = string.Format(@"<xml>
                                   <ToUserName><![CDATA[{0}]]></ToUserName>
                                   <FromUserName><![CDATA[{1}]]></FromUserName>
                                    <CreateTime>{2}</CreateTime>
                                    <MsgType><![CDATA[{3}]]></MsgType>
                                    <Content><![CDATA[{4}]]></Content>
                                   </xml>",
                rm.FromUserName, rm.ToUserName, DateTime.Now, rm.MsgType, content);
            return res;
        }

        public bool IsSubButtonSum(int ButtonID)
        {
            var num = _subButtonRepository.FindAll().Where(o => o.ButtonID == ButtonID).ToList().Count;
            if (num < 5)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion 提交数据到微信接口

        //#region 重定向界面

        //    public ActionResult AuthRedirectUri()
        //    {
        //        if (!string.IsNullOrEmpty(Request.QueryString["code"]))
        //        {
        //            string Code = Request.QueryString["code"].ToString();
        //            //获得Token
        //            OAuth_Token Model = Get_token(Code);
        //            OAuthUser OAuthUser_Model = Get_UserInfo(Model.access_token, Model.openid);
        //            Response.Write("用户OPENID:" + OAuthUser_Model.openid + "<br>用户昵称:" + OAuthUser_Model.nickname + "<br>性别:" + OAuthUser_Model.sex + "<br>所在省:" + OAuthUser_Model.province + "<br>所在市:" + OAuthUser_Model.city + "<br>所在国家:" + OAuthUser_Model.country + "<br>头像地址:" + OAuthUser_Model.headimgurl);
        //        }
        //    }

        //    //获得Token
        //    protected OAuth_Token Get_token(string Code)
        //    {
        //        ///获取用户数据
        //        var user = UserProfileRepository.Find(Specification<EasyWeixin.Model.UserProfile>.Eval(o => o.UserId == WebSecurity.CurrentUserId));
        //        string Str = GetJson("https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + user.AppId+ "&secret=" + user.AppSecret + "&code=" + Code + "&grant_type=authorization_code");
        //        OAuth_Token Oauth_Token_Model = JsonHelper.ParseFromJson<OAuth_Token>(Str);
        //        return Oauth_Token_Model;
        //    }
        //    //刷新Token
        //    protected OAuth_Token refresh_token(string REFRESH_TOKEN)
        //    {
        //        ///获取用户数据
        //        var user = UserProfileRepository.Find(Specification<EasyWeixin.Model.UserProfile>.Eval(o => o.UserId == WebSecurity.CurrentUserId));
        //        string Str = GetJson("https://api.weixin.qq.com/sns/oauth2/refresh_token?appid=" + user.AppId + "&grant_type=refresh_token&refresh_token=" + REFRESH_TOKEN);
        //        OAuth_Token Oauth_Token_Model = JsonHelper.ParseFromJson<OAuth_Token>(Str);
        //        return Oauth_Token_Model;
        //    }
        //    //获得用户信息
        //    protected OAuthUser Get_UserInfo(string REFRESH_TOKEN, string OPENID)
        //    {
        //        // Response.Write("获得用户信息REFRESH_TOKEN:" + REFRESH_TOKEN + "||OPENID:" + OPENID);
        //        string Str = GetJson("https://api.weixin.qq.com/sns/userinfo?access_token=" + REFRESH_TOKEN + "&openid=" + OPENID);
        //        OAuthUser OAuthUser_Model = JsonHelper.ParseFromJson<OAuthUser>(Str);
        //        return OAuthUser_Model;
        //    }
        //    protected string GetJson(string url)
        //    {
        //        WebClient wc = new WebClient();
        //        wc.Credentials = CredentialCache.DefaultCredentials;
        //        wc.Encoding = Encoding.UTF8;
        //        string returnText = wc.DownloadString(url);

        //        if (returnText.Contains("errcode"))
        //        {
        //            //可能发生错误
        //        }
        //        //Response.Write(returnText);
        //        return returnText;
        //    }

        //#endregion

        #region 测试用例

        /// <summary>
        /// 测试用例
        /// </summary>
        /// <returns></returns>
        public ActionResult Text()
        {
            var userProfile =
                _userProfileRepository.Find(
                    Specification<EasyWeixin.Model.UserProfile>.Eval(
                        o => o.UserId == WebSecurity.GetUserId(User.Identity.Name)));
            var buttons = userProfile.Buttons;
            foreach (var button in buttons)
            {
                if (button.SubButtons == null)
                {
                    var responseMessages = button.ResponseMessages.ToList();
                    //返回类型Text (ButtonType == 0)
                    if (responseMessages.Where(o => o.ButtonType == 0).ToList().Count > 0)
                    {
                        var responseMessage = new ResponseMessageText();
                        responseMessage.Content = responseMessages.ElementAt(0).Content;
                    }

                    //返回类型Music (ButtonType == 1)
                    else if (responseMessages.Where(o => o.ButtonType == 1).ToList().Count > 0)
                    {
                        var responseMessage = new ResponseMessageMusic();
                        responseMessage.Music.Title = responseMessages.ElementAt(0).ResponseMusic.MusicName;
                        responseMessage.Music.HQMusicUrl = responseMessages.ElementAt(0).ResponseMusic.HQMusicUrl;
                        responseMessage.Music.MusicUrl = responseMessages.ElementAt(0).ResponseMusic.MusicUrl;
                        responseMessage.Music.Description = "";
                        //   return responseMessage;
                    }
                    //返回类型News (ButtonType == 4)
                    else if (responseMessages.Where(o => o.ButtonType == 4).ToList().Count > 0)
                    {
                        var responseMessage = new ResponseMessageNews();
                        foreach (var item in responseMessages)
                        {
                            responseMessage.Articles.Add(new Article()
                            {
                                //Description = HtmlStringHelper.ClearHTMLString(item.ResponseImageText.Content),
                                //Title = item.ResponseImageText.ImageTextName,
                                //PicUrl = GetImageUrl(item.ResponseImageText.Content),
                                //Url = item.ResponseImageText.Url
                            });
                        }
                    }
                }
                else
                {
                    //var essage = UserProfile.ResponseMessages.Where(o => o.ResponseType == 2).ToList();
                    //return GetEventsResponseMessages(essage, requestMessage);
                    foreach (var Subitem in button.SubButtons.ToList())
                    {
                        var ResponseMessages = Subitem.ResponseMessages.ToList();
                        //返回类型Text (ButtonType == 0)
                        if (ResponseMessages.Where(o => o.ButtonType == 0).ToList().Count > 0)
                        {
                            var responseMessage = new ResponseMessageText();
                            responseMessage.Content = ResponseMessages.ElementAt(0).Content;
                        }

                        //返回类型Music (ButtonType == 1)
                        else if (ResponseMessages.Where(o => o.ButtonType == 1).ToList().Count > 0)
                        {
                            var responseMessage = new ResponseMessageMusic();
                            responseMessage.Music.Title = ResponseMessages.ElementAt(0).ResponseMusic.MusicName;
                            responseMessage.Music.HQMusicUrl = ResponseMessages.ElementAt(0).ResponseMusic.HQMusicUrl;
                            responseMessage.Music.MusicUrl = ResponseMessages.ElementAt(0).ResponseMusic.MusicUrl;
                            responseMessage.Music.Description = "";
                            //   return responseMessage;
                        }
                        //返回类型News (ButtonType == 4)
                        else if (ResponseMessages.Where(o => o.ButtonType == 4).ToList().Count > 0)
                        {
                            var responseMessage = new ResponseMessageNews();
                            foreach (var item in ResponseMessages)
                            {
                                responseMessage.Articles.Add(new Article()
                                {
                                    //Description = HtmlStringHelper.ClearHTMLString(item.ResponseImageText.Content),
                                    //Title = item.ResponseImageText.ImageTextName,
                                    //PicUrl = GetImageUrl(item.ResponseImageText.Content),
                                    //Url = item.ResponseImageText.Url
                                });
                            }
                        }
                    }
                }
            }
            return View();
        }

        #endregion 测试用例
    }
}