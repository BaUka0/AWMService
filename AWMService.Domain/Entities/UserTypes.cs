using AWMService.Domain.Commons;

namespace AWMService.Domain.Entities
{
    public class UserTypes : EntityBase
    {
        public string Name { get; set; } = null!;
        public ICollection<Users> Users { get; set; } = new List<Users>();
    }
}
