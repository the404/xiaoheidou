using System;
using System.ComponentModel.DataAnnotations;

namespace EasyWeixin.Web.Models
{
    /// <summary>
    /// 个人信息编辑
    /// </summary>
    public class PersonalInfoViewModel
    {
        public int UserId { get; set; }

        public Guid ID { get; set; }

        [Required(ErrorMessage = " **用户名不能为空")]
        [Display(Name = "用户名")]
        [System.Web.Mvc.Remote("IsExistUserName", "PersonalCenter", ErrorMessage = " **该用户名已经被注册过")]
        public string UserName { get; set; }

        [Required(ErrorMessage = " **QQ号不得为空")]
        [Display(Name = "QQ号码")]
        public string QQ { get; set; }

        [Required(ErrorMessage = " **邮箱地址不能为空")]
        [Display(Name = "邮箱地址")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$", ErrorMessage = " **请输入正确的邮箱格式")]
        [System.Web.Mvc.Remote("IsExistEmail", "PersonalCenter", ErrorMessage = " **该邮箱地址已经被注册过")]
        public string Email { get; set; }

        [Required(ErrorMessage = " **电话号码不能为空")]
        [Display(Name = "电话号码")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"((\d{11})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$)", ErrorMessage = " **请输入正确的电话号码")]
        public string Phone { get; set; }
    }
}