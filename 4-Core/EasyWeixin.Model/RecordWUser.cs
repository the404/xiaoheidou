using Apworks;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Model
{
    public class RecordWUser : IAggregateRoot
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ReID { get; set; }

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }

        public string ToUserName { get; set; } //开发者微信号

        public string FromUserName { get; set; }  //发送方帐号（一个OpenID）

        public string HeadimgUrl { get; set; }

        public int sex { get; set; }

        public string NickName { get; set; }
    }
}