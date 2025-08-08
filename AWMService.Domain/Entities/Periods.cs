using AWMService.Domain.Commons;

namespace AWMService.Domain.Entities
{
    public class Periods : AuditableSoftDeletableEntity
    {
        public int PeriodTypeId { get; set; }
        public PeriodTypes PeriodType { get; set; } = null!;

        public int AcademicYearId { get; set; }
        public AcademicYears AcademicYear { get; set; } = null!;

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int StatusId { get; set; }
        public Statuses Status { get; set; } = null!;
    }
}
