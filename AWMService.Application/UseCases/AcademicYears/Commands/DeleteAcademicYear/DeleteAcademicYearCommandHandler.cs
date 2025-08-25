using AWMService.Application.Abstractions.Data;
using AWMService.Application.Abstractions.Repositories;
using AWMService.Domain.Constants;
using KDS.Primitives.FluentResult;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AWMService.Application.UseCases.AcademicYears.Commands.DeleteAcademicYear
{
    public sealed class DeleteAcademicYearCommandHandler(
        IAcademicYearsRepository repo,
        IUnitOfWork uow,
        ILogger<DeleteAcademicYearCommandHandler> logger) : IRequestHandler<DeleteAcademicYearCommand, Result>
    {
        public async Task<Result> Handle(DeleteAcademicYearCommand request, CancellationToken ct)
        {
            using var scope = logger.BeginScope(new Dictionary<string, object>
            {
                ["AcademicYearId"] = request.Id,
                ["ActorUserId"] = request.ActorUserId
            });

            logger.LogInformation("Attempting to delete academic year with Id {AcademicYearId}", request.Id);

            var existing = await repo.GetAcademicYearsByIdAsync(request.Id, ct);
            if (existing is null)
            {
                logger.LogWarning("Academic year with Id {AcademicYearId} not found.", request.Id);
                return Result.Failure(new Error(ErrorCode.NotFound, "Учебный год не найден."));
            }

            await uow.BeginTransactionAsync(ct);
            try
            {
                await repo.SoftDeleteAcademicYearsAsync(request.Id, request.ActorUserId, ct);
                await uow.CommitAsync(ct);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to delete academic year with Id {AcademicYearId}", request.Id);
                await uow.RollbackAsync(ct);
                return Result.Failure(new Error(ErrorCode.InternalServerError, "Failed to delete academic year."));
            }

            logger.LogInformation("Successfully deleted academic year with Id {AcademicYearId}", request.Id);
            return Result.Success();
        }
    }
}
