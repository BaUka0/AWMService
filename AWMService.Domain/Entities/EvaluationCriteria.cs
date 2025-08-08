using AWMService.Domain.Commons;

namespace AWMService.Domain.Entities
{
    public class EvaluationCriteria : AuditableSoftDeletableEntity
    {
        public string Name { get; set; } = null!;
        public int CommissionTypeId { get; set; }
        public CommissionTypes CommissionType { get; set; } = null!;

        public ICollection<EvaluationScores> EvaluationScores { get; set; } = new List<EvaluationScores>();
    }
}
