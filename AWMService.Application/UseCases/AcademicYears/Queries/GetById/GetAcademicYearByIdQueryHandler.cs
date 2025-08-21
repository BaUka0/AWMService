using AWMService.Application.Abstractions.Repositories;
using AWMService.Application.DTOs;
using AWMService.Domain.Constants;
using KDS.Primitives.FluentResult;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AWMService.Application.UseCases.AcademicYears.Queries.GetById
{
    public sealed class GetAcademicYearByIdQueryHandler(
        IAcademicYearsRepository academicYearsRepository,
        ILogger<GetAcademicYearByIdQueryHandler> logger)
                : IRequestHandler<GetAcademicYearByIdQuery, Result<AcademicYearDto>>
    {
        public async Task<Result<AcademicYearDto>> Handle(GetAcademicYearByIdQuery request, CancellationToken ct)
        {
            using var scope = logger.BeginScope(new Dictionary<string, object> { ["AcademicYearId"] = request.Id });
            logger.LogInformation("Attempting to get academic year by Id: {AcademicYearId}", request.Id);

            var e = await academicYearsRepository.GetAcademicYearsByIdAsync(request.Id, ct);
            if (e is null)
            {
                logger.LogWarning("Academic year with Id {AcademicYearId} not found.", request.Id);
                return Result.Failure<AcademicYearDto>(new Error(ErrorCode.NotFound, "Учебный год не найден."));
            }

            var dto = new AcademicYearDto(e.Id, e.YearName, e.StartDate, e.EndDate);
            logger.LogInformation("Successfully found academic year '{YearName}' with Id {AcademicYearId}", dto.YearName, request.Id);
            return dto;
        }
    }
}