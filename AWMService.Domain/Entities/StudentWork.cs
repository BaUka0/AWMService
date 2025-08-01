namespace AWMService.Domain.Entities
{
    public class StudentWork
    {
        public int StudentWorkId { get; set; }
        public int StudentId { get; set; }
        public int TopicId { get; set; }
        public int AcademicYearId { get; set; }
        public int WorkTypeId { get; set; }
        public int StatusId { get; set; }
        public string FinalGrade { get; set; }
        public DateTime CreatedOn { get; set; }

        public Users Student { get; set; }
        public Topics Topic { get; set; }
        public AcademicYears AcademicYear { get; set; }
        public WorkTypes WorkType { get; set; }
        public Statuses Status { get; set; }
        public List<DefenseSchedules> DefenseSchedules { get; set; }
        public List<WorkChecks> WorkChecks { get; set; }

    }
}
