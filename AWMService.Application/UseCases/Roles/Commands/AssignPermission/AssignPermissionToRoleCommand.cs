using KDS.Primitives.FluentResult;
using MediatR;

namespace AWMService.Application.UseCases.Roles.Commands.AssignPermission
{
    public class AssignPermissionToRoleCommand : IRequest<Result>
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
        public int ActorUserId { get; set; }
    }
}