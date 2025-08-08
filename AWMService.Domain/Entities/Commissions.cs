
using AWMService.Domain.Commons;

namespace AWMService.Domain.Entities
{
    public class Commissions : AuditableSoftDeletableEntity
    {
        public string Name { get; set; } = null!;
        public int CommissionTypeId { get; set; }
        public CommissionTypes CommissionType { get; set; } = null!;

        public int SecretaryId { get; set; }
        public Users Secretary { get; set; } = null!;

        public int PeriodId { get; set; }
        public Periods Period { get; set; } = null!;

        public int DepartmentId { get; set; }
        public Departments Department { get; set; } = null!;

        public ICollection<CommissionMembers> Members { get; set; } = new List<CommissionMembers>();
    }
}
