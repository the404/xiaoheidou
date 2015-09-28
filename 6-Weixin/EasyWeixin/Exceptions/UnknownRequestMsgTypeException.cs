using System;

namespace EasyWeixin.Exceptions
{
    /// <summary>
    /// 未知请求类型。
    /// </summary>
    public class UnknownRequestMsgTypeException : WeixinException //ArgumentOutOfRangeException
    {
        public UnknownRequestMsgTypeException(string message, Exception inner)
            : base(message, inner)
        { }
    }
}