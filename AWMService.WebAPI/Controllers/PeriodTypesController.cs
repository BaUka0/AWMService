using AWMService.Application.UseCases.PeriodType.Commands.AddPeriodType;
using AWMService.Application.UseCases.PeriodType.Commands.DeletePeriodType;
using AWMService.Application.UseCases.PeriodType.Commands.UpdatePeriodType;
using AWMService.Application.UseCases.PeriodType.Queries.GetPeriodTypeById;
using AWMService.Application.UseCases.PeriodType.Queries.ListPeriodTypes;
using AWMService.WebAPI.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AWMService.WebAPI.Controllers
{
    [ApiController]
    [Route("api/period-types")]
    public class PeriodTypesController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PeriodTypesController> _logger;

        public PeriodTypesController(IMediator mediator, ILogger<PeriodTypesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> ListAll(CancellationToken ct)
        {
            _logger.LogInformation("ListAll PeriodTypes endpoint triggered.");
            var result = await _mediator.Send(new ListPeriodTypesQuery(), ct);
            return Ok(result.Value);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            _logger.LogInformation("GetById PeriodType endpoint triggered for Id={Id}.", id);
            var result = await _mediator.Send(new GetPeriodTypeByIdQuery(id), ct);
            if (result.IsSuccess)
                return Ok(result.Value);
            return GenerateProblemResponse(result.Error);
        }

        [HttpPost]
        [HasPermission("manage_period_types")]
        public async Task<IActionResult> Add([FromBody] AddPeriodTypeCommand command, CancellationToken ct)
        {
            _logger.LogInformation("Add PeriodType endpoint triggered.");
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out var actorUserId))
            {
                return Unauthorized();
            }
            command.ActorUserId = actorUserId;

            var result = await _mediator.Send(command, ct);

            if (result.IsSuccess)
                return CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value);

            return GenerateProblemResponse(result.Error);
        }

        [HttpPut("{id:int}")]
        [HasPermission("manage_period_types")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePeriodTypeCommand command, CancellationToken ct)
        {
            _logger.LogInformation("Update PeriodType endpoint triggered for Id={Id}.", id);

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
        [HasPermission("manage_period_types")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            _logger.LogInformation("Delete PeriodType endpoint triggered for Id={Id}.", id);
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out var actorUserId))
            {
                return Unauthorized();
            }

            var command = new DeletePeriodTypeCommand { Id = id, ActorUserId = actorUserId };
            var result = await _mediator.Send(command, ct);

            if (result.IsSuccess)
                return NoContent();

            return GenerateProblemResponse(result.Error);
        }
    }
}
