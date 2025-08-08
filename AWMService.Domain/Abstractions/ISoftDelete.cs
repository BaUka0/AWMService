namespace AWMService.Domain.Abstractions
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
        DateTime? DeletedOn { get; set; }
        int? DeletedBy { get; set; }
    }
}
