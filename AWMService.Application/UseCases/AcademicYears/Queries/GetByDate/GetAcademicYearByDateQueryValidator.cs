using FluentValidation;
using System;

namespace AWMService.Application.UseCases.AcademicYears.Queries.GetByDate
{
    public class GetAcademicYearByDateQueryValidator : AbstractValidator<GetAcademicYearByDateQuery>
    {
        public GetAcademicYearByDateQueryValidator()
        {
            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("Date is required and cannot be the default value.");
        }
    }
}
