
using AWMService.Domain.Commons;
namespace AWMService.Domain.Entities
{
    public class Users : EntityBase
    {
        public DateTime CreatedOn { get; set; }

        public int UserTypeId { get; set; }
        public UserTypes UserType { get; set; } = null!;

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? SurName { get; set; }
        public string Email { get; set; } = null!;
        public string Login { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? IIN { get; set; }

        public string PasswordHash { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        public int? DepartmentId { get; set; }
        public Departments? Department { get; set; }


        public ICollection<UserRoles> UserRoles { get; set; } = new List<UserRoles>();
    }
}
