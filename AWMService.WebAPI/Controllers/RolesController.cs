using AWMService.Application.UseCases.Roles.Commands.AssignPermission;
using AWMService.Application.UseCases.Roles.Commands.RevokePermission;
using AWMService.Application.UseCases.Roles.Commands.AssignRoleToUser;
using AWMService.Application.UseCases.Roles.Commands.RevokeRoleFromUser;
using AWMService.WebAPI.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AWMService.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : BaseController
    {
        private readonly IMediator _mediator;

        public RolesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("{roleId}/permissions/{permissionId}")]
        [HasPermission("manage_permissions")]
        public async Task<IActionResult> AssignPermission(int roleId, int permissionId, CancellationToken ct)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out var actorUserId))
            {
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
                return NoContent();
            }

            return GenerateProblemResponse(result.Error);
        }

        [HttpDelete("{roleId}/permissions/{permissionId}")]
        [HasPermission("manage_permissions")]
        public async Task<IActionResult> RevokePermission(int roleId, int permissionId, CancellationToken ct)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out var actorUserId))
            {
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
                return NoContent();
            }

            return GenerateProblemResponse(result.Error);
        }

        [HttpPost("{roleId}/users/{userId}")]
        [HasPermission("manage_user_roles")]
        public async Task<IActionResult> AssignRoleToUser(int roleId, int userId, CancellationToken ct)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out var actorUserId))
            {
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
                return NoContent();
            }

            return GenerateProblemResponse(result.Error);
        }

        [HttpDelete("{roleId}/users/{userId}")]
        [HasPermission("manage_user_roles")]
        public async Task<IActionResult> RevokeRoleFromUser(int roleId, int userId, CancellationToken ct)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out var actorUserId))
            {
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
                return NoContent();
            }

            return GenerateProblemResponse(result.Error);
        }
    }
}