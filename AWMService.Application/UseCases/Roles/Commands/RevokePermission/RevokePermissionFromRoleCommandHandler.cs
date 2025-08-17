using AWMService.Application.Abstractions;
using AWMService.Domain.Constatns;
using KDS.Primitives.FluentResult;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AWMService.Application.UseCases.Roles.Commands.RevokePermission
{
    public class RevokePermissionFromRoleCommandHandler : IRequestHandler<RevokePermissionFromRoleCommand, Result>
    {
        private readonly IRolesRepository _rolesRepository;
        private readonly IPermissionsRepository _permissionsRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RevokePermissionFromRoleCommandHandler> _logger;

        public RevokePermissionFromRoleCommandHandler(
            IRolesRepository rolesRepository,
            IPermissionsRepository permissionsRepository,
            IUnitOfWork unitOfWork,
            ILogger<RevokePermissionFromRoleCommandHandler> logger)
        {
            _rolesRepository = rolesRepository;
            _permissionsRepository = permissionsRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result> Handle(RevokePermissionFromRoleCommand request, CancellationToken ct)
        {
            _logger.LogInformation("Attempting to revoke permission {PermissionId} from role {RoleId}", request.PermissionId, request.RoleId);

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
                // Revoke permission from role
                await _rolesRepository.RemovePermissionFromRoleAsync(request.RoleId, request.PermissionId, request.ActorUserId, ct);
                await _unitOfWork.CommitAsync(ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while revoking permission {PermissionId} from role {RoleId}", request.PermissionId, request.RoleId);
                await _unitOfWork.RollbackAsync(ct);
                return Result.Failure(new Error(ErrorCode.InternalServerError, "Failed to revoke permission from role"));
            }

            _logger.LogInformation("Permission {PermissionId} revoked from role {RoleId} successfully", request.PermissionId, request.RoleId);
            return Result.Success();
        }
    }
}