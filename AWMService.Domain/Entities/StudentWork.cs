using AWMService.Domain.Commons;

namespace AWMService.Domain.Entities
{
    public class StudentWork : AuditableSoftDeletableEntity
    {
        public int StudentId { get; set; }
        public Users Student { get; set; } = null!;

        public int TopicId { get; set; }
        public Topics Topic { get; set; } = null!;

        public int AcademicYearId { get; set; }
        public AcademicYears AcademicYear { get; set; } = null!;

        public int WorkTypeId { get; set; }
        public WorkTypes WorkType { get; set; } = null!;

        public int StatusId { get; set; }
        public Statuses Status { get; set; } = null!;

        public string? FinalGrade { get; set; }

        public ICollection<Attachments> Attachments { get; set; } = new List<Attachments>();
        public ICollection<WorkChecks> WorkChecks { get; set; } = new List<WorkChecks>();
        public ICollection<DefenseSchedules> DefenseSchedules { get; set; } = new List<DefenseSchedules>();
    }
}
