using EasyWeixin.Exceptions;
using EasyWeixin.MessageHandlers;
using System.Web.Mvc;

namespace EasyWeixin.MvcExtension.Results
{
    public class WeixinResult : ContentResult
    {
        //private string _content;
        private IMessageHandler _messageHandler;

        public WeixinResult(string content)
        {
            //_content = content;
            base.Content = content;
        }

        public WeixinResult(IMessageHandler messageHandler)
        {
            _messageHandler = messageHandler;
        }

        /// <summary>
        /// 获取ContentResult中的Content或IMessageHandler中的ResponseDocument文本结果。
        /// 一般在测试的时候使用。
        /// </summary>
        new public string Content
        {
            get
            {
                if (base.Content != null)
                {
                    return base.Content;
                }
                else if (_messageHandler != null)
                {
                    return _messageHandler.ResponseDocument.ToString();
                }
                else
                {
                    return null;
                }
            }
            set { base.Content = value; }
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (base.Content == null)
            {
                //使用IMessageHandler输出
                if (_messageHandler == null)
                {
                    throw new WeixinException("执行WeixinResult时提供的MessageHandler不能为Null！", null);
                }

                if (_messageHandler.ResponseMessage == null)
                {
                    throw new WeixinException("ResponseMessage不能为Null！", null);
                }

                context.HttpContext.Response.ClearContent();
                context.HttpContext.Response.ContentType = "text/xml";
                _messageHandler.ResponseDocument.Save(context.HttpContext.Response.OutputStream);
            }

            base.ExecuteResult(context);
        }
    }
}