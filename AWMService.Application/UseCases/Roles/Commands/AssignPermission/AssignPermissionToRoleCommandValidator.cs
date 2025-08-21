using FluentValidation;

namespace AWMService.Application.UseCases.Roles.Commands.AssignPermission
{
    public class AssignPermissionToRoleCommandValidator : AbstractValidator<AssignPermissionToRoleCommand>
    {
        public AssignPermissionToRoleCommandValidator()
        {
            RuleFor(x => x.RoleId)
                .GreaterThan(0).WithMessage("RoleId must be greater than 0.");

            RuleFor(x => x.PermissionId)
                .GreaterThan(0).WithMessage("PermissionId must be greater than 0.");

            RuleFor(x => x.ActorUserId)
                .GreaterThan(0).WithMessage("ActorUserId must be greater than 0.");
        }
    }
}