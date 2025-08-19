using AWMService.Application.Abstractions.Repositories;
using AWMService.Application.DTOs;
using AWMService.Domain.Constatns;
using KDS.Primitives.FluentResult;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AWMService.Application.UseCases.AcademicYears.Queries.GetByDate
{
    public sealed class GetAcademicYearByDateQueryHandler 
        : IRequestHandler<GetAcademicYearByDateQuery, Result<AcademicYearDto>>
    {
        private readonly IAcademicYearsRepository _repo;
        private readonly ILogger<GetAcademicYearByDateQueryHandler> _logger;

        public GetAcademicYearByDateQueryHandler(
            IAcademicYearsRepository repo,
            ILogger<GetAcademicYearByDateQueryHandler> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<Result<AcademicYearDto>> Handle(GetAcademicYearByDateQuery request, CancellationToken ct)
        {
            _logger.LogInformation("GetAcademicYearByDate Date={Date}", request.Date);

            var e = await _repo.GetAcademicYearsByDateAsync(request.Date, ct);
            if (e is null)
                return Result.Failure<AcademicYearDto>(new Error(ErrorCode.NotFound, "Учебный год для указанной даты не найден."));

            var dto = new AcademicYearDto(e.Id, e.YearName, e.StartDate, e.EndDate);
            return Result.Success(dto);
        }
    }
}