using AWMService.Application.Abstractions.Data;
using AWMService.Application.Abstractions.Repositories;
using AWMService.Domain.Constants;
using KDS.Primitives.FluentResult;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AWMService.Application.UseCases.Supervisors.Commands.RevokeSupervisors
{
    public class RevokeSupervisorsCommandHandler(
        ISupervisorApprovalsRepository approvalsRepository,
        IUnitOfWork unitOfWork,
        ILogger<RevokeSupervisorsCommandHandler> logger) : IRequestHandler<RevokeSupervisorsCommand, Result>
    {
        public async Task<Result> Handle(RevokeSupervisorsCommand request, CancellationToken ct)
        {
            using var scope = logger.BeginScope(new Dictionary<string, object> { ["DepartmentId"] = request.DepartmentId, ["AcademicYearId"] = request.AcademicYearId, ["ActorUserId"] = request.ActorUserId });
            logger.LogInformation("Attempting to revoke supervisors for DepartmentId {DepartmentId} and AcademicYearId {AcademicYearId}. UserIds: {UserIds}",
                request.DepartmentId, request.AcademicYearId, string.Join(",", request.UserIds));

            if (request.UserIds.Count == 0)
            {
                logger.LogWarning("Revoke supervisors failed: UserIds list is empty.");
                return Result.Failure(new Error(ErrorCode.BadRequest, "UserIds is empty."));
            }

            await unitOfWork.BeginTransactionAsync(ct);
            try
            {
                foreach (var uid in request.UserIds.Distinct())
                {
                    await approvalsRepository.RevokeAsync(uid, request.DepartmentId, request.AcademicYearId, request.ActorUserId, ct);
                }
                await unitOfWork.CommitAsync(ct);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while revoking supervisors in bulk.");
                await unitOfWork.RollbackAsync(ct);
                return Result.Failure(new Error(ErrorCode.InternalServerError, "Failed to revoke supervisors."));
            }

            logger.LogInformation("Successfully revoked supervisors for DepartmentId {DepartmentId}", request.DepartmentId);
            return Result.Success();
        }
    }
}
