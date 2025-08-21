using FluentValidation;

namespace AWMService.Application.UseCases.Supervisors.Commands.RevokeSupervisors
{
    public class RevokeSupervisorsCommandValidator : AbstractValidator<RevokeSupervisorsCommand>
    {
        public RevokeSupervisorsCommandValidator()
        {
            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage("DepartmentId must be greater than 0.");

            RuleFor(x => x.AcademicYearId)
                .GreaterThan(0).WithMessage("AcademicYearId must be greater than 0.");

            RuleFor(x => x.UserIds)
                .NotEmpty().WithMessage("UserIds list cannot be empty.");

            RuleFor(x => x.ActorUserId)
                .GreaterThan(0).WithMessage("ActorUserId must be greater than 0.");
        }
    }
}