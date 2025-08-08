using AWMService.Domain.Commons;

namespace AWMService.Domain.Entities
{
    public class SupervisorApprovals : EntityBase
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public Users User { get; set; } = null!;

        public int DepartmentId { get; set; }
        public Departments Department { get; set; } = null!;

        public int AcademicYearId { get; set; }
        public AcademicYears AcademicYear { get; set; } = null!;

        public int ApprovedBy { get; set; }
        public Users ApprovedByUser { get; set; } = null!;
        public DateTime ApprovedOn { get; set; }

        
        public int? ModifiedBy { get; set; }
        public Users? ModifiedByUser { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? RevokedBy { get; set; }
        public Users? RevokedByUser { get; set; }
        public DateTime? RevokedOn { get; set; }
    }
}
