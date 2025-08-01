namespace AWMService.Domain.Entities
{
    public class Topics
    {
        public int TopicId { get; set; }
        public int DirectionId { get; set; }
        public string TitleKz { get; set; }
        public string TitleRu { get; set; }
        public string TitleEn { get; set; }
        public string Description { get; set; }
        public int MaxParticipants { get; set; }
        public int SuperVisorId { get; set; }
        public int StatusId { get; set; }
        public DateTime CreatedOn { get; set; }


        public Directions Direction { get; set; }
        public Users SuperVisor { get; set; }
        public Statuses Status { get; set; }

        public List<StudentWork> StudentWorks { get; set; }
        public List<Applications> Applications { get; set; }




    }
}
