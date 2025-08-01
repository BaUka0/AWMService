
using System.ComponentModel.DataAnnotations;

namespace AWMService.Domain.Entities
{
    public class Roles
    {
        public int RoleId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }


        public List<UserRoles> UserRoles { get; set; }
        public List<RolePermissions> RolePermissions { get; set; }
    }
}
