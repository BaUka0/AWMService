using Microsoft.AspNetCore.Authorization;

namespace AWMService.WebAPI.Authorization
{
    public class HasPermissionAttribute : AuthorizeAttribute
    {
        public HasPermissionAttribute(string permission)
            : base(policy: permission)
        {
        }
    }
}
