using EasyWeixin.Entities.Request.Events;
using EasyWeixin.Entities.Response;
using Microsoft.AspNet.SignalR.Client;
using Senparc.Weixin.MP.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace EasyWeixin.Web.Framework.CommonService.CustomMessageHandler
{
    /// <summary>
    ///     自定义MessageHandler
    /// </summary>
    public partial class CustomMessageHandler
    {
        #region 重写父类事件处理方法

        public override IResponseMessageBase OnEvent_ClickRequest(IRequestMessageEventBase requestMessage)
        {
            var buttons = UserProfile.Buttons;
            //var fileVersionInfo =
            //    FileVersionInfo.GetVersionInfo(HttpContext.Current.Server.MapPath("~/bin/EasyWeixin.dll"));
            foreach (var button in buttons)
            {
                if (button.SubButtons.ToList().Count == 0)
                {
                    if (button.key == requestMessage.EventKey)
                    {
                        var responseMessages = button.ResponseMessages;
                        return GetEventsResponseMessages(responseMessages.ToList(), requestMessage);
                    }
                }
                else
                {
                    foreach (var subitem in button.SubButtons)
                    {
                        if (subitem.key == requestMessage.EventKey)
                        {
                            var responseMessages = subitem.ResponseMessages;
                            return GetEventsResponseMessages(responseMessages.ToList(), requestMessage);
                        }
                    }
                }
            }
            var rMessage = UserProfile.ResponseMessages.Where(o => o.ResponseType == 2).ToList();
            return GetEventsResponseMessages(rMessage, requestMessage);
        }

        public override IResponseMessageBase OnEvent_EnterRequest(IRequestMessageEventBase requestMessage)
        {
            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            responseMessage.Content = "您刚才发送了ENTER事件请求。";
            return responseMessage;
        }

        public override IResponseMessageBase OnEvent_LocationRequest(IRequestMessageEventBase requestMessage)
        {
            throw new Exception("暂不可用");
        }

        public override IResponseMessageBase OnEvent_SubscribeRequest(IRequestMessageEventBase requestMessage)
        {
            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);

            #region 如果是扫描二维码,未关注
            ResponseQrCode(requestMessage, ref responseMessage);
            #endregion

            var message = UserProfile.ResponseMessages.Where(o => o.ResponseType == 1).ToList();
            return GetEventsResponseMessages(message, requestMessage);
        }

        public override IResponseMessageBase OnEvent_UnsubscribeRequest(IRequestMessageEventBase requestMessage)
        {
            var responseMessage = ResponseMessageBase.CreateFromRequestMessage<ResponseMessageText>(requestMessage);
            responseMessage.Content = "";
            return responseMessage;
        }

        public override IResponseMessageBase OnEvent_ScanRequest(IRequestMessageEventBase requestMessage)
        {
            //应该是什么都没有返回,然后只发送一个模板消息
            var requestMessageScan = (RequestMessageEvent_Scan)requestMessage;

            ResponseMessageText responseMessage = CreateResponseMessage<ResponseMessageText>(); ;
            ResponseQrCode(requestMessage, ref responseMessage);

            return responseMessage;
        }

        #endregion 重写父类事件处理方法

        private void ResponseQrCode(IRequestMessageEventBase requestMessage, ref ResponseMessageText responseMessage)
        {
            #region
            //Dictionary<string, string> qs = new Dictionary<string, string>();
            //qs.Add("groupId", "handler1");

            //HubConnection hubConnection = new HubConnection("http://" + System.Web.HttpContext.Current.Request.Url.Authority + "/", qs);
            //IHubProxy testHubProxy = hubConnection.CreateHubProxy("TestHub");

            //ServicePointManager.DefaultConnectionLimit = 10;
            //hubConnection.Start().ContinueWith(task =>
            //{
            //    testHubProxy.Invoke("hello", RequestMessage.FromUserName);
            //});
            #endregion

            var qrcode = WeixinHelper.GetQrCode(requestMessage.EventKey);

            if (qrcode == null)
            {
                responseMessage.Content = "未找到,请刷新页面";
                return;
            }

            if (qrcode.IsWeixinSend)
            {
                responseMessage.Content = "二维码已过期,刷新电脑页面重试";
            }
            else
            {
                var eventKey = requestMessage.EventKey;
                var openId = requestMessage.FromUserName;

                WeixinHelper.UpdateQrCodeStatu(eventKey, openId);
            }
        }
    }
}