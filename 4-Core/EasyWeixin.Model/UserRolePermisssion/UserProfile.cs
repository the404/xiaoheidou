using Apworks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyWeixin.Model
{
    [Table("UserProfile")]
    public class UserProfile : IAggregateRoot
    {
        #region Ctor

        public UserProfile()
        {
            WeixinUserInUsers = new List<WeixinUserInUsers>();
        }

        #endregion Ctor

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [StringLength(250)]
        public string UserName { get; set; }

        [StringLength(250)]
        public string Email { get; set; }

        public DateTime? DateCreated { get; set; }

        [StringLength(50)]
        public string QQ { get; set; } //电话

        [StringLength(50)]
        public string Phone { get; set; } //电话

        public int? UserTypeID { get; set; } //用户类型 ?

        public int? LoginType { get; set; }

        [StringLength(100)]
        public string UserPhoto { get; set; } //用户头像

        public string CompanyName { get; set; } //用户公司名称

        public string UserCode { get; set; }

        public string Alt { get; set; }

        public string WeixinToken { get; set; }

        public string openid { get; set; }

        /// 普通用户的标识，对当前公众号唯一
        public string AppId { get; set; }

        public string AppSecret { get; set; }

        public string WeixinUser { get; set; }

        public string Md5Password { get; set; }

        public string Header { get; set; }

        public DateTime InDate { get; set; }

        public virtual ICollection<Button> Buttons { get; set; }

        public virtual ICollection<ResponseMessage> ResponseMessages { get; set; }

        public virtual ICollection<ResponseImage> ResponseImages { get; set; }

        public virtual ICollection<ResponseImageText> ResponseImageTexts { get; set; }

        public virtual ICollection<ResponseKey> ResponseKeys { get; set; }

        public virtual ICollection<ResponseMusic> ResponseMusics { get; set; }

        public virtual ICollection<ResponseVideo> ResponseVideos { get; set; }

        public virtual ICollection<GuessUser> GuessUsers { get; set; }

        public virtual ICollection<WeixinUserInUsers> WeixinUserInUsers { get; set; }

        public virtual ICollection<QrCode> QrCodes { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }
    }
}