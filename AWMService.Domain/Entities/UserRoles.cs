namespace AWMService.Domain.Entities
{
    public class UserRoles
    {
        public int UserId { get; set; }
        public Users User { get; set; } = null!;

        public int RoleId { get; set; }
        public Roles Role { get; set; } = null!;


        public DateTime AssignedOn { get; set; }
        public int AssignedBy { get; set; }
        public DateTime? RevokedOn { get; set; }
        public int? RevokedBy { get; set; }
    }
}
