namespace AWMService.Domain.Entities
{
    public class DepartmentExperts
    {
        public int DepartmentExpertId { get; set; }
        public int UserId { get; set; }
        public int DepartmentId { get; set; }
        public int CheckTypeId { get; set; }
        public int AcademicYearId { get; set; }
        public int AssignedBy { get; set; }
        public DateTime AssignedOn { get; set; }

        public Users User { get; set; }
        public Department Department { get; set; }
        public CheckTypes CheckType { get; set; }
        public AcademicYears AcademicYear { get; set; }
        public Users AssignedByUser { get; set; }

    }
}
