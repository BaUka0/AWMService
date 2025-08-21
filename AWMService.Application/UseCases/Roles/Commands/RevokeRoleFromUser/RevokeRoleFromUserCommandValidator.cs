using FluentValidation;

namespace AWMService.Application.UseCases.Roles.Commands.RevokeRoleFromUser
{
    public class RevokeRoleFromUserCommandValidator : AbstractValidator<RevokeRoleFromUserCommand>
    {
        public RevokeRoleFromUserCommandValidator()
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