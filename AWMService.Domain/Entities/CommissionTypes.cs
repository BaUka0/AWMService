namespace AWMService.Domain.Entities
{
    public class CommissionTypes
    {
        public int CommissionTypesId { get; set; }
        public string Name { get; set; }
        public List<Commissions> Commissions { get; set; }
        public List<EvaluationCriteria> EvaluationCriteria { get; set; }
    }
}
