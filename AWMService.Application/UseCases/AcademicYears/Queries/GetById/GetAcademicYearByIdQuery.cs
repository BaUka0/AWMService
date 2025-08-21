using KDS.Primitives.FluentResult;
using MediatR;
using AWMService.Application.DTOs;

namespace AWMService.Application.UseCases.AcademicYears.Queries.GetById
{
    public sealed record GetAcademicYearByIdQuery(int Id) 
        : IRequest<Result<AcademicYearDto>>;
}