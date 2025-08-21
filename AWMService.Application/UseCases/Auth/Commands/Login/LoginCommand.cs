using AWMService.Application.DTOs;
using KDS.Primitives.FluentResult;
using MediatR;

namespace AWMService.Application.UseCases.Auth.Commands.Login
{
    public sealed record LoginCommand : IRequest<Result<AuthResult>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
