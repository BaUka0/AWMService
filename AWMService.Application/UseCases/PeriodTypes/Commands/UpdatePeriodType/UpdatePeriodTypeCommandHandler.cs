using AWMService.Application.Abstractions.Data;
using AWMService.Application.Abstractions.Repositories;
using AWMService.Domain.Constants;
using KDS.Primitives.FluentResult;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AWMService.Application.UseCases.PeriodType.Commands.UpdatePeriodType
{
    public sealed class UpdatePeriodTypeCommandHandler(
        IPeriodTypesRepository periodTypesRepository,
        IUnitOfWork unitOfWork,
        ILogger<UpdatePeriodTypeCommandHandler> logger) : IRequestHandler<UpdatePeriodTypeCommand, Result>
    {
        public async Task<Result> Handle(UpdatePeriodTypeCommand request, CancellationToken ct)
        {
            using var scope = logger.BeginScope(new Dictionary<string, object> { ["PeriodTypeId"] = request.Id });
            logger.LogInformation("Attempting to update period type with Id {PeriodTypeId}", request.Id);

            var entity = await periodTypesRepository.GetByIdAsync(request.Id, ct);
            if (entity is null)
            {
                logger.LogWarning("Period type with Id {PeriodTypeId} not found.", request.Id);
                return Result.Failure(new Error(ErrorCode.NotFound, "Тип периода не найден."));
            }

            var newName = request.Name.Trim();
            if (!string.Equals(entity.Name, newName, StringComparison.OrdinalIgnoreCase))
            {
                var existing = await periodTypesRepository.GetByNameAsync(newName, ct);
                if (existing is not null && existing.Id != request.Id)
                {
                    logger.LogWarning("Period type with name '{Name}' already exists.", newName);
                    return Result.Failure(new Error(ErrorCode.Conflict, "Тип периода с таким названием уже существует."));
                }
                entity.Name = newName;
            }

            entity.ModifiedBy = request.ActorUserId;
            entity.ModifiedOn = DateTime.UtcNow;

            await unitOfWork.BeginTransactionAsync(ct);
            try
            {
                await periodTypesRepository.UpdateAsync(entity, ct);
                await unitOfWork.CommitAsync(ct);
                logger.LogInformation("Successfully updated period type with Id {PeriodTypeId}", request.Id);
                return Result.Success();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to update period type with Id {PeriodTypeId}", request.Id);
                await unitOfWork.RollbackAsync(ct);
                return Result.Failure(new Error(ErrorCode.InternalServerError, "Failed to update period type."));
            }
        }
    }
}
