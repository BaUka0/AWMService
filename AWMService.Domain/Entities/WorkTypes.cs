using AWMService.Domain.Commons;

namespace AWMService.Domain.Entities
{
    public class WorkTypes : AuditableSoftDeletableEntity
    {
        public string Name { get; set; } = null!;
        public ICollection<StudentWork> StudentWorks { get; set; } = new List<StudentWork>();
    }
}