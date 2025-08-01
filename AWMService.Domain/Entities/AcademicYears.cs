namespace AWMService.Domain.Entities
{
    public class AcademicYears
    {
        public int AcademicYearId { get; set; }
        public string YearName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public List<Periods> Periods { get; set; }
        public List<StudentWork> StudentWorks { get; set; }
        public List<DepartmentExperts> DepartmentExperts { get; set; }
        public List<SupervisorApprovals> SupervisorApprovals { get; set; }
    }
}
