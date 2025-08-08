using AWMService.Domain.Abstractions;

namespace AWMService.Domain.Commons
{
    public abstract class AuditableEntity : EntityBase, IHasAudit
    {
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
