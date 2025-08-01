namespace AWMService.Domain.Entities
{
    public class SupervisorApprovals
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int DepartmentId { get; set; }
        public int AcademicYearId { get; set; }
        public int ApprovedBy { get; set; }
        public DateTime ApprovedOn { get; set; }

        public Users User { get; set; }
        public Department Department { get; set; }
        public AcademicYears AcademicYear { get; set; }
        public Users ApprovedByUser { get; set; }
    }
}
