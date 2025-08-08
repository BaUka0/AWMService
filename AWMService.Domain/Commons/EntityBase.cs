using AWMService.Domain.Abstractions;

namespace AWMService.Domain.Commons
{
    public abstract class EntityBase : IEntity<int>
    {
        public int Id { get; set; }

    }
}
