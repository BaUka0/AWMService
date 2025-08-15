using AWMService.Application.UseCases.Auth.Commands.Login;
using AWMService.Application.UseCases.Auth.Commands.Logout;
using AWMService.Application.UseCases.Auth.Commands.Register;
using AWMService.Application.UseCases.Auth.Commands.RefreshToken;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AWMService.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        public readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken ct)
        {
            var result = await _mediator.Send(command, ct);
            if (result.IsSuccess)
                return Ok(result.Value);
            return GenerateProblemResponse(result.Error);
        }
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command, CancellationToken ct)
        {
            var result = await _mediator.Send(command, ct);
            if (result.IsSuccess)
                return Ok(result.Value);
            return GenerateProblemResponse(result.Error);
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command, CancellationToken ct)
        {
            var result = await _mediator.Send(command, ct);
            if (result.IsSuccess)
                return Ok(result.Value);
            return GenerateProblemResponse(result.Error);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout(CancellationToken ct)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            var command = new LogoutCommand { UserId = userId };
            var result = await _mediator.Send(command, ct);

            if (result.IsSuccess)
                return Ok();
            
            return GenerateProblemResponse(result.Error);
        }
    }
}
