using EasyWeixin.Model;
using FluentValidation;

namespace EasyWeixin.Data.Validator
{
    public class SubButtonValidator : AbstractValidator<SubButton>
    {
        public SubButtonValidator()
        {
            RuleFor(o => o.name).NotEmpty().WithMessage("** 不能为空").NotNull().WithMessage("** 不能为空").Length(0, 16).WithMessage("** 字符在16的范围之内");
            //RuleFor(o => o.key).NotEmpty().WithMessage("** 不能为空").NotNull().WithMessage("** 不能为空").Length(0, 200).WithMessage("** 字符在200的范围之内");
        }
    }
}