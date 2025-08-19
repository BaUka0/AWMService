using KDS.Primitives.FluentResult;
using MediatR;

namespace AWMService.Application.UseCases.Roles.Commands.AssignRoleToUser
{
    public class AssignRoleToUserCommand : IRequest<Result>
    {
        public int RoleId { get; set; }
        public int UserId { get; set; }
        public int ActorUserId { get; set; }
    }
}
