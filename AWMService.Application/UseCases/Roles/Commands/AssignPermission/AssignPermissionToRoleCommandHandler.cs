using AWMService.Application.Abstractions.Data;
using AWMService.Application.Abstractions.Repositories;
using AWMService.Domain.Constants;
using KDS.Primitives.FluentResult;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AWMService.Application.UseCases.Roles.Commands.AssignPermission
{
    public class AssignPermissionToRoleCommandHandler(
        IRolesRepository rolesRepository,
        IPermissionsRepository permissionsRepository,
        IUnitOfWork unitOfWork,
        ILogger<AssignPermissionToRoleCommandHandler> logger) : IRequestHandler<AssignPermissionToRoleCommand, Result>
    {
        public async Task<Result> Handle(AssignPermissionToRoleCommand request, CancellationToken ct)
        {
            using var scope = logger.BeginScope(new Dictionary<string, object> { ["RoleId"] = request.RoleId, ["PermissionId"] = request.PermissionId, ["ActorUserId"] = request.ActorUserId });
            logger.LogInformation("Attempting to assign permission {PermissionId} to role {RoleId}", request.PermissionId, request.RoleId);

            var role = await rolesRepository.GetByIdAsync(request.RoleId, ct);
            if (role == null)
            {
                logger.LogWarning("Role with ID {RoleId} not found", request.RoleId);
                return Result.Failure(new Error(ErrorCode.NotFound, "Role not found"));
            }

            var permission = await permissionsRepository.GetPermissionsByIdAsync(request.PermissionId, ct);
            if (permission == null)
            {
                logger.LogWarning("Permission with ID {PermissionId} not found", request.PermissionId);
                return Result.Failure(new Error(ErrorCode.NotFound, "Permission not found"));
            }

            await unitOfWork.BeginTransactionAsync(ct);
            try
            {
                await rolesRepository.AddPermissionToRoleAsync(request.RoleId, request.PermissionId, request.ActorUserId, ct);
                await unitOfWork.CommitAsync(ct);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while assigning permission {PermissionId} to role {RoleId}", request.PermissionId, request.RoleId);
                await unitOfWork.RollbackAsync(ct);
                return Result.Failure(new Error(ErrorCode.InternalServerError, "Failed to assign permission to role"));
            }

            logger.LogInformation("Permission {PermissionId} assigned to role {RoleId} successfully", request.PermissionId, request.RoleId);
            return Result.Success();
        }
    }
}