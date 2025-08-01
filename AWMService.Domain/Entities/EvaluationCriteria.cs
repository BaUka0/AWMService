namespace AWMService.Domain.Entities
{
    public class EvaluationCriteria
    {
        public int EvaluationCriteriaId { get; set; }
        public string Name { get; set; }

        public int CommissionTypeId { get; set; }

        public CommissionTypes CommissionType { get; set; }
        public List<EvaluationScores> EvaluationScores { get; set; }
    }
}
