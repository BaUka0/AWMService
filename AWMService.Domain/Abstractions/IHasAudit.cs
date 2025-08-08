namespace AWMService.Domain.Abstractions
{
    public interface IHasAudit
    {
        DateTime CreatedOn { get; set; }
        int CreatedBy { get; set; }
        int? ModifiedBy { get; set; }
        DateTime? ModifiedOn { get; set; }
    }
}
