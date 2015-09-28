using System.ComponentModel.DataAnnotations;

namespace EasyWeixin.Web.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "邮件地址")]
        [RegularExpression(@"^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$", ErrorMessage = " **请输入正确的邮箱格式")]
        public string Email { get; set; }
    }
}