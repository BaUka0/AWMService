using AWMService.Domain.Commons;

namespace AWMService.Domain.Entities
{
    public class AcademicYears : AuditableSoftDeletableEntity
    {
        public string? YearName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ICollection<Periods> Periods { get; set; } = new List<Periods>();
    }
}
