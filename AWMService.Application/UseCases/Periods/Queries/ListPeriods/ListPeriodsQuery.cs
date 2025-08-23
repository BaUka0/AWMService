using AWMService.Application.DTOs;
using KDS.Primitives.FluentResult;
using MediatR;

namespace AWMService.Application.UseCases.Periods.Queries.ListPeriods
{
    public sealed record ListPeriodsQuery : IRequest<Result<IReadOnlyList<PeriodDto>>>
    {
    }
}
