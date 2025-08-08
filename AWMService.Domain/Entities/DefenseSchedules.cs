using AWMService.Domain.Commons;

namespace AWMService.Domain.Entities
{
    public class DefenseSchedules : AuditableSoftDeletableEntity
    {
        public int CommissionId { get; set; }
        public Commissions Commission { get; set; } = null!;

        public int StudentWorkId { get; set; }
        public StudentWork StudentWork { get; set; } = null!;

        public DateTime DefenseDate { get; set; }
        public string? Location { get; set; }

        public ICollection<DefenseGrades> DefenseGrades { get; set; } = new List<DefenseGrades>();
    }
}
