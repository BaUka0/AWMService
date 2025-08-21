using AWMService.Application.UseCases.Supervisors.Commands.ApproveSupervisors;
using AWMService.Application.UseCases.Supervisors.Commands.RevokeSupervisors;
using AWMService.Application.UseCases.Supervisors.Queries.GetTeachers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AWMService.WebAPI.Controllers
{
    [ApiController]
    [Route("api/supervisors")]
    [Authorize(Policy = "ManageSupervisors")]
    public class DepartmentSupervisorsController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<DepartmentSupervisorsController> _logger;

        public DepartmentSupervisorsController(IMediator mediator, ILogger<DepartmentSupervisorsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("departments/{departmentId:int}/teachers")]
        public async Task<IActionResult> GetTeachers(
            int departmentId,
            [FromQuery] int academicYearId,
            CancellationToken ct)
        {
            _logger.LogInformation("GetTeachers endpoint triggered for DepartmentId {DepartmentId} and AcademicYearId {AcademicYearId}", departmentId, academicYearId);

            var query = new GetTeachersQuery
            {
                DepartmentId = departmentId,
                AcademicYearId = academicYearId
            };

            var result = await _mediator.Send(query, ct);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            _logger.LogWarning("GetTeachers for DepartmentId {DepartmentId} failed with error: {Error}", departmentId, result.Error);
            return GenerateProblemResponse(result.Error);
        }

        [HttpPost("approvals/bulk")]
        public async Task<IActionResult> ApproveBulk([FromBody] ApproveSupervisorsCommand command, CancellationToken ct)
        {
            _logger.LogInformation("ApproveBulk endpoint triggered.");

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out var actorUserId))
            {
                _logger.LogWarning("ApproveBulk failed: Could not parse user ID from claims.");
                return Unauthorized();
            }
            command.ActorUserId = actorUserId;

            var result = await _mediator.Send(command, ct);

            if (result.IsSuccess)
            {
                return NoContent();
            }

            _logger.LogWarning("ApproveBulk failed with error: {Error}", result.Error);
            return GenerateProblemResponse(result.Error);
        }

        [HttpDelete("approvals/bulk")]
        public async Task<IActionResult> RevokeBulk([FromBody] RevokeSupervisorsCommand command, CancellationToken ct)
        {
            _logger.LogInformation("RevokeBulk endpoint triggered.");

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out var actorUserId))
            {
                _logger.LogWarning("RevokeBulk failed: Could not parse user ID from claims.");
                return Unauthorized();
            }
            command.ActorUserId = actorUserId;

            var result = await _mediator.Send(command, ct);

            if (result.IsSuccess)
            {
                return NoContent();
            }

            _logger.LogWarning("RevokeBulk failed with error: {Error}", result.Error);
            return GenerateProblemResponse(result.Error);
        }
    }
}
