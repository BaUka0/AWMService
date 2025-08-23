using FluentValidation;

namespace AWMService.Application.UseCases.PeriodType.Queries.GetPeriodTypeById
{
    public class GetPeriodTypeByIdQueryValidator : AbstractValidator<GetPeriodTypeByIdQuery>
    {
        public GetPeriodTypeByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0.");
        }
    }
}
