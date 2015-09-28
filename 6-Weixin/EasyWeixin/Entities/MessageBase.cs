using System;

namespace EasyWeixin.Entities
{
    //Senparc.Weixin.MP
    public interface IMessageBase
    {
        /// <summary>
        /// 服务号
        /// </summary>
        string ToUserName { get; set; }

        /// <summary>
        /// 就是openid
        /// </summary>
        string FromUserName { get; set; }

        DateTime CreateTime { get; set; }
    }

    /// <summary>
    /// 所有Request和Response消息的基类
    /// </summary>
    public class MessageBase
    {
        public string ToUserName { get; set; }

        public string FromUserName { get; set; }

        public DateTime CreateTime { get; set; }
    }
}