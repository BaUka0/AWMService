using FluentValidation;

namespace AWMService.Application.UseCases.PeriodType.Commands.UpdatePeriodType
{
    public class UpdatePeriodTypeCommandValidator : AbstractValidator<UpdatePeriodTypeCommand>
    {
        public UpdatePeriodTypeCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name cannot be longer than 100 characters.");

            RuleFor(x => x.ActorUserId)
                .GreaterThan(0).WithMessage("ActorUserId must be greater than 0.");
        }
    }
}
