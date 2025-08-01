namespace AWMService.Domain.Entities
{
    public class EvaluationScores
    {
        public int EvaluationScoresId { get; set; }
        
        public int DefenseGradeId { get; set; }
        public int CriteriaId { get; set; }
        public int CommissionMemberId { get; set; }
        public int ScoreValue { get; set; }
        public string Comment { get; set; }

        public DefenseGrades DefenseGrade { get; set; }
        public EvaluationCriteria Criteria { get; set; }
        public Users CommissionMember { get; set; }


    }
}
