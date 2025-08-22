using FluentValidation;

namespace AWMService.Application.UseCases.Periods.Commands.DeletePeriod
{
    public class DeletePeriodCommandValidator : AbstractValidator<DeletePeriodCommand>
    {
        public DeletePeriodCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0.");
            RuleFor(x => x.ActorUserId)
                .GreaterThan(0).WithMessage("ActorUserId must be greater than 0.");
        }
    }
}
