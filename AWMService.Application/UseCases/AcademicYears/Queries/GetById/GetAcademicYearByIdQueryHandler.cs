using AWMService.Application.Abstractions.Repositories;
using AWMService.Application.DTOs;
using AWMService.Domain.Constatns;
using KDS.Primitives.FluentResult;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AWMService.Application.UseCases.AcademicYears.Queries.GetById
{
    public sealed class GetAcademicYearByIdQueryHandler 
        : IRequestHandler<GetAcademicYearByIdQuery, Result<AcademicYearDto>>
    {
        private readonly IAcademicYearsRepository _repo;
        private readonly ILogger<GetAcademicYearByIdQueryHandler> _logger;

        public GetAcademicYearByIdQueryHandler(
            IAcademicYearsRepository repo,
            ILogger<GetAcademicYearByIdQueryHandler> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<Result<AcademicYearDto>> Handle(GetAcademicYearByIdQuery request, CancellationToken ct)
        {
            _logger.LogInformation("GetAcademicYearById Id={Id}", request.Id);

            var e = await _repo.GetAcademicYearsByIdAsync(request.Id, ct);
            if (e is null)
                return Result.Failure<AcademicYearDto>(new Error(ErrorCode.NotFound, "Учебный год не найден."));

            var dto = new AcademicYearDto(e.Id, e.YearName, e.StartDate, e.EndDate);
            return Result.Success(dto);
        }
    }
}