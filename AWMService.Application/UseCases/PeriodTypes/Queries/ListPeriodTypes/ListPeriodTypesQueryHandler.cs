using AWMService.Application.Abstractions.Repositories;
using AWMService.Application.DTOs;
using KDS.Primitives.FluentResult;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AWMService.Application.UseCases.PeriodType.Queries.ListPeriodTypes
{
    public sealed class ListPeriodTypesQueryHandler(
        IPeriodTypesRepository periodTypesRepository,
        ILogger<ListPeriodTypesQueryHandler> logger)
        : IRequestHandler<ListPeriodTypesQuery, Result<IReadOnlyList<PeriodTypeDto>>>
    {
        public async Task<Result<IReadOnlyList<PeriodTypeDto>>> Handle(
            ListPeriodTypesQuery request, CancellationToken ct)
        {
            logger.LogInformation("Attempting to list all period types.");
            var items = await periodTypesRepository.ListAllAsync(ct);

            var dtos = items
                .Select(x => new PeriodTypeDto(x.Id, x.Name))
                .ToList()
                .AsReadOnly();
            
            logger.LogInformation("Successfully retrieved {Count} period types.", dtos.Count);
            return dtos;
        }
    }
}
