using System.ComponentModel.DataAnnotations;

namespace EasyWeixin.Web.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = " **用户名不能为空")]
        [Display(Name = "用户名")]
        [System.Web.Mvc.Remote("IsExistUserName", "Admin", ErrorMessage = " **该用户名已经被注册过")]
        public string UserName { get; set; }

        [Required(ErrorMessage = " **密码不能为空")]
        [StringLength(100, ErrorMessage = " **{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = " **密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = " **QQ号不得为空")]
        [Display(Name = "QQ号码")]
        public string QQ { get; set; }

        [Required(ErrorMessage = " **邮箱地址不能为空")]
        [Display(Name = "邮箱地址")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$", ErrorMessage = " **请输入正确的邮箱格式")]
        [System.Web.Mvc.Remote("IsExistEmail", "Admin", ErrorMessage = " **该邮箱地址已经被注册过")]
        public string Email { get; set; }

        [Required(ErrorMessage = " **电话号码不能为空")]
        [Display(Name = "电话号码")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"((\d{11})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$)", ErrorMessage = " **请输入正确的电话号码")]
        public string Phone { get; set; }
    }
}