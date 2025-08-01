
using System.ComponentModel.DataAnnotations;


namespace AWMService.Domain.Entities
{
    public class Permissions
    {
        public int PermissionId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        public List<RolePermissions> RolePermissions { get; set; }

    }
}
