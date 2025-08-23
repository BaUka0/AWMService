using FluentValidation;

namespace AWMService.Application.UseCases.Periods.Commands.AddPeriod
{
    public class AddPeriodCommandValidator : AbstractValidator<AddPeriodCommand>
    {
        public AddPeriodCommandValidator()
        {
            RuleFor(x => x.PeriodTypeId)
                .GreaterThan(0).WithMessage("PeriodTypeId must be greater than 0.");
            RuleFor(x => x.AcademicYearId)
                .GreaterThan(0).WithMessage("AcademicYearId must be greater than 0.");
            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("StartDate is required.");
            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("EndDate is required.")
                .GreaterThan(x => x.StartDate).WithMessage("EndDate must be after StartDate.");
            RuleFor(x => x.StatusId)
                .GreaterThan(0).WithMessage("StatusId must be greater than 0.");
            RuleFor(x => x.ActorUserId)
                .GreaterThan(0).WithMessage("ActorUserId must be greater than 0.");
        }
    }
}
