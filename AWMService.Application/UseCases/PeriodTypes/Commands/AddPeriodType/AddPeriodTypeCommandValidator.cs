using FluentValidation;

namespace AWMService.Application.UseCases.PeriodType.Commands.AddPeriodType
{
    public class AddPeriodTypeCommandValidator : AbstractValidator<AddPeriodTypeCommand>
    {
        public AddPeriodTypeCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name cannot be longer than 100 characters.");

            RuleFor(x => x.ActorUserId)
                .GreaterThan(0).WithMessage("ActorUserId must be greater than 0.");
        }
    }
}
