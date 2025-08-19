using AWMService.Application.Abstractions.Data;
using AWMService.Application.Abstractions.Repositories;
using AWMService.Domain.Constatns;
using KDS.Primitives.FluentResult;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AWMService.Application.UseCases.AcademicYears.Commands.AddAcademicYear
{
    public sealed class AddAcademicYearCommandHandler : IRequestHandler<AddAcademicYearCommand, Result>
    {
        private readonly IAcademicYearsRepository _repo;
        private readonly IUnitOfWork _uow;
        private readonly ILogger<AddAcademicYearCommandHandler> _logger;

        public AddAcademicYearCommandHandler(
            IAcademicYearsRepository repo,
            IUnitOfWork uow,
            ILogger<AddAcademicYearCommandHandler> logger)
        {
            _repo = repo;
            _uow = uow;
            _logger = logger;
        }

        public async Task<Result> Handle(AddAcademicYearCommand request, CancellationToken ct)
        {
            _logger.LogInformation("AddAcademicYear: {Name} {Start:yyyy-MM-dd}..{End:yyyy-MM-dd}",
                request.YearName, request.StartDate, request.EndDate);

            var yearName = request.YearName?.Trim();
            if (string.IsNullOrWhiteSpace(yearName))
                return Result.Failure(new Error(ErrorCode.BadRequest, "YearName is required."));
            if (request.EndDate <= request.StartDate)
                return Result.Failure(new Error(ErrorCode.BadRequest, "EndDate must be greater than StartDate."));

            var all = await _repo.ListAllAsync(ct);
            
            if (all.Any(y => string.Equals(y.YearName, yearName, StringComparison.OrdinalIgnoreCase)))
                return Result.Failure(new Error(ErrorCode.Conflict, "Учебный год с таким названием уже существует."));
            
            static bool Overlaps(DateTime s1, DateTime e1, DateTime s2, DateTime e2) => s1 <= e2 && s2 <= e1;
            if (all.Any(y => Overlaps(request.StartDate, request.EndDate, y.StartDate, y.EndDate)))
                return Result.Failure(new Error(ErrorCode.Conflict, "Диапазон дат пересекается с существующим учебным годом."));

            await _uow.BeginTransactionAsync(ct);
            try
            {
                await _repo.AddAcademicYearsAsync(yearName, request.StartDate, request.EndDate, request.ActorUserId, ct);
                await _uow.CommitAsync(ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AddAcademicYear failed.");
                await _uow.RollbackAsync(ct);
                return Result.Failure(new Error(ErrorCode.InternalServerError, "Failed to add academic year."));
            }

            _logger.LogInformation("AddAcademicYear: success ({Name})", yearName);
            return Result.Success();
        }
    }
}
