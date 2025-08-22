using FluentValidation;

namespace AWMService.Application.UseCases.Periods.Queries.GetPeriodById
{
    public class GetPeriodByIdQueryValidator : AbstractValidator<GetPeriodByIdQuery>
    {
        public GetPeriodByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0.");
        }
    }
}
