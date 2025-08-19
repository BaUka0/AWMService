using AWMService.Application.Abstractions.Repositories;
using AWMService.Application.DTOs;
using KDS.Primitives.FluentResult;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AWMService.Application.UseCases.AcademicYears.Queries.ListAcademicYears
{
    public sealed class ListAcademicYearsQueryHandler(
        IAcademicYearsRepository repo,
        ILogger<ListAcademicYearsQueryHandler> logger)
        : IRequestHandler<ListAcademicYearsQuery, Result<IReadOnlyList<AcademicYearDto>>>
    {
        public async Task<Result<IReadOnlyList<AcademicYearDto>>> Handle(
            ListAcademicYearsQuery request, CancellationToken ct)
        {
            logger.LogInformation("ListAcademicYears triggered.");
            var items = await repo.ListAllAsync(ct);

            var dtos = items
                .OrderByDescending(x => x.StartDate)
                .Select(x => new AcademicYearDto(x.Id, x.YearName, x.StartDate, x.EndDate))
                .ToList()
                .AsReadOnly();

            return Result.Success<IReadOnlyList<AcademicYearDto>>(dtos);
        }
    }
}