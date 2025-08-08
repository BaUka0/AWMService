using AWMService.Domain.Commons;

namespace AWMService.Domain.Entities
{
    public class CheckTypes : AuditableSoftDeletableEntity
    {
        public string Name { get; set; } = null!;
        public ICollection<WorkChecks> WorkChecks { get; set; } = new List<WorkChecks>();
    }
}
