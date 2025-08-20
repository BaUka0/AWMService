using KDS.Primitives.FluentResult;
using MediatR;

namespace AWMService.Application.UseCases.Roles.Commands.RevokeRoleFromUser
{
    public sealed record RevokeRoleFromUserCommand : IRequest<Result>
    {
        public int RoleId { get; set; }
        public int UserId { get; set; }
        public int ActorUserId { get; set; }
    }
}
