using AWMService.Application.Abstractions.Data;
using AWMService.Application.Abstractions.Repositories;
using AWMService.Domain.Constants;
using KDS.Primitives.FluentResult;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AWMService.Application.UseCases.Periods.Commands.UpdatePeriod
{
    public class UpdatePeriodCommandHandler(
        IPeriodsRepository periodsRepository, 
        IUnitOfWork unitOfWork, 
        ILogger<UpdatePeriodCommandHandler> logger,
        IAcademicYearsRepository academicYearsRepository,
        IStatusesRepository statusesRepository,
        IPeriodTypesRepository periodTypesRepository) : IRequestHandler<UpdatePeriodCommand, Result>
    {
        public async Task<Result> Handle(UpdatePeriodCommand request, CancellationToken ct)
        {
            using var scope = logger.BeginScope(new Dictionary<string, object> { ["PeriodId"] = request.Id, ["ActorUserId"] = request.ActorUserId });
            logger.LogInformation("Attempting to update period with ID {PeriodId}", request.Id);

            var period = await periodsRepository.GetByIdAsync(request.Id, ct);

            if (period == null)
            {
                logger.LogWarning("Period with ID {PeriodId} not found", request.Id);
                return Result.Failure(new Error(ErrorCode.NotFound, "Period not found"));
            }
            
            var academicYear = await academicYearsRepository.GetByIdAsync(request.AcademicYearId, ct);
            if (academicYear is null)
            {
                logger.LogWarning("AcademicYear with ID {AcademicYearId} not found.", request.AcademicYearId);
                return Result.Failure(new Error(ErrorCode.NotFound, $"AcademicYear with ID {request.AcademicYearId} not found."));
            }

            var status = await statusesRepository.GetByIdAsync(request.StatusId, ct);
            if (status is null)
            {
                logger.LogWarning("Status with ID {StatusId} not found.", request.StatusId);
                return Result.Failure(new Error(ErrorCode.NotFound, $"Status with ID {request.StatusId} not found."));
            }

            var periodType = await periodTypesRepository.GetByIdAsync(request.PeriodTypeId, ct);
            if (periodType is null)
            {
                logger.LogWarning("PeriodType with ID {PeriodTypeId} not found.", request.PeriodTypeId);
                return Result.Failure(new Error(ErrorCode.NotFound, $"PeriodType with ID {request.PeriodTypeId} not found."));
            }

            period.PeriodTypeId = request.PeriodTypeId;
            period.AcademicYearId = request.AcademicYearId;
            period.StartDate = request.StartDate;
            period.EndDate = request.EndDate;
            period.StatusId = request.StatusId;
            period.ModifiedBy = request.ActorUserId;
            period.ModifiedOn = DateTime.UtcNow;

            await unitOfWork.BeginTransactionAsync(ct);
            try
            {
                await periodsRepository.UpdateAsync(period, ct);
                await unitOfWork.CommitAsync(ct);

                logger.LogInformation("Successfully updated period with ID {PeriodId}", request.Id);
                return Result.Success();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while updating period with ID {PeriodId}", request.Id);
                await unitOfWork.RollbackAsync(ct);
                return Result.Failure(new Error(ErrorCode.InternalServerError, "An unexpected error occurred."));
            }
        }
    }
}
