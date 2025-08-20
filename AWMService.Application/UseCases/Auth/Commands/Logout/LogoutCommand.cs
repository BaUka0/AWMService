using KDS.Primitives.FluentResult;
using MediatR;

namespace AWMService.Application.UseCases.Auth.Commands.Logout
{
    public sealed record LogoutCommand : IRequest<Result>
    {
        public int UserId { get; set; }
    }
}
