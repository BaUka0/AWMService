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

namespace AWMService.Application.UseCases.Supervisors.Commands.RevokeSupervisors
{
    public class RevokeSupervisorsCommandHandler : IRequestHandler<RevokeSupervisorsCommand, Result>
    {
        private readonly ISupervisorApprovalsRepository _approvalsRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RevokeSupervisorsCommandHandler> _logger;

        public RevokeSupervisorsCommandHandler(
            ISupervisorApprovalsRepository approvalsRepository,
            IUnitOfWork unitOfWork,
            ILogger<RevokeSupervisorsCommandHandler> logger)
        {
            _approvalsRepository = approvalsRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result> Handle(RevokeSupervisorsCommand request, CancellationToken ct)
        {
            _logger.LogInformation("Attempting to revoke supervisors for DepartmentId {DepartmentId} and AcademicYearId {AcademicYearId}. UserIds: {UserIds}",
                request.DepartmentId, request.AcademicYearId, string.Join(",", request.UserIds));

            if (request.UserIds.Count == 0)
            {
                _logger.LogWarning("Revoke supervisors failed: UserIds list is empty.");
                return Result.Failure(new Error(ErrorCode.BadRequest, "UserIds is empty."));
            }

            await _unitOfWork.BeginTransactionAsync(ct);
            try
            {
                foreach (var uid in request.UserIds.Distinct())
                {
                    await _approvalsRepository.RevokeAsync(uid, request.DepartmentId, request.AcademicYearId, request.ActorUserId, ct);
                }
                await _unitOfWork.CommitAsync(ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while revoking supervisors in bulk.");
                await _unitOfWork.RollbackAsync(ct);
                return Result.Failure(new Error(ErrorCode.InternalServerError, "Failed to revoke supervisors."));
            }

            _logger.LogInformation("Successfully revoked supervisors for DepartmentId {DepartmentId}", request.DepartmentId);
            return Result.Success();
        }
    }
}
