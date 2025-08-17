using AWMService.Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace AWMService.WebAPI.Authorization
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User.HasClaim(c => c.Type == PermissionClaimTypes.Permission && c.Value == requirement.Permission))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
