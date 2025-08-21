using FluentValidation;

namespace AWMService.Application.UseCases.PeriodType.Commands.DeletePeriodType
{
    public class DeletePeriodTypeCommandValidator : AbstractValidator<DeletePeriodTypeCommand>
    {
        public DeletePeriodTypeCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0.");

            RuleFor(x => x.ActorUserId)
                .GreaterThan(0).WithMessage("ActorUserId must be greater than 0.");
        }
    }
}
