using AWMService.Application.UseCases.Auth.Commands.Login;
using AWMService.Application.UseCases.Auth.Commands.Logout;
using AWMService.Application.UseCases.Auth.Commands.Register;
using AWMService.Application.UseCases.Auth.Commands.RefreshToken;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace AWMService.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IMediator mediator, ILogger<AuthController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken ct)
        {
            _logger.LogInformation("Login endpoint triggered for email {Email}", command.Email);
            var result = await _mediator.Send(command, ct);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            _logger.LogWarning("Login for {Email} failed with error: {Error}", command.Email, result.Error);
            return GenerateProblemResponse(result.Error);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command, CancellationToken ct)
        {
            _logger.LogInformation("Register endpoint triggered for email {Email}", command.Email);
            var result = await _mediator.Send(command, ct);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            _logger.LogWarning("Registration for {Email} failed with error: {Error}", command.Email, result.Error);
            return GenerateProblemResponse(result.Error);
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command, CancellationToken ct)
        {
            _logger.LogInformation("RefreshToken endpoint triggered.");
            var result = await _mediator.Send(command, ct);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            _logger.LogWarning("RefreshToken failed with error: {Error}", result.Error);
            return GenerateProblemResponse(result.Error);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout(CancellationToken ct)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out var userId))
            {
                _logger.LogWarning("Logout failed: Could not parse user ID from claims.");
                return Unauthorized();
            }

            _logger.LogInformation("Logout endpoint triggered for user ID {UserId}", userId);
            var command = new LogoutCommand { UserId = userId };
            var result = await _mediator.Send(command, ct);

            if (result.IsSuccess)
            {
                return Ok();
            }
            
            _logger.LogWarning("Logout for user ID {UserId} failed with error: {Error}", userId, result.Error);
            return GenerateProblemResponse(result.Error);
        }
    }
}
