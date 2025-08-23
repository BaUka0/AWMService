using AWMService.Application.Abstractions.Repositories;
using AWMService.Application.DTOs;
using KDS.Primitives.FluentResult;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AWMService.Application.UseCases.AcademicYears.Queries.ListAcademicYears
{
    public sealed class ListAcademicYearsQueryHandler(
        IAcademicYearsRepository academicYearsRepository,
        ILogger<ListAcademicYearsQueryHandler> logger)
        : IRequestHandler<ListAcademicYearsQuery, Result<IReadOnlyList<AcademicYearDto>>>
    {
        public async Task<Result<IReadOnlyList<AcademicYearDto>>> Handle(
            ListAcademicYearsQuery request, CancellationToken ct)
        {
            logger.LogInformation("Attempting to list all academic years.");
            var items = await academicYearsRepository.ListAllAsync(ct);

            var dtos = items
                .OrderByDescending(x => x.StartDate)
                .Select(x => new AcademicYearDto(x.Id, x.YearName, x.StartDate, x.EndDate))
                .ToList()
                .AsReadOnly();
            
            logger.LogInformation("Successfully retrieved {Count} academic years.", dtos.Count);
            return dtos;
        }
    }
}