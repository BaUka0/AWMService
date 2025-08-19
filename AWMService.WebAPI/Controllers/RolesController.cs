using AWMService.Application.UseCases.Roles.Commands.AssignPermission;
using AWMService.Application.UseCases.Roles.Commands.RevokePermission;
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
    }
}