using AWMService.Application.DTOs;
using KDS.Primitives.FluentResult;
using MediatR;

namespace AWMService.Application.UseCases.Auth.Commands.RefreshToken
{
    public sealed record RefreshTokenCommand : IRequest<Result<AuthResult>>
    {
        public string RefreshToken { get; set; } = null!;
    }
}
