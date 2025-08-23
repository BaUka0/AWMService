using AWMService.Application.Abstractions.Data;
using AWMService.Application.Abstractions.Repositories;
using AWMService.Domain.Constants;
using KDS.Primitives.FluentResult;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AWMService.Application.UseCases.Periods.Commands.DeletePeriod
{
    public class DeletePeriodCommandHandler(IPeriodsRepository periodsRepository, IUnitOfWork unitOfWork, ILogger<DeletePeriodCommandHandler> logger) : IRequestHandler<DeletePeriodCommand, Result>
    {
        public async Task<Result> Handle(DeletePeriodCommand request, CancellationToken ct)
        {
            using var scope = logger.BeginScope(new Dictionary<string, object> { ["PeriodId"] = request.Id });
            logger.LogInformation("Attempting to delete period with Id {PeriodId}", request.Id);

            var entity = await periodsRepository.GetByIdAsync(request.Id, ct);
            if (entity is null)
            {
                logger.LogWarning("Period with Id {PeriodId} not found.", request.Id);
                return Result.Failure(new Error(ErrorCode.NotFound, "Период не найден."));
            }

            entity.IsDeleted = true;
            entity.DeletedOn = DateTime.UtcNow;
            entity.DeletedBy = request.ActorUserId;

            await unitOfWork.BeginTransactionAsync(ct);
            try
            {
                await periodsRepository.SoftDeleteAsync(entity, ct);
                await unitOfWork.CommitAsync(ct);
                logger.LogInformation("Successfully deleted period with Id {PeriodId}", request.Id);
                return Result.Success();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to delete period with Id {PeriodId}", request.Id);
                await unitOfWork.RollbackAsync(ct);
                return Result.Failure(new Error(ErrorCode.InternalServerError, "Failed to delete period."));
            }
        }
    }
}
