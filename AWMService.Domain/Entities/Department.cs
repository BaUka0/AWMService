namespace AWMService.Domain.Entities
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public int InstituteId { get; set; }
        public Institutes Institute { get; set; }
        public List<Users> Users { get; set; } 
        public List<Commissions> Commissions { get; set; }
        public List<DepartmentExperts> DepartmentExperts { get; set; }
        public List<SupervisorApprovals> SupervisorApprovals { get; set; }
    }
}
