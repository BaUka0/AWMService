using AWMService.Application.UseCases.Periods.Commands.AddPeriod;
using AWMService.Application.UseCases.Periods.Commands.DeletePeriod;
using AWMService.Application.UseCases.Periods.Commands.UpdatePeriod;
using AWMService.Application.UseCases.Periods.Queries.GetPeriodById;
using AWMService.Application.UseCases.Periods.Queries.ListPeriods;
using AWMService.WebAPI.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AWMService.WebAPI.Controllers
{
    [ApiController]
    [Route("api/periods")]
    public class PeriodsController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PeriodsController> _logger;

        public PeriodsController(IMediator mediator, ILogger<PeriodsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> ListAll(CancellationToken ct)
        {
            _logger.LogInformation("ListAll Periods endpoint triggered.");
            var result = await _mediator.Send(new ListPeriodsQuery(), ct);
            return Ok(result.Value);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            _logger.LogInformation("GetById Period endpoint triggered for Id={Id}.", id);
            var result = await _mediator.Send(new GetPeriodByIdQuery { Id = id }, ct);
            if (result.IsSuccess)
                return Ok(result.Value);
            return GenerateProblemResponse(result.Error);
        }

        [HttpPost]
        [HasPermission("manage_periods")]
        public async Task<IActionResult> Add([FromBody] AddPeriodCommand command, CancellationToken ct)
        {
            _logger.LogInformation("Add Period endpoint triggered.");
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out var actorUserId))
            {
                return Unauthorized();
            }
            command.ActorUserId = actorUserId;

            var result = await _mediator.Send(command, ct);

            if (result.IsSuccess)
                return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);

            return GenerateProblemResponse(result.Error);
        }

        [HttpPut("{id:int}")]
        [HasPermission("manage_periods")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePeriodCommand command, CancellationToken ct)
        {
            _logger.LogInformation("Update Period endpoint triggered for Id={Id}.", id);

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out var actorUserId))
            {
                return Unauthorized();
            }
            command.ActorUserId = actorUserId;
            command.Id = id;

            var result = await _mediator.Send(command, ct);

            if (result.IsSuccess)
                return NoContent();

            return GenerateProblemResponse(result.Error);
        }

        [HttpDelete("{id:int}")]
        [HasPermission("manage_periods")]
        public async Task<IActionResult> Delete(int id, [FromBody] DeletePeriodCommand command, CancellationToken ct)
        {
            _logger.LogInformation("Delete Period endpoint triggered for Id={Id}.", id);

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out var actorUserId))
            {
                return Unauthorized();
            }
            command.ActorUserId = actorUserId;
            command.Id = id;

            var result = await _mediator.Send(command, ct);

            if (result.IsSuccess)
                return NoContent();

            return GenerateProblemResponse(result.Error);
        }
    }
}
