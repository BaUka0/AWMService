using FluentValidation;

namespace AWMService.Application.UseCases.AcademicYears.Queries.GetById
{
    public class GetAcademicYearByIdQueryValidator : AbstractValidator<GetAcademicYearByIdQuery>
    {
        public GetAcademicYearByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0.");
        }
    }
}
