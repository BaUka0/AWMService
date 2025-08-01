namespace AWMService.Domain.Entities
{
    public class CheckTypes
    {
        public int CheckTypeId { get; set; }
        public string Name { get; set; }
        public List<DepartmentExperts> DepartmentExperts { get; set; } 
        public List<WorkChecks> WorkChecks { get; set; }
    }
}
