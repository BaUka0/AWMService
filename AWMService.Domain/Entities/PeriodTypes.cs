using AWMService.Domain.Commons;

namespace AWMService.Domain.Entities
{
    public class PeriodTypes : AuditableSoftDeletableEntity
    {
        public string Name { get; set; } = null!;
        public ICollection<Periods> Periods { get; set; } = new List<Periods>();
    }
}
