using AWMService.Domain.Commons;

namespace AWMService.Domain.Entities
{
    public class CommissionTypes : AuditableSoftDeletableEntity
    {
        public string Name { get; set; } = null!;
        public ICollection<Commissions> Commissions { get; set; } = new List<Commissions>();
    }
}
