using KDS.Primitives.FluentResult;
using MediatR;
using AWMService.Application.DTOs;

namespace AWMService.Application.UseCases.AcademicYears.Queries.ListAcademicYears
{
    public sealed record ListAcademicYearsQuery() : IRequest<Result<IReadOnlyList<AcademicYearDto>>>;
}

