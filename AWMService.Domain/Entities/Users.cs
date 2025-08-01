
using System.ComponentModel.DataAnnotations;
namespace AWMService.Domain.Entities
{
    public class Users
    {
        public int UserId { get; set; }
        public DateTime CreatedOn { get; set; }
        public int UserTypeId { get; set; } 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? SurName { get; set; } 

        [Required]
        public string Email { get; set; }
        [Required]
        public string Login { get; set; }

        public string PhoneNumber { get; set; }
        public string IIN { get; set; }
        public int DepartmentId { get; set; }


        public Department Department { get; set; } 
        public UserType UserType { get; set; }
        public List<UserRoles> UserRoles { get; set; } 
        public List<Periods> Periods { get; set; }
        public List<Commissions> Commissions { get; set; }
        public List<Directions> Directions { get; set; }
        public List<StudentWork> StudentWorks { get; set; }
        public List<CommissionMembers> CommissionMembers { get; set; }
        public List<Topics> Topics { get; set; }
        public List<Applications> Applications { get; set; }
        public List<DepartmentExperts> DepartmentExperts { get; set; }
        public List<DepartmentExperts> AssignedDepartmentExperts { get; set; }
        public List<EvaluationScores> EvaluationScores { get; set; }
        public List<WorkChecks> WorkChecks { get; set; }
        public List<Attachments> Attachments { get; set; }
        public List<SupervisorApprovals> SupervisorApprovals { get; set; }
        public List<SupervisorApprovals> ApprovedByUsers { get; set; }

    }
}
