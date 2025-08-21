using AWMService.Application.Abstractions.Repositories;
using AWMService.Application.DTOs;
using AWMService.Domain.Constants;
using KDS.Primitives.FluentResult;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AWMService.Application.UseCases.AcademicYears.Queries.GetByDate
{
    public sealed class GetAcademicYearByDateQueryHandler(
        IAcademicYearsRepository academicYearsRepository,
        ILogger<GetAcademicYearByDateQueryHandler> logger)
                : IRequestHandler<GetAcademicYearByDateQuery, Result<AcademicYearDto>>
    {
        public async Task<Result<AcademicYearDto>> Handle(GetAcademicYearByDateQuery request, CancellationToken ct)
        {
            using var scope = logger.BeginScope(new Dictionary<string, object> { ["Date"] = request.Date });
            logger.LogInformation("Attempting to get academic year by date: {Date}", request.Date);

            var e = await academicYearsRepository.GetAcademicYearsByDateAsync(request.Date, ct);
            if (e is null)
            {
                logger.LogWarning("Academic year for date {Date} not found.", request.Date);
                return Result.Failure<AcademicYearDto>(new Error(ErrorCode.NotFound, "Учебный год для указанной даты не найден."));
            }

            var dto = new AcademicYearDto(e.Id, e.YearName, e.StartDate, e.EndDate);
            logger.LogInformation("Successfully found academic year '{YearName}' for date {Date}", dto.YearName, request.Date);
            return dto;
        }
    }
}