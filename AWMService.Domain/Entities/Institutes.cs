using AWMService.Domain.Commons;

namespace AWMService.Domain.Entities
{
    public class Institutes : EntityBase
    {
        public string Name { get; set; } = null!;
        public ICollection<Departments> Departments { get; set; } = new List<Departments>();
    }
}
