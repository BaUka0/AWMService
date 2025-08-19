using AWMService.Application.Abstractions.Data;
using AWMService.Application.Abstractions.Repositories;
using AWMService.Domain.Constatns;
using KDS.Primitives.FluentResult;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AWMService.Application.UseCases.Supervisors.Commands.ApproveSupervisors
{
    public class ApproveSupervisorsCommandHandler : IRequestHandler<ApproveSupervisorsCommand, Result>
    {
        private readonly IUsersRepository _usersRepository;
        private readonly ISupervisorApprovalsRepository _approvalsRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ApproveSupervisorsCommandHandler> _logger;

        public ApproveSupervisorsCommandHandler(
            IUsersRepository usersRepository,
            ISupervisorApprovalsRepository approvalsRepository,
            IUnitOfWork unitOfWork,
            ILogger<ApproveSupervisorsCommandHandler> logger)
        {
            _usersRepository = usersRepository;
            _approvalsRepository = approvalsRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result> Handle(ApproveSupervisorsCommand request, CancellationToken ct)
        {
            _logger.LogInformation("Attempting to approve supervisors for DepartmentId {DepartmentId} and AcademicYearId {AcademicYearId}. UserIds: {UserIds}",
                request.DepartmentId, request.AcademicYearId, string.Join(",", request.UserIds));

            if (request.UserIds.Count == 0)
            {
                _logger.LogWarning("Approve supervisors failed: UserIds list is empty.");
                return Result.Failure(new Error(ErrorCode.BadRequest, "UserIds is empty."));
            }

            var teachers = await _usersRepository.GetTeachersByDepartmentAsync(request.DepartmentId, ct);
            var teacherIds = teachers.Select(t => t.Id).ToHashSet();

            var notTeachers = request.UserIds.Except(teacherIds).ToArray();
            if (notTeachers.Length > 0)
            {
                var errorMsg = $"Следующие пользователи не являются преподавателями указанной кафедры: {string.Join(",", notTeachers)}";
                _logger.LogWarning(errorMsg);
                return Result.Failure(new Error(ErrorCode.BadRequest, errorMsg));
            }

            await _unitOfWork.BeginTransactionAsync(ct);
            try
            {
                foreach (var uid in request.UserIds.Distinct())
                {
                    await _approvalsRepository.ApproveAsync(uid, request.DepartmentId, request.AcademicYearId, request.ActorUserId, ct);
                }
                await _unitOfWork.CommitAsync(ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while approving supervisors in bulk.");
                await _unitOfWork.RollbackAsync(ct);
                return Result.Failure(new Error(ErrorCode.InternalServerError, "Failed to approve supervisors."));
            }

            if (request.Notify)
            {
                _logger.LogInformation("Notification for approved supervisors would be sent here.");
            }

            _logger.LogInformation("Successfully approved supervisors for DepartmentId {DepartmentId}", request.DepartmentId);
            return Result.Success();
        }
    }
}
