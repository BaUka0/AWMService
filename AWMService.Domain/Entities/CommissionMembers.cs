namespace AWMService.Domain.Entities
{
    public class CommissionMembers
    {
        public int CommissionId { get; set; }
        public int MemberId { get; set; }
        public string RoleInCommission { get; set; }

        public Commissions Commission { get; set; }
        public Users Member { get; set; }
    }
}
