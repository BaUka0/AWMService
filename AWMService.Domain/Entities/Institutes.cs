namespace AWMService.Domain.Entities
{
    public class Institutes
    {
        public int InstituteId { get; set; }
        public string Name { get; set; }

        public List<Department> Departments { get; set; } 

    }
}
