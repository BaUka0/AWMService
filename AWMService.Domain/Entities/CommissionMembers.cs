using AWMService.Domain.Commons;

namespace AWMService.Domain.Entities
{
    public class CommissionMembers : EntityBase
    {
        public int CommissionId { get; set; }
        public Commissions Commission { get; set; } = null!;

        public int? MemberId { get; set; }
        public Users? Member { get; set; }

        public int? ExternalContactId { get; set; }
        public ExternalContacts? ExternalContact { get; set; }

        public string? RoleInCommission { get; set; }

        public DateTime AssignedOn { get; set; }
        public int AssignedBy { get; set; }
        public DateTime? RemovedOn { get; set; }
        public int? RemovedBy { get; set; }
    }
}
