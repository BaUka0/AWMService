namespace AWMService.Domain.Entities
{
    public class UserRoles
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public Users Users { get; set; }
        public Roles Roles { get; set; }
    }
}
