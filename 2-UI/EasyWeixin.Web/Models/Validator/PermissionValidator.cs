using FluentValidation;

namespace EasyWeixin.Web.Models.Validator
{
    public class PermissionValidator : AbstractValidator<PermissionViewModel>
    {
        public PermissionValidator()
        {
            RuleFor(o => o.PermissionName).NotEmpty().WithMessage("** 权限路径不能为空").NotNull().WithMessage("** 权限路径不能为空").Length(0, 20).WithMessage("** 权限路径字符在200的范围之内")
             ;
            RuleFor(o => o.PermissionChineseName).NotEmpty().WithMessage("** 权限名称不能为空").NotNull().WithMessage("** 权限名称不能为空").Length(0, 20).WithMessage("** 权限名称字符在200的范围之内")

             .WithName("权限名称")
             ;
        }
    }
}