using AWMService.Domain.Commons;

namespace AWMService.Domain.Entities
{
    public class WorkChecks : AuditableEntity
    {
        public int Id { get; set; }

        public int StudentWorkId { get; set; }
        public StudentWork StudentWork { get; set; } = null!;

        public int CheckTypeId { get; set; }
        public CheckTypes CheckType { get; set; } = null!;

        public int? ExpertId { get; set; }
        public Users? Expert { get; set; }

        public int? ReviewerId { get; set; }
        public ExternalContacts? Reviewer { get; set; } = null!;

        public int StatusId { get; set; }
        public Statuses Status { get; set; } = null!;

        public string? Comment { get; set; }
        public string? ResultData { get; set; }

        public DateTime? SubmittedOn { get; set; }
        public DateTime? CheckedOn { get; set; }

        public ICollection<Attachments> Attachments { get; set; } = new List<Attachments>();
    }
}
