namespace AWMService.Domain.Entities
{
    public class WorkTypes
    {
        public int WorkTypeId { get; set; }
        public string Name { get; set; }

        public List<StudentWork> StudentWorks { get; set; }
    }
}