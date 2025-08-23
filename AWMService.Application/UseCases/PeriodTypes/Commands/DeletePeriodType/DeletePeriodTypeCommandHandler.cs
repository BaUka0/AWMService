using AWMService.Application.Abstractions.Data;
using AWMService.Application.Abstractions.Repositories;
using AWMService.Domain.Constants;
using KDS.Primitives.FluentResult;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AWMService.Application.UseCases.PeriodType.Commands.DeletePeriodType
{
    public sealed class DeletePeriodTypeCommandHandler(
        IPeriodTypesRepository periodTypesRepository,
        IUnitOfWork unitOfWork,
        ILogger<DeletePeriodTypeCommandHandler> logger) : IRequestHandler<DeletePeriodTypeCommand, Result>
    {
        public async Task<Result> Handle(DeletePeriodTypeCommand request, CancellationToken ct)
        {
            using var scope = logger.BeginScope(new Dictionary<string, object> { ["PeriodTypeId"] = request.Id });
            logger.LogInformation("Attempting to delete period type with Id {PeriodTypeId}", request.Id);

            var entity = await periodTypesRepository.GetByIdAsync(request.Id, ct);
            if (entity is null)
            {
                logger.LogWarning("Period type with Id {PeriodTypeId} not found.", request.Id);
                return Result.Failure(new Error(ErrorCode.NotFound, "Тип периода не найден."));
            }

            entity.IsDeleted = true;
            entity.DeletedOn = DateTime.UtcNow;
            entity.DeletedBy = request.ActorUserId;

            await unitOfWork.BeginTransactionAsync(ct);
            try
            {
                await periodTypesRepository.SoftDeleteAsync(entity, ct);
                await unitOfWork.CommitAsync(ct);
                logger.LogInformation("Successfully deleted period type with Id {PeriodTypeId}", request.Id);
                return Result.Success();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to delete period type with Id {PeriodTypeId}", request.Id);
                await unitOfWork.RollbackAsync(ct);
                return Result.Failure(new Error(ErrorCode.InternalServerError, "Failed to delete period type."));
            }
        }
    }
}
