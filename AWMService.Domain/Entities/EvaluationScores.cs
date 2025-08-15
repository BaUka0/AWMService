using AWMService.Domain.Commons;

namespace AWMService.Domain.Entities
{
    public class EvaluationScores : AuditableEntity
    {
        public int DefenseGradeId { get; set; }
        public DefenseGrades DefenseGrade { get; set; } = null!;

        public int CriteriaId { get; set; }
        public EvaluationCriteria Criteria { get; set; } = null!;

        public int CommissionMemberId { get; set; }
        public CommissionMembers CommissionMember { get; set; } = null!;

        public int ScoreValue { get; set; }
        public string? Comment { get; set; }
    }
}
