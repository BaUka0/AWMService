using AWMService.Application.Abstractions.Repositories;
using AWMService.Application.DTOs;
using KDS.Primitives.FluentResult;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AWMService.Application.UseCases.Periods.Queries.ListPeriods
{
    public class ListPeriodsQueryHandler(IPeriodsRepository periodsRepository, ILogger<ListPeriodsQueryHandler> logger) : IRequestHandler<ListPeriodsQuery, Result<IReadOnlyList<PeriodDto>>>
    {
        public async Task<Result<IReadOnlyList<PeriodDto>>> Handle(ListPeriodsQuery request, CancellationToken ct)
        {
            logger.LogInformation("Attempting to list all periods.");

            var periods = await periodsRepository.GetAllAsync(ct);

            var periodDtos = periods.Select(p => new PeriodDto(p.Id, p.PeriodTypeId, p.PeriodType.Name, p.AcademicYearId, p.AcademicYear.YearName, p.StartDate, p.EndDate, p.StatusId, p.Status.Name)).ToList();

            logger.LogInformation("Successfully retrieved {Count} periods.", periodDtos.Count);
            return Result.Success<IReadOnlyList<PeriodDto>>(periodDtos);
        }
    }
}
