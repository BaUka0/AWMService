using AWMService.Domain.Commons;

namespace AWMService.Domain.Entities
{
    public class Topics : AuditableSoftDeletableEntity
    {
        public int DirectionId { get; set; }
        public Directions Direction { get; set; } = null!;

        public string TitleKz { get; set; }
        public string TitleRu { get; set; }
        public string TitleEn { get; set; }
        public string Description { get; set; }

        public int MaxParticipants { get; set; } = 1;

        public int SupervisorId { get; set; }
        public Users Supervisor { get; set; } = null!;

        public int StatusId { get; set; }
        public Statuses Status { get; set; } = null!;
    }
}
