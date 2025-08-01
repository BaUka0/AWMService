namespace AWMService.Domain.Entities
{
    public class PeriodTypes
    {
        public int PeriodTypeId { get; set; }
        public string Name { get; set; }

        public List<Periods> Periods { get; set; } 
    }
}
