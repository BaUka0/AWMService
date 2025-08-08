namespace AWMService.Domain.Entities
{
    public class Notifications
    {
        public long Id { get; set; }
        public int UserId { get; set; }
        public Users User { get; set; } = null!;

        public string Message { get; set; } = null!;
        public bool IsRead { get; set; }
        public DateTime? ReadOn { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
