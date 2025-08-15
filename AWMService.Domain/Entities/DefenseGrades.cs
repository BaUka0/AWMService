using AWMService.Domain.Commons;

namespace AWMService.Domain.Entities
{
    public class DefenseGrades : AuditableEntity
    {
        public int DefenseScheduleId { get; set; }
        public DefenseSchedules DefenseSchedule { get; set; } = null!;

        public double FinalScore { get; set; }
        public string? FinalGrade { get; set; }

        public int StatusId { get; set; }
        public Statuses Status { get; set; } = null!;

        public ICollection<EvaluationScores> EvaluationScores { get; set; } = new List<EvaluationScores>();
    }
}
