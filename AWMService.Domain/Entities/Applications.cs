using AWMService.Domain.Commons;

namespace AWMService.Domain.Entities
{
    public class Applications : AuditableEntity
    {
        public int StudentId { get; set; }
        public Users Student { get; set; } = null!;

        public int TopicId { get; set; }
        public Topics Topic { get; set; } = null!;

        public int StatusId { get; set; }
        public Statuses Status { get; set; } = null!;
    }
}
