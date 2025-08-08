namespace AWMService.Domain.Entities
{
    public class RolePermissions
    {
        public int RoleId { get; set; }
        public Roles Role { get; set; } = null!;

        public int PermissionId { get; set; }
        public Permissions Permission { get; set; } = null!;


        public DateTime AssignedOn { get; set; }
        public int AssignedBy { get; set; }
        public DateTime? RevokedOn { get; set; }
        public int? RevokedBy { get; set; }
    }
}
