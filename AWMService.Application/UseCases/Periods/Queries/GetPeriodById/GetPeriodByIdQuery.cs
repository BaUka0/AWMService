using AWMService.Application.DTOs;
using KDS.Primitives.FluentResult;
using MediatR;

namespace AWMService.Application.UseCases.Periods.Queries.GetPeriodById
{
    public sealed record GetPeriodByIdQuery : IRequest<Result<PeriodDto>>
    {
        public int Id { get; set; }
    }
}
