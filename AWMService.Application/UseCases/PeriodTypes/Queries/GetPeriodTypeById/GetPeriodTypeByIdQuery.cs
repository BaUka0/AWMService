using AWMService.Application.DTOs;
using KDS.Primitives.FluentResult;
using MediatR;

namespace AWMService.Application.UseCases.PeriodType.Queries.GetPeriodTypeById
{
    public sealed record GetPeriodTypeByIdQuery(int Id) : IRequest<Result<PeriodTypeDto>>;
}
