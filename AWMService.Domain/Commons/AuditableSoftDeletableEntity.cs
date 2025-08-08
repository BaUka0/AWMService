using AWMService.Domain.Abstractions;

namespace AWMService.Domain.Commons
{
    public abstract class AuditableSoftDeletableEntity : AuditableEntity, ISoftDelete
    {
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public int? DeletedBy { get; set; }
    }
}
