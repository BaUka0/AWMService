namespace AWMService.Domain.Entities
{
    public class DefenseSchedules
    {
        public int DefenseSchedulesId { get; set; }
        public int CommissionId { get; set; }
        public int StudentWorkId { get; set; }
        public DateTime DefenseDate { get; set; }
        public string Location { get; set; }

        public Commissions Commissions { get; set; }
        public StudentWork StudentWorks { get; set; }

        public List<DefenseGrades> DefenseGrades { get; set; }

    }
}
