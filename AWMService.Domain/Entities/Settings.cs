using AWMService.Domain.Commons;

namespace AWMService.Domain.Entities
{
    public class Settings : AuditableEntity
    {
        public string SettingKey { get; set; } = null!;
        public string SettingValue { get; set; } = null!;
        public string? Description { get; set; }
    }
}
