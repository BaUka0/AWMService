using AWMService.Application.Abstractions.Repositories;
using AWMService.Application.DTOs;
using AWMService.Domain.Constants;
using KDS.Primitives.FluentResult;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace AWMService.Application.UseCases.PeriodType.Queries.GetPeriodTypeById
{
    public sealed class GetPeriodTypeByIdQueryHandler(
        IPeriodTypesRepository periodTypesRepository,
        ILogger<GetPeriodTypeByIdQueryHandler> logger)
        : IRequestHandler<GetPeriodTypeByIdQuery, Result<PeriodTypeDto>>
    {
        public async Task<Result<PeriodTypeDto>> Handle(GetPeriodTypeByIdQuery request, CancellationToken ct)
        {
            using var scope = logger.BeginScope(new Dictionary<string, object> { ["PeriodTypeId"] = request.Id });
            logger.LogInformation("Attempting to get period type by Id: {PeriodTypeId}", request.Id);

            var entity = await periodTypesRepository.GetByIdAsync(request.Id, ct);
            if (entity is null)
            {
                logger.LogWarning("Period type with Id {PeriodTypeId} not found.", request.Id);
                return Result.Failure<PeriodTypeDto>(new Error(ErrorCode.NotFound, "Тип периода не найден."));
            }

            var dto = new PeriodTypeDto(entity.Id, entity.Name);
            logger.LogInformation("Successfully found period type '{Name}' with Id {PeriodTypeId}", dto.Name, request.Id);
            return dto;
        }
    }
}
