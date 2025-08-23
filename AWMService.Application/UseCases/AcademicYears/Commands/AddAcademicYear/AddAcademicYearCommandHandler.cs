using AWMService.Application.Abstractions.Data;
using AWMService.Application.Abstractions.Repositories;
using AWMService.Domain.Constants;
using AWMService.Domain.Commons;
using KDS.Primitives.FluentResult;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AWMService.Application.UseCases.AcademicYears.Commands.AddAcademicYear
{
    public sealed class AddAcademicYearCommandHandler(
        IAcademicYearsRepository academicYearsRepository,
        IUnitOfWork unitOfWork,
        ILogger<AddAcademicYearCommandHandler> logger) : IRequestHandler<AddAcademicYearCommand, Result>
    {
        public async Task<Result> Handle(AddAcademicYearCommand request, CancellationToken ct)
        {
            using var scope = logger.BeginScope(new Dictionary<string, object>
            {
                ["YearName"] = request.YearName,
                ["ActorUserId"] = request.ActorUserId
            });

            logger.LogInformation("Attempting to add academic year '{YearName}' from {StartDate} to {EndDate}",
                request.YearName, request.StartDate, request.EndDate);

            var yearName = request.YearName.Trim();

            var all = await academicYearsRepository.ListAllAsync(ct);

            if (all.Any(y => string.Equals(y.YearName, yearName, StringComparison.OrdinalIgnoreCase)))
            {
                logger.LogWarning("Academic year with name '{YearName}' already exists.", yearName);
                return Result.Failure(new Error(ErrorCode.Conflict, "Учебный год с таким названием уже существует."));
            }

            if (all.Any(y => DateRangeHelper.Overlaps(request.StartDate, request.EndDate, y.StartDate, y.EndDate)))
            {
                logger.LogWarning("Date range for '{YearName}' overlaps with an existing academic year.", yearName);
                return Result.Failure(new Error(ErrorCode.Conflict, "Диапазон дат пересекается с существующим учебным годом."));
            }

            await unitOfWork.BeginTransactionAsync(ct);
            try
            {
                await academicYearsRepository.AddAcademicYearsAsync(yearName, request.StartDate, request.EndDate, request.ActorUserId, ct);
                await unitOfWork.CommitAsync(ct);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to add academic year '{YearName}'.", yearName);
                await unitOfWork.RollbackAsync(ct);
                return Result.Failure(new Error(ErrorCode.InternalServerError, "Failed to add academic year."));
            }

            logger.LogInformation("Successfully added academic year '{YearName}'.", yearName);
            return Result.Success();
        }
    }
}
