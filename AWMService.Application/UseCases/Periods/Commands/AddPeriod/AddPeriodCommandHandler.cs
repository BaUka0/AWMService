using AWMService.Application.Abstractions.Data;
using AWMService.Application.Abstractions.Repositories;
using AWMService.Application.DTOs;
using AWMService.Domain.Constants;
using KDS.Primitives.FluentResult;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AWMService.Application.UseCases.Periods.Commands.AddPeriod
{
    public class AddPeriodCommandHandler(
        IPeriodsRepository periodsRepository,
        IUnitOfWork unitOfWork,
        ILogger<AddPeriodCommandHandler> logger,
        IAcademicYearsRepository academicYearsRepository,
        IStatusesRepository statusesRepository,
        IPeriodTypesRepository periodTypesRepository) : IRequestHandler<AddPeriodCommand, Result<PeriodDto>>
    {
        public async Task<Result<PeriodDto>> Handle(AddPeriodCommand request, CancellationToken ct)
        {
            using var scope = logger.BeginScope(new Dictionary<string, object> { ["ActorUserId"] = request.ActorUserId });
            logger.LogInformation("Attempting to add a new period.");

            var academicYear = await academicYearsRepository.GetAcademicYearsByIdAsync(request.AcademicYearId, ct);
            if (academicYear is null)
            {
                logger.LogWarning("AcademicYear with ID {AcademicYearId} not found.", request.AcademicYearId);
                return Result.Failure<PeriodDto>(new Error(ErrorCode.NotFound, $"AcademicYear with ID {request.AcademicYearId} not found."));
            }

            var status = await statusesRepository.GetStatusesByIdAsync(request.StatusId, ct);
            if (status is null)
            {
                logger.LogWarning("Status with ID {StatusId} not found.", request.StatusId);
                return Result.Failure<PeriodDto>(new Error(ErrorCode.NotFound, $"Status with ID {request.StatusId} not found."));
            }

            var periodType = await periodTypesRepository.GetByIdAsync(request.PeriodTypeId, ct);
            if (periodType is null)
            {
                logger.LogWarning("PeriodType with ID {PeriodTypeId} not found.", request.PeriodTypeId);
                return Result.Failure<PeriodDto>(new Error(ErrorCode.NotFound, $"PeriodType with ID {request.PeriodTypeId} not found."));
            }

            var period = new Domain.Entities.Periods
            {
                PeriodTypeId = request.PeriodTypeId,
                AcademicYearId = request.AcademicYearId,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                StatusId = request.StatusId,
                CreatedBy = request.ActorUserId,
                CreatedOn = DateTime.UtcNow
            };

            await unitOfWork.BeginTransactionAsync(ct);
            try
            {
                await periodsRepository.AddAsync(period, ct);
                await unitOfWork.CommitAsync(ct);

                var newPeriod = await periodsRepository.GetByIdAsync(period.Id, ct);
                var periodDto = new PeriodDto(newPeriod.Id, newPeriod.PeriodTypeId, newPeriod.PeriodType.Name, newPeriod.AcademicYearId, newPeriod.AcademicYear.YearName, newPeriod.StartDate, newPeriod.EndDate, newPeriod.StatusId, newPeriod.Status.Name);

                logger.LogInformation("Successfully added a new period with ID {PeriodId}", newPeriod.Id);
                return periodDto;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while adding a new period.");
                await unitOfWork.RollbackAsync(ct);
                return Result.Failure<PeriodDto>(new Error(ErrorCode.InternalServerError, "An unexpected error occurred."));
            }
        }
    }
}
