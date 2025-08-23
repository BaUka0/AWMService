using AWMService.Application.DTOs;
using KDS.Primitives.FluentResult;
using MediatR;
using System.Collections.Generic;

namespace AWMService.Application.UseCases.PeriodType.Queries.ListPeriodTypes
{
    public sealed record ListPeriodTypesQuery() : IRequest<Result<IReadOnlyList<PeriodTypeDto>>>;
}
