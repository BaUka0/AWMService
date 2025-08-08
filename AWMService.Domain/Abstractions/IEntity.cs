namespace AWMService.Domain.Abstractions
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }
}
