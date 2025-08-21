using FluentValidation;

namespace AWMService.Application.UseCases.Roles.Commands.AssignRoleToUser
{
    public class AssignRoleToUserCommandValidator : AbstractValidator<AssignRoleToUserCommand>
    {
        public AssignRoleToUserCommandValidator()
        {
            RuleFor(x => x.RoleId)
                .GreaterThan(0).WithMessage("RoleId must be greater than 0.");

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("UserId must be greater than 0.");

            RuleFor(x => x.ActorUserId)
                .GreaterThan(0).WithMessage("ActorUserId must be greater than 0.");
        }
    }
}