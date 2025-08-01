
namespace AWMService.Domain.Entities
{
    public class Commissions
    {
        public int CommissionId { get; set; }
        public string Name { get; set; }
        public int CommissionTypeId { get; set; }
        public int SecretaryId { get; set; }
        public int PeriodId { get; set; }
        public int DepartmentId { get; set; }
        public DateTime CreatedOn { get; set; }

        public Periods Periods { get; set; }
        public CommissionTypes CommissionTypes { get; set; }
        public Users Secretary { get; set; }
        public Department Department { get; set; }

        public List<CommissionMembers> CommissionMembers { get; set; } 
        public List<DefenseSchedules> DefenseSchedules { get; set; }
    }
}
