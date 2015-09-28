using Apworks;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Model
{
    /// <summary>
    ///     表示微信普通用户和我们系统内用户的对应关系
    /// </summary>
    public class WeixinUserInUsers : IAggregateRoot
    {
        public int UserId { get; set; }

        public int WeixinUserId { get; set; }

        [Column("UserId"), InverseProperty("WeixinUserInUsers")]
        public UserProfile UserProfile { get; set; }

        [Column("WeixinUserId"), InverseProperty("WeixinUserInUsers")]
        public WeixinUser WeixinUser { get; set; }

        public DateTime AddDate { get; set; }

        public DateTime UpdateDate { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }
    }
}