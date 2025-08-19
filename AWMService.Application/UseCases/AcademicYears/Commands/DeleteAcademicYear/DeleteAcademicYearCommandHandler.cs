using AWMService.Application.Abstractions.Data;
using AWMService.Application.Abstractions.Repositories;
using AWMService.Domain.Constatns;
using KDS.Primitives.FluentResult;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AWMService.Application.UseCases.AcademicYears.Commands.DeleteAcademicYear
{
    public sealed class DeleteAcademicYearCommandHandler : IRequestHandler<DeleteAcademicYearCommand, Result>
    {
        private readonly IAcademicYearsRepository _repo;
        private readonly IUnitOfWork _uow;
        private readonly ILogger<DeleteAcademicYearCommandHandler> _logger;

        public DeleteAcademicYearCommandHandler(
            IAcademicYearsRepository repo,
            IUnitOfWork uow,
            ILogger<DeleteAcademicYearCommandHandler> logger)
        {
            _repo = repo;
            _uow = uow;
            _logger = logger;
        }

        public async Task<Result> Handle(DeleteAcademicYearCommand request, CancellationToken ct)
        {
            _logger.LogInformation("DeleteAcademicYear: Id={Id}", request.Id);

            if (request.Id <= 0)
                return Result.Failure(new Error(ErrorCode.BadRequest, "Invalid Id."));

            var existing = await _repo.GetAcademicYearsByIdAsync(request.Id, ct);
            if (existing is null)
                return Result.Failure(new Error(ErrorCode.NotFound, "Учебный год не найден."));

            await _uow.BeginTransactionAsync(ct);
            try
            {
                await _repo.SoftDeleteAcademicYearsAsync(request.Id, request.ActorUserId, ct);
                await _uow.CommitAsync(ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DeleteAcademicYear failed. Id={Id}", request.Id);
                await _uow.RollbackAsync(ct);
                return Result.Failure(new Error(ErrorCode.InternalServerError, "Failed to delete academic year."));
            }

            _logger.LogInformation("DeleteAcademicYear: success Id={Id}", request.Id);
            return Result.Success();
        }
    }
}
