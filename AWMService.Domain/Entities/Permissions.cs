
using AWMService.Domain.Commons;


namespace AWMService.Domain.Entities
{
    public class Permissions : AuditableSoftDeletableEntity
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
