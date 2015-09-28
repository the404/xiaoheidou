using System;
using System.ComponentModel.DataAnnotations;

namespace EasyWeixin.Web.Models
{
    public class PersonalWeiXinViewModel
    {
        public int UserId { get; set; }

        public Guid ID { get; set; }

        public string UserCode { get; set; }

        public string Alt { get; set; }

        public string WeixinToken { get; set; }

        public string openid { get; set; }  /// 普通用户的标识，对当前公众号唯一

        [Required(ErrorMessage = " **AppId不能为空")]
        [Display(Name = "AppId")]
        [System.Web.Mvc.Remote("IsExistAppId", "PersonalCenter", ErrorMessage = " **该AppId已经被注册过")]
        public string AppId { get; set; }

        [Required(ErrorMessage = " **AppSecret不能为空")]
        [Display(Name = "AppSecret")]
        [System.Web.Mvc.Remote("IsExistAppSecret", "PersonalCenter", ErrorMessage = " **该AppSecret已经被注册过")]
        public string AppSecret { get; set; }

        [Required(ErrorMessage = " **微信账号不能为空")]
        [Display(Name = "微信账号")]
        public string WeixinUser { get; set; }

        [Required(ErrorMessage = " **微信密码不能为空")]
        [Display(Name = "微信密码")]
        public string Md5Password { get; set; }
    }
}