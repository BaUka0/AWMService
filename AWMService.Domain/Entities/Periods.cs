namespace AWMService.Domain.Entities
{
    public class Periods
    {
        public int PeriodId { get; set; }
        public int PeriodTypeId { get; set; }
        public int AcademicYearId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int StatusId { get; set; }

        public DateTime CreatedOn { get; set; } 
        public int CreatedBy { get; set; }

        public PeriodTypes PeriodType { get; set; }
        public AcademicYears AcademicYear { get; set; }
        public Statuses Status { get; set; }
        public Users CreatedByUser { get; set; }

        public List<Commissions> Commissions { get; set; }
    }
}
