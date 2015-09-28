using EasyWeixin.Model;
using FluentValidation;

namespace EasyWeixin.Data.Validator
{
    public class RoleValidator : AbstractValidator<Role>
    {
        public RoleValidator()
        {
            RuleFor(o => o.RoleChineseName).NotEmpty().WithMessage("** 角色名不能为空").NotNull().WithMessage("** 角色名不能为空").Length(0, 200).WithMessage("** 角色名字符在200的范围之内");
            RuleFor(o => o.RoleName).NotEmpty().WithMessage("** 角色路径不能为空").NotNull().WithMessage("** 角色路径不能为空").Length(0, 200).WithMessage("** 角色路径字符在200的范围之内");
            RuleFor(o => o.RoleImageName).NotEmpty().WithMessage("** 角色图片不能为空").NotNull().WithMessage("** 角色图片不能为空").Length(0, 200).WithMessage("** 角色图片字符在200的范围之内");
            RuleFor(o => o.RoleDesc).Length(0, 300).WithMessage("** 角色描述字符在300的范围之内");
        }
    }
}