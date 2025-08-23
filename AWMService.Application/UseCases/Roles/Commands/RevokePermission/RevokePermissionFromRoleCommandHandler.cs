using AWMService.Application.Abstractions.Repositories;
using AWMService.Application.Abstractions.Data;
using AWMService.Domain.Constants;
using KDS.Primitives.FluentResult;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AWMService.Application.UseCases.Roles.Commands.RevokePermission
{
    public class RevokePermissionFromRoleCommandHandler(
        IRolesRepository rolesRepository,
        IPermissionsRepository permissionsRepository,
        IUnitOfWork unitOfWork,
        ILogger<RevokePermissionFromRoleCommandHandler> logger) : IRequestHandler<RevokePermissionFromRoleCommand, Result>
    {
        public async Task<Result> Handle(RevokePermissionFromRoleCommand request, CancellationToken ct)
        {
            using var scope = logger.BeginScope(new Dictionary<string, object> { ["RoleId"] = request.RoleId, ["PermissionId"] = request.PermissionId, ["ActorUserId"] = request.ActorUserId });
            logger.LogInformation("Attempting to revoke permission {PermissionId} from role {RoleId}", request.PermissionId, request.RoleId);

            // Check if role exists
            var role = await rolesRepository.GetByIdAsync(request.RoleId, ct);
            if (role == null)
            {
                logger.LogWarning("Role with ID {RoleId} not found", request.RoleId);
                return Result.Failure(new Error(ErrorCode.NotFound, "Role not found"));
            }

            // Check if permission exists
            var permission = await permissionsRepository.GetPermissionsByIdAsync(request.PermissionId, ct);
            if (permission == null)
            {
                logger.LogWarning("Permission with ID {PermissionId} not found", request.PermissionId);
                return Result.Failure(new Error(ErrorCode.NotFound, "Permission not found"));
            }

            await unitOfWork.BeginTransactionAsync(ct);
            try
            {
                // Revoke permission from role
                await rolesRepository.RemovePermissionFromRoleAsync(request.RoleId, request.PermissionId, request.ActorUserId, ct);
                await unitOfWork.CommitAsync(ct);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while revoking permission {PermissionId} from role {RoleId}", request.PermissionId, request.RoleId);
                await unitOfWork.RollbackAsync(ct);
                return Result.Failure(new Error(ErrorCode.InternalServerError, "Failed to revoke permission from role"));
            }

            logger.LogInformation("Permission {PermissionId} revoked from role {RoleId} successfully", request.PermissionId, request.RoleId);
            return Result.Success();
        }
    }
}