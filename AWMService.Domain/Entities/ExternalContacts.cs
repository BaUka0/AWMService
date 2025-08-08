using AWMService.Domain.Commons;

namespace AWMService.Domain.Entities
{
    public class ExternalContacts : AuditableSoftDeletableEntity
    {
        public string FullName { get; set; } = null!;
        public string? Position { get; set; }
        public string? Organization { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}