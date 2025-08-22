using AWMService.Application.Abstractions.Repositories;
using AWMService.Application.DTOs;
using AWMService.Domain.Constants;
using KDS.Primitives.FluentResult;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AWMService.Application.UseCases.Periods.Queries.GetPeriodById
{
    public class GetPeriodByIdQueryHandler(IPeriodsRepository periodsRepository, ILogger<GetPeriodByIdQueryHandler> logger) : IRequestHandler<GetPeriodByIdQuery, Result<PeriodDto>>
    {
        public async Task<Result<PeriodDto>> Handle(GetPeriodByIdQuery request, CancellationToken ct)
        {
            logger.LogInformation("Attempting to get period with ID {PeriodId}", request.Id);

            var period = await periodsRepository.GetByIdAsync(request.Id, ct);

            if (period == null)
            {
                logger.LogWarning("Period with ID {PeriodId} not found", request.Id);
                return Result.Failure<PeriodDto>(new Error(ErrorCode.NotFound, "Period not found"));
            }

            var periodDto = new PeriodDto(period.Id, period.PeriodTypeId, period.PeriodType.Name, period.AcademicYearId, period.AcademicYear.YearName, period.StartDate, period.EndDate, period.StatusId, period.Status.Name);

            logger.LogInformation("Successfully retrieved period with ID {PeriodId}", request.Id);
            return Result.Success(periodDto);
        }
    }
}
