using FluentValidation;

namespace EasyWeixin.Web.Models.Validator
{
    public class ButtonValidator : AbstractValidator<ButtonViewModel>
    {
        public ButtonValidator()
        {
            RuleFor(o => o.name).NotEmpty().WithMessage("** 菜单名称不能为空").NotNull().WithMessage("** 菜单名称不能为空").Length(0, 16).WithMessage("** 按钮在16个字符的范围之内")
             ;
        }
    }
}