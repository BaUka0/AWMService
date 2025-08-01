namespace AWMService.Domain.Entities
{
    public class UserType
    {
        public int UserTypeId { get; set; }
        public string Name { get; set; }
        public List<Users> Users { get; set; } 
    }
}
