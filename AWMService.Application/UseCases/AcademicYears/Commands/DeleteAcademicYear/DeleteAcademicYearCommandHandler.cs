using AWMService.Application.Abstractions.Data;
using AWMService.Application.Abstractions.Repositories;
using AWMService.Domain.Constants;
using KDS.Primitives.FluentResult;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AWMService.Application.UseCases.AcademicYears.Commands.DeleteAcademicYear
{
    public sealed class DeleteAcademicYearCommandHandler(
        IAcademicYearsRepository academicYearsRepository,
        IUnitOfWork unitOfWork,
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

            var entity = await academicYearsRepository.GetAcademicYearsByIdAsync(request.Id, ct);
            if (entity is null)
            {
                logger.LogWarning("Academic year with Id {AcademicYearId} not found.", request.Id);
                return Result.Failure(new Error(ErrorCode.NotFound, "Учебный год не найден."));
            }

            entity.IsDeleted = true;
            entity.DeletedOn = DateTime.UtcNow;
            entity.DeletedBy = request.ActorUserId;

            await unitOfWork.BeginTransactionAsync(ct);
            try
            {
                await academicYearsRepository.SoftDeleteAsync(entity, ct);
                await unitOfWork.CommitAsync(ct);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to delete academic year with Id {AcademicYearId}", request.Id);
                await unitOfWork.RollbackAsync(ct);
                return Result.Failure(new Error(ErrorCode.InternalServerError, "Failed to delete academic year."));
            }

            logger.LogInformation("Successfully deleted academic year with Id {AcademicYearId}", request.Id);
            return Result.Success();
        }
    }
}
