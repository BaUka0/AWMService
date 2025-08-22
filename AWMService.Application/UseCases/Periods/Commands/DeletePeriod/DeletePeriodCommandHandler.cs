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
            using var scope = logger.BeginScope(new Dictionary<string, object> { ["PeriodId"] = request.Id, ["ActorUserId"] = request.ActorUserId });
            logger.LogInformation("Attempting to delete period with ID {PeriodId}", request.Id);

            var period = await periodsRepository.GetByIdAsync(request.Id, ct);

            if (period == null)
            {
                logger.LogWarning("Period with ID {PeriodId} not found", request.Id);
                return Result.Failure(new Error(ErrorCode.NotFound, "Period not found"));
            }

            await unitOfWork.BeginTransactionAsync(ct);
            try
            {
                await periodsRepository.DeleteAsync(request.Id, ct);
                await unitOfWork.CommitAsync(ct);

                logger.LogInformation("Successfully deleted period with ID {PeriodId}", request.Id);
                return Result.Success();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while deleting period with ID {PeriodId}", request.Id);
                await unitOfWork.RollbackAsync(ct);
                return Result.Failure(new Error(ErrorCode.InternalServerError, "An unexpected error occurred."));
            }
        }
    }
}
