using AWMService.Application.Abstractions;
using AWMService.Domain.Constatns;
using KDS.Primitives.FluentResult;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AWMService.Application.UseCases.Roles.Commands.AssignPermission
{
    public class AssignPermissionToRoleCommandHandler : IRequestHandler<AssignPermissionToRoleCommand, Result>
    {
        private readonly IRolesRepository _rolesRepository;
        private readonly IPermissionsRepository _permissionsRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AssignPermissionToRoleCommandHandler> _logger;

        public AssignPermissionToRoleCommandHandler(
            IRolesRepository rolesRepository,
            IPermissionsRepository permissionsRepository,
            IUnitOfWork unitOfWork,
            ILogger<AssignPermissionToRoleCommandHandler> logger)
        {
            _rolesRepository = rolesRepository;
            _permissionsRepository = permissionsRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result> Handle(AssignPermissionToRoleCommand request, CancellationToken ct)
        {
            _logger.LogInformation("Attempting to assign permission {PermissionId} to role {RoleId}", request.PermissionId, request.RoleId);

            // Check if role exists
            var role = await _rolesRepository.GetByIdAsync(request.RoleId, ct);
            if (role == null)
            {
                _logger.LogWarning("Role with ID {RoleId} not found", request.RoleId);
                return Result.Failure(new Error(ErrorCode.NotFound, "Role not found"));
            }

            // Check if permission exists
            var permission = await _permissionsRepository.GetPermissionsByIdAsync(request.PermissionId, ct);
            if (permission == null)
            {
                _logger.LogWarning("Permission with ID {PermissionId} not found", request.PermissionId);
                return Result.Failure(new Error(ErrorCode.NotFound, "Permission not found"));
            }

            await _unitOfWork.BeginTransactionAsync(ct);
            try
            {
                // Assign permission to role
                await _rolesRepository.AddPermissionToRoleAsync(request.RoleId, request.PermissionId, request.ActorUserId, ct);
                await _unitOfWork.CommitAsync(ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while assigning permission {PermissionId} to role {RoleId}", request.PermissionId, request.RoleId);
                await _unitOfWork.RollbackAsync(ct);
                return Result.Failure(new Error(ErrorCode.InternalServerError, "Failed to assign permission to role"));
            }

            _logger.LogInformation("Permission {PermissionId} assigned to role {RoleId} successfully", request.PermissionId, request.RoleId);
            return Result.Success();
        }
    }
}