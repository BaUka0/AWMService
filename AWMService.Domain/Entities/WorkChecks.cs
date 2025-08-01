namespace AWMService.Domain.Entities
{
    public class WorkChecks
    {
        public int WorkChecksId { get; set; }
        public int StudentWorkId { get; set; }
        public int CheckTypeId { get; set; }
        public int ExpertId { get; set; }
        public int StatusId { get; set; }
        public string Comment { get; set; }
        public string ResultData { get; set; }
        public DateTime SubmittedOn { get; set; }
        public DateTime CheckedOn { get; set; }

        public StudentWork StudentWork { get; set; }
        public CheckTypes CheckType { get; set; }
        public Users Expert { get; set; }
        public Statuses Status { get; set; }
    }
}
