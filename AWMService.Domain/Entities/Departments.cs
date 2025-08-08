using AWMService.Domain.Commons;

namespace AWMService.Domain.Entities
{
    public class Departments : EntityBase
    {
        public string Name { get; set; } = null!;
        public int InstituteId { get; set; }
        public Institutes Institute { get; set; } = null!;

        public ICollection<Users> Users { get; set; } = new List<Users>();
    }
}
