using System.ComponentModel.DataAnnotations;
namespace AWMService.Domain.Entities
{
    public class Statuses
    {
        public int StatusId { get; set; }
        [Required]
        public string Name { get; set; }
        public string EntityType { get; set; } 

        public List<Periods> Periods { get; set; } 
        public List<Directions> Directions { get; set; }
        public List<StudentWork> StudentWorks { get; set; }
        public List<Topics> Topics { get; set; }
        public List<Applications> Applications { get; set; }
        public List<DefenseGrades> DefenseGrades { get; set; }
        public List<WorkChecks> WorkChecks { get; set; }
    }
}
