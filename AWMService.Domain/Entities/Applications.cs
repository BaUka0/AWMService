namespace AWMService.Domain.Entities
{
    public class Applications
    {
        public int ApplicationId { get; set; }
        
        public int StudentId { get; set; }
        public int TopicId { get; set; }
        public string MotivationLetter { get; set; }
        public int StatusId { get; set; }
        public DateTime CreatedOn { get; set; }
        
        public Users Student { get; set; }
        public Topics Topic { get; set; }
        public Statuses Status { get; set; }
    }
}
