namespace AWMService.Domain.Entities
{
    public class DefenseGrades
    {
        public int DefenseGradeId { get; set; }
        public int DefenseScheduledId { get; set; }
        public float FinalScore { get; set; }
        public float FinalGrade { get; set; }
        public int StatusId { get; set; }

        public DefenseSchedules DefenseScheduled { get; set; }
        public Statuses Status { get; set; }

        public List<EvaluationScores> EvaluationScores { get; set; } 
    }
}
