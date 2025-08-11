using AWMService.Application.Abstractions;           
using AWMService.Domain.Entities;
using AWMService.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;


namespace AWMService.Infrastructure.Data
{
    public class AppDbContext : DbContext
    { 
         public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
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
        public DbSet<Departments> Departments { get; set; }
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
        public DbSet<UserTypes> UserTypes { get; set; }
        public DbSet<WorkChecks> WorkChecks { get; set; }
        public DbSet<WorkTypes> WorkTypes { get; set; }
        public DbSet<ExternalContacts> ExternalContacts { get; set; }
        public DbSet<Notifications> Notifications { get; set; }
        public DbSet<Settings> Settings { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.ApplyConfiguration(new AcademicYearsConfiguration());
            modelBuilder.ApplyConfiguration(new ApplicationsConfiguration());
            modelBuilder.ApplyConfiguration(new AttachmentsConfiguration());
            modelBuilder.ApplyConfiguration(new CheckTypesConfiguration());
            modelBuilder.ApplyConfiguration(new CommissionMembersConfiguration());
            modelBuilder.ApplyConfiguration(new CommissionsConfiguration());
            modelBuilder.ApplyConfiguration(new CommissionTypesConfiguration());
            modelBuilder.ApplyConfiguration(new DefenseGradesConfiguration());
            modelBuilder.ApplyConfiguration(new DefenseSchedulesConfiguration());
            modelBuilder.ApplyConfiguration(new DepartmentExpertsConfiguration());
            modelBuilder.ApplyConfiguration(new DepartmentsConfiguration());
            modelBuilder.ApplyConfiguration(new DirectionsConfiguration());
            modelBuilder.ApplyConfiguration(new EvaluationCriteriaConfiguration());
            modelBuilder.ApplyConfiguration(new EvaluationScoresConfiguration());
            modelBuilder.ApplyConfiguration(new InstitutesConfiguration());
            modelBuilder.ApplyConfiguration(new PeriodsConfiguration());
            modelBuilder.ApplyConfiguration(new PeriodTypesConfiguration());
            modelBuilder.ApplyConfiguration(new PermissionsConfiguration());
            modelBuilder.ApplyConfiguration(new RolePermissionsConfiguration());
            modelBuilder.ApplyConfiguration(new RolesConfiguration());
            modelBuilder.ApplyConfiguration(new StatusesConfiguration());
            modelBuilder.ApplyConfiguration(new StudentWorkConfiguration());
            modelBuilder.ApplyConfiguration(new SupervisorApprovalsConfiguration());
            modelBuilder.ApplyConfiguration(new TopicsConfiguration());
            modelBuilder.ApplyConfiguration(new UserRolesConfiguration());
            modelBuilder.ApplyConfiguration(new UsersConfiguration());
            modelBuilder.ApplyConfiguration(new UserTypeConfiguration());
            modelBuilder.ApplyConfiguration(new WorkChecksConfiguration());
            modelBuilder.ApplyConfiguration(new WorkTypesConfiguration());
            modelBuilder.ApplyConfiguration(new ExternalContactsConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationsConfiguration());
            modelBuilder.ApplyConfiguration(new SettingsConfiguration());


            base.OnModelCreating(modelBuilder);

        }
    }
 
}
