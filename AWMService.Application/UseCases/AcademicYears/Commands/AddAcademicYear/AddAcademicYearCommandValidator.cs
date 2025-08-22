using FluentValidation;
using System;

namespace AWMService.Application.UseCases.AcademicYears.Commands.AddAcademicYear
{
    public class AddAcademicYearCommandValidator : AbstractValidator<AddAcademicYearCommand>
    {
        public AddAcademicYearCommandValidator()
        {
            RuleFor(x => x.YearName)
                .NotEmpty().WithMessage("YearName is required.")
                .MaximumLength(50).WithMessage("YearName cannot be longer than 50 characters.");

            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate).WithMessage("EndDate must be greater than StartDate.");

            RuleFor(x => x.ActorUserId)
                .GreaterThan(0).WithMessage("ActorUserId must be greater than 0.");
        }
    }
}
