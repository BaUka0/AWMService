
using AWMService.Domain.Commons;

namespace AWMService.Domain.Entities
{
    public class Roles : EntityBase
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public ICollection<UserRoles> UserRoles { get; set; } = new List<UserRoles>();
        public ICollection<RolePermissions> RolePermissions { get; set; } = new List<RolePermissions>();
    }
}
