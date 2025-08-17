using KDS.Primitives.FluentResult;
using MediatR;

namespace AWMService.Application.UseCases.Roles.Commands.RevokePermission
{
    public class RevokePermissionFromRoleCommand : IRequest<Result>
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
        public int ActorUserId { get; set; }
    }
}