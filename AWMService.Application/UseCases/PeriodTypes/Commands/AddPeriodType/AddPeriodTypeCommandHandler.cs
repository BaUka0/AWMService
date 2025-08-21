using AWMService.Application.Abstractions.Data;
using AWMService.Application.Abstractions.Repositories;
using AWMService.Domain.Constants;
using AWMService.Domain.Entities;
using KDS.Primitives.FluentResult;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AWMService.Application.UseCases.PeriodType.Commands.AddPeriodType
{
    public sealed class AddPeriodTypeCommandHandler(
        IPeriodTypesRepository periodTypesRepository,
        IUnitOfWork unitOfWork,
        ILogger<AddPeriodTypeCommandHandler> logger) : IRequestHandler<AddPeriodTypeCommand, Result<int>>
    {
        public async Task<Result<int>> Handle(AddPeriodTypeCommand request, CancellationToken ct)
        {
            using var scope = logger.BeginScope(new Dictionary<string, object>
            {
                ["Name"] = request.Name,
                ["ActorUserId"] = request.ActorUserId
            });

            logger.LogInformation("Attempting to add period type '{Name}'", request.Name);

            var name = request.Name.Trim();
            var existing = await periodTypesRepository.GetByNameAsync(name, ct);

            if (existing is not null)
            {
                logger.LogWarning("Period type with name '{Name}' already exists.", name);
                return Result.Failure<int>(new Error(ErrorCode.Conflict, "Тип периода с таким названием уже существует."));
            }

            var entity = new PeriodTypes
            {
                Name = name,
                CreatedBy = request.ActorUserId,
                CreatedOn = DateTime.UtcNow
            };

            await unitOfWork.BeginTransactionAsync(ct);
            try
            {
                var newEntity = await periodTypesRepository.AddAsync(entity, ct);
                await unitOfWork.CommitAsync(ct);
                logger.LogInformation("Successfully added period type '{Name}' with Id {Id}.", name, newEntity.Id);
                return newEntity.Id;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to add period type '{Name}'.", name);
                await unitOfWork.RollbackAsync(ct);
                return Result.Failure<int>(new Error(ErrorCode.InternalServerError, "Failed to add period type."));
            }
        }
    }
}
