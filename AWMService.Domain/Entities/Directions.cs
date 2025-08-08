using AWMService.Domain.Commons;

namespace AWMService.Domain.Entities
{
    public class Directions : AuditableSoftDeletableEntity
    {
        public string NameKz { get; set; }
        public string NameRu { get; set; }
        public string NameEn { get; set; }
        public string Description { get; set; }

        public int SupervisorId { get; set; }
        public Users Supervisor { get; set; } = null!;

        public int StatusId { get; set; }
        public Statuses Status { get; set; } = null!;

        public ICollection<Topics> Topics { get; set; } = new List<Topics>();
    }
}
