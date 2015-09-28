using Apworks;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Model
{
    /// <summary>
    /// 最好是只存储临时的二维码
    /// </summary>
    public class QrCode : IAggregateRoot
    {
        public QrCode()
        {
            AddTime = DateTime.Now;
            IsWeixinSend = false;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        /// <summary>
        /// 在系统中唯一
        /// 场景值ID，临时二维码时为32位非0整型，永久二维码时最大值为100000（目前参数只支持1--100000）
        /// </summary>
        public string SceneId { get; set; }

        public OrCodeType Type { get; set; }

        public int AwardNum { get; set; }

        [ForeignKey("UserProfile")]
        public int UserId { get; set; }

        public string QrCodeUrl { get; set; }

        public DateTime AddTime { get; set; }
        /// <summary>
        /// 过期时间(创建时间+二维码的过期时间),过期数据是可以删除的,所以需要另外的东西来存储积分系统
        /// </summary>
        public DateTime ExpireTime { get; set; }

        /// <summary>
        /// 为了确保微信中只发送一条提示数据的标识
        /// </summary>
        public bool IsWeixinSend { get; set; }

        /// <summary>
        /// 存储扫描二维码的微信用户的openId,用于推送消息
        /// </summary>
        public string OpenId { get; set; }

        public virtual UserProfile UserProfile { get; set; }
    }

    public enum OrCodeType
    {
        Award = 12
    }
}