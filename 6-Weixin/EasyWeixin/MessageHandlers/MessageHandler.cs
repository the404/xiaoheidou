using EasyWeixin.Context;
using EasyWeixin.Entities.Request;
using EasyWeixin.Entities.Request.Events;
using EasyWeixin.Entities.Response;
using EasyWeixin.Exceptions;
using EasyWeixin.Helpers;
using Senparc.Weixin.MP.Entities;
using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace EasyWeixin.MessageHandlers
{/// <summary>
 /// 微信请求的集中处理方法
 /// 此方法中所有过程，都基于Senparc.Weixin.MP的基础功能，只为简化代码而设。
 /// </summary>
    public abstract class MessageHandler<TC> : IMessageHandler where TC : class, IMessageContext, new()
    {
        #region 属性(常用的就是WeixinOPenId,ResponseMessage,RequestMessage)

        /// <summary>
        /// 上下文
        /// </summary>
        public static WeixinContext<TC> GlobalWeixinContext = new WeixinContext<TC>();

        /// <summary>
        /// 全局消息上下文
        /// </summary>
        public WeixinContext<TC> WeixinContext
        {
            get { return GlobalWeixinContext; }
        }

        /// <summary>
        /// 当前用户消息上下文
        /// </summary>
        public TC CurrentMessageContext
        {
            get { return WeixinContext.GetMessageContext(RequestMessage); }
        }

        /// <summary>
        /// 发送者用户名（OpenId）
        /// </summary>
        public string WeixinOpenId
        {
            get
            {
                if (RequestMessage != null)
                {
                    return RequestMessage.FromUserName;
                }
                return null;
            }
        }

        /// <summary>
        ///
        /// </summary>
        [Obsolete("UserName属性从v0.6起已过期，建议使用WeixinOpenId")]
        public string UserName
        {
            get
            {
                return WeixinOpenId;
            }
        }

        /// <summary>
        /// 取消执行Execute()方法。一般在OnExecuting()中用于临时阻止执行Execute()。
        /// 默认为False。
        /// 如果在执行OnExecuting()执行前设为True，则所有OnExecuting()、Execute()、OnExecuted()代码都不会被执行。
        /// 如果在执行OnExecuting()执行过程中设为True，则后续Execute()及OnExecuted()代码不会被执行。
        /// </summary>
        public bool CancelExcute { get; set; }

        /// <summary>
        /// 在构造函数中转换得到原始XML数据
        /// </summary>
        public XDocument RequestDocument { get; set; }

        /// <summary>
        /// 根据ResponseMessageBase获得转换后的ResponseDocument
        /// 注意：这里每次请求都会根据当前的ResponseMessageBase生成一次，如需重用此数据，建议使用缓存或局部变量
        /// </summary>
        public XDocument ResponseDocument
        {
            get
            {
                if (ResponseMessage == null)
                {
                    return null;
                }
                return (ResponseMessage as ResponseMessageBase).ConvertEntityToXml();
            }
        }

        //protected Stream InputStream { get; set; }
        /// <summary>
        /// 请求实体
        /// </summary>
        public IRequestMessageBase RequestMessage { get; set; }

        /// <summary>
        /// 响应实体
        /// 只有当执行Execute()方法后才可能有值
        /// </summary>
        public IResponseMessageBase ResponseMessage { get; set; }

        #endregion 属性(常用的就是WeixinOPenId,ResponseMessage,RequestMessage)

        #region 构造函数

        public MessageHandler(Stream inputStream)
        {
            using (XmlReader xr = XmlReader.Create(inputStream))
            {
                RequestDocument = XDocument.Load(xr);
                Init(RequestDocument);
            }
        }

        public MessageHandler(XDocument requestDocument)
        {
            Init(requestDocument);
        }

        private void Init(XDocument requestDocument)
        {
            RequestDocument = requestDocument;
            //将多种xml文档解析成同一个表达的对象
            RequestMessage = RequestMessageFactory.GetRequestEntity(RequestDocument);

            //记录上下文
            if (WeixinContextGlobal.UseWeixinContext)
            {
                WeixinContext.InsertMessage(RequestMessage);
            }
        }

        #endregion 构造函数

        #region 一个可以得到不同类型的ResponseMessage的方法(实现中是反射+工厂)

        /// <summary>
        /// 根据当前的RequestMessage创建指定类型的ResponseMessage
        /// </summary>
        /// <typeparam name="TR">基于ResponseMessageBase的响应消息类型</typeparam>
        /// <returns></returns>
        public TR CreateResponseMessage<TR>() where TR : ResponseMessageBase
        {
            if (RequestMessage == null)
            {
                return null;
            }

            return RequestMessage.CreateResponseMessage<TR>();
        }

        #endregion 一个可以得到不同类型的ResponseMessage的方法(实现中是反射+工厂)

        #region Execute():对构造函数处理后的RequestMessage做一次根本性的处理

        /// <summary>
        /// 执行微信请求
        /// </summary>
        public void Execute()
        {
            if (CancelExcute)
            {
                return;
            }

            OnExecuting(RequestMessage);

            if (CancelExcute)
            {
                return;
            }

            try
            {
                if (RequestMessage == null)
                {
                    return;
                }
                switch (RequestMessage.MsgType)
                {
                    case RequestMsgType.Text:
                        ResponseMessage = OnTextRequest(RequestMessage as RequestMessageText);
                        break;

                    case RequestMsgType.Location:
                        ResponseMessage = OnLocationRequest(RequestMessage as RequestMessageLocation);
                        break;

                    case RequestMsgType.Image:
                        ResponseMessage = OnImageRequest(RequestMessage as RequestMessageImage);
                        break;

                    case RequestMsgType.Voice:
                        ResponseMessage = OnVoiceRequest(RequestMessage as RequestMessageVoice);
                        break;

                    case RequestMsgType.Event:
                        ResponseMessage = OnEventRequest((IRequestMessageEventBase)RequestMessage);
                        break;

                    default:
                        throw new UnknownRequestMsgTypeException("未知的MsgType请求类型", null);
                }

                //记录上下文
                //if (WeixinContextGlobal.UseWeixinContext && ResponseMessage != null)
                //{
                //    WeixinContext.InsertMessage(ResponseMessage);
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                OnExecuted(RequestMessage, ResponseMessage);
            }
        }

        #endregion Execute():对构造函数处理后的RequestMessage做一次根本性的处理

        #region 子类可重写(或必须重写)的方法

        public virtual void OnExecuting(IRequestMessageBase requestMessage = null)
        {
        }

        public virtual void OnExecuted(IRequestMessageBase requestMessage = null, IResponseMessageBase repMessage = null)
        {
        }

        /// <summary>
        /// 默认返回消息（当任何OnXX消息没有被重写，都将自动返回此默认消息）
        /// </summary>
        public abstract IResponseMessageBase DefaultResponseMessage(IRequestMessageBase requestMessage);

        public abstract IResponseMessageBase DefaultResponseMessage(string content = "");

        /// <summary>
        /// 文字类型请求
        /// </summary>
        public virtual IResponseMessageBase OnTextRequest(RequestMessageText requestMessage)
        {
            return DefaultResponseMessage(requestMessage);
        }

        /// <summary>
        /// 位置类型请求
        /// </summary>
        public virtual IResponseMessageBase OnLocationRequest(RequestMessageLocation requestMessage)
        {
            return DefaultResponseMessage(requestMessage);
        }

        /// <summary>
        /// 图片类型请求
        /// </summary>
        public virtual IResponseMessageBase OnImageRequest(RequestMessageImage requestMessage)
        {
            return DefaultResponseMessage(requestMessage);
        }

        /// <summary>
        /// 语音类型请求
        /// </summary>
        public virtual IResponseMessageBase OnVoiceRequest(RequestMessageVoice requestMessage)
        {
            return DefaultResponseMessage(requestMessage);
        }

        /// <summary>
        /// 链接消息类型请求
        /// </summary>
        public virtual IResponseMessageBase OnLinkRequest(RequestMessageLink requestMessage)
        {
            return DefaultResponseMessage(requestMessage);
        }

        /// <summary>
        /// Event事件类型请求
        /// </summary>
        public virtual IResponseMessageBase OnEventRequest(IRequestMessageEventBase requestMessage)
        {
            if (requestMessage == null) throw new ArgumentNullException("requestMessage", "MessageHandler line 282");
            IResponseMessageBase responseMessage = null;
            switch (requestMessage.Event)
            {
                case Event.ENTER:
                    responseMessage = OnEvent_EnterRequest(requestMessage);
                    break;

                case Event.LOCATION:
                    responseMessage = OnEvent_LocationRequest(requestMessage); //目前实际无效
                    break;

                case Event.subscribe: //订阅
                    responseMessage = OnEvent_SubscribeRequest(requestMessage);
                    break;

                case Event.unsubscribe: //退订
                    responseMessage = OnEvent_UnsubscribeRequest(requestMessage);
                    break;

                case Event.CLICK:
                    responseMessage = OnEvent_ClickRequest(requestMessage);
                    break;

                case Event.VIEW://URL跳转（view视图）
                    responseMessage = OnEvent_ViewRequest(RequestMessage as RequestMessageEvent_View);
                    break;

                case Event.SCAN: //二维码
                    responseMessage = OnEvent_ScanRequest(requestMessage);
                    break;
                    //default:
                    //    throw new UnknownRequestMsgTypeException(string.Format("{0},未知的Event下属请求信息", requestMessage.Event), null);
            }
            return responseMessage;
        }

        #region Event 下属分类 这些事件都是返回了默认"敬请期待"消息

        private IResponseMessageBase OnEvent_ViewRequest(RequestMessageEvent_View requestMessageEvent_View)
        {
            return DefaultResponseMessage();
        }

        /// <summary>
        /// Event事件类型请求之ENTER
        /// </summary>
        public virtual IResponseMessageBase OnEvent_EnterRequest(IRequestMessageEventBase requestMessage)
        {
            return DefaultResponseMessage(requestMessage);
        }

        /// <summary>
        /// Event事件类型请求之LOCATION
        /// </summary>
        public virtual IResponseMessageBase OnEvent_LocationRequest(IRequestMessageEventBase requestMessage)
        {
            return DefaultResponseMessage(requestMessage);
        }

        /// <summary>
        /// Event事件类型请求之subscribe
        /// </summary>
        public virtual IResponseMessageBase OnEvent_SubscribeRequest(IRequestMessageEventBase requestMessage)
        {
            return DefaultResponseMessage(requestMessage);
        }

        /// <summary>
        /// Event事件类型请求之unsubscribe
        /// </summary>
        public virtual IResponseMessageBase OnEvent_UnsubscribeRequest(IRequestMessageEventBase requestMessage)
        {
            return DefaultResponseMessage(requestMessage);
        }

        /// <summary>
        /// Event事件类型请求之CLICK
        /// </summary>
        public virtual IResponseMessageBase OnEvent_ClickRequest(IRequestMessageEventBase requestMessage)
        {
            return DefaultResponseMessage(requestMessage);
        }

        /// <summary>
        /// Event事件之扫描二维码
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public virtual IResponseMessageBase OnEvent_ScanRequest(IRequestMessageEventBase requestMessage)
        {
            return DefaultResponseMessage(requestMessage);
        }

        #endregion Event 下属分类 这些事件都是返回了默认"敬请期待"消息

        #endregion 子类可重写(或必须重写)的方法
    }
}