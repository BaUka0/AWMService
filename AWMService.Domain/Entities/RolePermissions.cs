namespace AWMService.Domain.Entities
{
    public class RolePermissions
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }

        public Roles Role { get; set; }
        public Permissions Permission { get; set; }

    }
}
