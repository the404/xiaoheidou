using EasyWeixin.Model;
using FluentValidation;

namespace EasyWeixin.Data.Validator
{
    public class ButtonValidator : AbstractValidator<Button>
    {
        public ButtonValidator()
        {
            RuleFor(o => o.name).Length(0, 16).WithMessage("** 按钮在16个字符的范围之内")
              ;
        }
    }
}