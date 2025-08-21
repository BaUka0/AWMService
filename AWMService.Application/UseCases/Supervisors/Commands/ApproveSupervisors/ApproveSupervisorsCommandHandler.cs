using AWMService.Application.Abstractions.Data;
using AWMService.Application.Abstractions.Repositories;
using AWMService.Domain.Constants;
using KDS.Primitives.FluentResult;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AWMService.Application.UseCases.Supervisors.Commands.ApproveSupervisors
{
    public class ApproveSupervisorsCommandHandler(
        IUsersRepository usersRepository,
        ISupervisorApprovalsRepository approvalsRepository,
        IUnitOfWork unitOfWork,
        ILogger<ApproveSupervisorsCommandHandler> logger) : IRequestHandler<ApproveSupervisorsCommand, Result>
    {
        public async Task<Result> Handle(ApproveSupervisorsCommand request, CancellationToken ct)
        {
            using var scope = logger.BeginScope(new Dictionary<string, object> { ["DepartmentId"] = request.DepartmentId, ["AcademicYearId"] = request.AcademicYearId, ["ActorUserId"] = request.ActorUserId });
            logger.LogInformation("Attempting to approve supervisors for DepartmentId {DepartmentId} and AcademicYearId {AcademicYearId}. UserIds: {UserIds}",
                request.DepartmentId, request.AcademicYearId, string.Join(",", request.UserIds));

            if (request.UserIds.Count == 0)
            {
                logger.LogWarning("Approve supervisors failed: UserIds list is empty.");
                return Result.Failure(new Error(ErrorCode.BadRequest, "UserIds is empty."));
            }

            var teachers = await usersRepository.GetTeachersByDepartmentAsync(request.DepartmentId, ct);
            var teacherIds = teachers.Select(t => t.Id).ToHashSet();

            var notTeachers = request.UserIds.Except(teacherIds).ToArray();
            if (notTeachers.Length > 0)
            {
                var errorMsg = $"Следующие пользователи не являются преподавателями указанной кафедры: {string.Join(",", notTeachers)}";
                logger.LogWarning(errorMsg);
                return Result.Failure(new Error(ErrorCode.BadRequest, errorMsg));
            }

            await unitOfWork.BeginTransactionAsync(ct);
            try
            {
                foreach (var uid in request.UserIds.Distinct())
                {
                    await approvalsRepository.ApproveAsync(uid, request.DepartmentId, request.AcademicYearId, request.ActorUserId, ct);
                }
                await unitOfWork.CommitAsync(ct);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while approving supervisors in bulk.");
                await unitOfWork.RollbackAsync(ct);
                return Result.Failure(new Error(ErrorCode.InternalServerError, "Failed to approve supervisors."));
            }

            if (request.Notify)
            {
                logger.LogInformation("Notification for approved supervisors would be sent here.");
            }

            logger.LogInformation("Successfully approved supervisors for DepartmentId {DepartmentId}", request.DepartmentId);
            return Result.Success();
        }
    }
}
