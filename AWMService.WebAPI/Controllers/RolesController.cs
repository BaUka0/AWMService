using AWMService.Application.UseCases.Roles.Commands.AssignPermission;
using AWMService.Application.UseCases.Roles.Commands.RevokePermission;
using AWMService.Application.UseCases.Roles.Commands.AssignRoleToUser;
using AWMService.Application.UseCases.Roles.Commands.RevokeRoleFromUser;
using AWMService.WebAPI.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace AWMService.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<RolesController> _logger;

        public RolesController(IMediator mediator, ILogger<RolesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("{roleId}/permissions/{permissionId}")]
        [HasPermission("manage_permissions")]
        public async Task<IActionResult> AssignPermission(int roleId, int permissionId, CancellationToken ct)
        {
            _logger.LogInformation("AssignPermission endpoint triggered for RoleId {RoleId} and PermissionId {PermissionId}.", roleId, permissionId);

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out var actorUserId))
            {
                _logger.LogWarning("AssignPermission failed: Could not parse user ID from claims.");
                return Unauthorized();
            }

            var command = new AssignPermissionToRoleCommand
            {
                RoleId = roleId,
                PermissionId = permissionId,
                ActorUserId = actorUserId
            };

            var result = await _mediator.Send(command, ct);

            if (result.IsSuccess)
            {
                _logger.LogInformation("Permission {PermissionId} assigned to role {RoleId} successfully.", permissionId, roleId);
                return NoContent();
            }

            _logger.LogWarning("AssignPermission failed for RoleId {RoleId} and PermissionId {PermissionId}: {Error}", roleId, permissionId, result.Error);
            return GenerateProblemResponse(result.Error);
        }

        [HttpDelete("{roleId}/permissions/{permissionId}")]
        [HasPermission("manage_permissions")]
        public async Task<IActionResult> RevokePermission(int roleId, int permissionId, CancellationToken ct)
        {
            _logger.LogInformation("RevokePermission endpoint triggered for RoleId {RoleId} and PermissionId {PermissionId}.", roleId, permissionId);

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out var actorUserId))
            {
                _logger.LogWarning("RevokePermission failed: Could not parse user ID from claims.");
                return Unauthorized();
            }

            var command = new RevokePermissionFromRoleCommand
            {
                RoleId = roleId,
                PermissionId = permissionId,
                ActorUserId = actorUserId
            };

            var result = await _mediator.Send(command, ct);

            if (result.IsSuccess)
            {
                _logger.LogInformation("Permission {PermissionId} revoked from role {RoleId} successfully.", permissionId, roleId);
                return NoContent();
            }

            _logger.LogWarning("RevokePermission failed for RoleId {RoleId} and PermissionId {PermissionId}: {Error}", roleId, permissionId, result.Error);
            return GenerateProblemResponse(result.Error);
        }

        [HttpPost("{roleId}/users/{userId}")]
        [HasPermission("manage_user_roles")]
        public async Task<IActionResult> AssignRoleToUser(int roleId, int userId, CancellationToken ct)
        {
            _logger.LogInformation("AssignRoleToUser endpoint triggered for RoleId {RoleId} and UserId {UserId}.", roleId, userId);

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out var actorUserId))
            {
                _logger.LogWarning("AssignRoleToUser failed: Could not parse user ID from claims.");
                return Unauthorized();
            }

            var command = new AssignRoleToUserCommand
            {
                RoleId = roleId,
                UserId = userId,
                ActorUserId = actorUserId
            };

            var result = await _mediator.Send(command, ct);

            if (result.IsSuccess)
            {
                _logger.LogInformation("Role {RoleId} assigned to user {UserId} successfully.", roleId, userId);
                return NoContent();
            }

            _logger.LogWarning("AssignRoleToUser failed for RoleId {RoleId} and UserId {UserId}: {Error}", roleId, userId, result.Error);
            return GenerateProblemResponse(result.Error);
        }

        [HttpDelete("{roleId}/users/{userId}")]
        [HasPermission("manage_user_roles")]
        public async Task<IActionResult> RevokeRoleFromUser(int roleId, int userId, CancellationToken ct)
        {
            _logger.LogInformation("RevokeRoleFromUser endpoint triggered for RoleId {RoleId} and UserId {UserId}.", roleId, userId);

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out var actorUserId))
            {
                _logger.LogWarning("RevokeRoleFromUser failed: Could not parse user ID from claims.");
                return Unauthorized();
            }

            var command = new RevokeRoleFromUserCommand
            {
                RoleId = roleId,
                UserId = userId,
                ActorUserId = actorUserId
            };

            var result = await _mediator.Send(command, ct);

            if (result.IsSuccess)
            {
                _logger.LogInformation("Role {RoleId} revoked from user {UserId} successfully.", roleId, userId);
                return NoContent();
            }

            _logger.LogWarning("RevokeRoleFromUser failed for RoleId {RoleId} and UserId {UserId}: {Error}", roleId, userId, result.Error);
            return GenerateProblemResponse(result.Error);
        }
    }
}