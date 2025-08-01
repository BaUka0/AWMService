namespace AWMService.Domain.Entities
{
    public class Directions
    {
        public int DirectionId { get; set; }
        public string NameKz { get; set; }
        public string NameRu { get; set; }
        public string NameEn { get; set; }
        public string Description { get; set; }
        public int SupervisorId { get; set; }
        public int StatusId { get; set; }
        public DateTime CreatedOn { get; set; }

        public Users Supervisor { get; set; }
        public Statuses Status { get; set; }

        public List<Topics> Topics { get; set; }

    }
}
