using KDS.Primitives.FluentResult;
using MediatR;
using AWMService.Application.DTOs;

namespace AWMService.Application.UseCases.AcademicYears.Queries.GetByDate
{
    public sealed record GetAcademicYearByDateQuery(DateTime Date) 
        : IRequest<Result<AcademicYearDto>>;
}