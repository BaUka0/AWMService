using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AWMService.Application.UseCases.AcademicYears.Queries.ListAcademicYears;
using AWMService.Application.UseCases.AcademicYears.Queries.GetById;
using AWMService.Application.UseCases.AcademicYears.Queries.GetByDate;
using AWMService.Application.UseCases.AcademicYears.Commands.AddAcademicYear;
using AWMService.Application.UseCases.AcademicYears.Commands.DeleteAcademicYear;
using AWMService.WebAPI.Authorization;

namespace AWMService.WebAPI.Controllers
{
    [ApiController]
    [Route("api/academic-years")]
    public class AcademicYearsController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AcademicYearsController> _logger;

        public AcademicYearsController(IMediator mediator, ILogger<AcademicYearsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        
        [HttpGet]
        public async Task<IActionResult> ListAll(CancellationToken ct)
        {
            _logger.LogInformation("ListAll AcademicYears endpoint triggered.");

            var result = await _mediator.Send(new ListAcademicYearsQuery(), ct);

            if (result.IsSuccess)
                return Ok(result.Value);

            _logger.LogWarning("ListAll AcademicYears failed: {Error}", result.Error);
            return GenerateProblemResponse(result.Error);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            _logger.LogInformation("GetById AcademicYear endpoint triggered for Id={Id}.", id);

            var result = await _mediator.Send(new GetAcademicYearByIdQuery(id), ct);

            if (result.IsSuccess)
                return Ok(result.Value);

            _logger.LogWarning("GetById AcademicYear {Id} failed: {Error}", id, result.Error);
            return GenerateProblemResponse(result.Error);
        }

        [HttpGet("by-date")]
        public async Task<IActionResult> GetByDate([FromQuery] DateTime date, CancellationToken ct)
        {
            _logger.LogInformation("GetByDate AcademicYear endpoint triggered for Date={Date}.", date);

            var result = await _mediator.Send(new GetAcademicYearByDateQuery(date), ct);

            if (result.IsSuccess)
                return Ok(result.Value);

            _logger.LogWarning("GetByDate AcademicYear for {Date} failed: {Error}", date, result.Error);
            return GenerateProblemResponse(result.Error);
        }

        [HttpPost]
        [HasPermission("manage_academic_years")]
        public async Task<IActionResult> Add([FromBody] AddAcademicYearCommand command, CancellationToken ct)
        {
            _logger.LogInformation("Add AcademicYear endpoint triggered.");

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out var actorUserId))
            {
                _logger.LogWarning("Add AcademicYear failed: Could not parse user ID from claims.");
                return Unauthorized();
            }
            command.ActorUserId = actorUserId;

            var result = await _mediator.Send(command, ct);

            if (result.IsSuccess)
                return NoContent();

            _logger.LogWarning("Add AcademicYear failed: {Error}", result.Error);
            return GenerateProblemResponse(result.Error);
        }

        [HttpDelete("{id:int}")]
        [HasPermission("manage_academic_years")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            _logger.LogInformation("Delete AcademicYear endpoint triggered for Id={Id}.", id);

            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out var actorUserId))
            {
                _logger.LogWarning("Delete AcademicYear failed: Could not parse user ID from claims.");
                return Unauthorized();
            }

            var command = new DeleteAcademicYearCommand
            {
                Id = id,
                ActorUserId = actorUserId
            };

            var result = await _mediator.Send(command, ct);

            if (result.IsSuccess)
                return NoContent();

            _logger.LogWarning("Delete AcademicYear {Id} failed: {Error}", id, result.Error);
            return GenerateProblemResponse(result.Error);
        }
    }
}
