using AWMService.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace AWMService.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
      
        public DbSet<AcademicYears> AcademicYears { get; set; }
        public DbSet<Applications> Applications { get; set; }
        public DbSet<Attachments> Attachments { get; set; }
        public DbSet<CheckTypes> CheckTypes { get; set; }
        public DbSet<CommissionMembers> CommissionMembers { get; set; }
        public DbSet<Commissions> Commissions { get; set; }
        public DbSet<CommissionTypes> CommissionTypes { get; set; }
        public DbSet<DefenseGrades> DefenseGrades { get; set; }
        public DbSet<DefenseSchedules> DefenseSchedules { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<DepartmentExperts> DepartmentExperts { get; set; }
        public DbSet<Directions> Directions { get; set; }
        public DbSet<EvaluationCriteria> EvaluationCriteria { get; set; }
        public DbSet<EvaluationScores> EvaluationScores { get; set; }
        public DbSet<Institutes> Institutes { get; set; }
        public DbSet<Periods> Periods { get; set; }
        public DbSet<PeriodTypes> PeriodTypes { get; set; }
        public DbSet<Permissions> Permissions { get; set; }
        public DbSet<RolePermissions> RolePermissions { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Statuses> Statuses { get; set; }
        public DbSet<StudentWork> StudentWorks { get; set; }
        public DbSet<SupervisorApprovals> SupervisorApprovals { get; set; }
        public DbSet<Topics> Topics { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<UserType> userTypes { get; set; }
        public DbSet<WorkChecks> WorkChecks { get; set; }
        public DbSet<WorkTypes> WorkTypes { get; set; } 
        

    }
}
