using AWMService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace AWMService.Infrastructure.Configurations
{
    public class UsersConfiguration : IEntityTypeConfiguration<Users>
    {
        public void Configure(EntityTypeBuilder<Users> builder)
        {
            builder.HasKey(u => u.UserId);
            builder.Property(u => u.FirstName).HasMaxLength(100);
            builder.Property(u => u.LastName).HasMaxLength(100);
            builder.Property(u =>u.SurName)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(u => u.Email)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(u => u.Login)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(u=>u.PhoneNumber)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(u => u.IIN)
                .HasMaxLength(12)
                .IsRequired(false);

            builder.HasIndex(u => u.Email).IsUnique();
            builder.HasIndex(u => u.Login).IsUnique();
            builder.HasIndex(u=> u.IIN).IsUnique();

            builder.HasOne(u => u.Department)
                .WithMany(d => d.Users)
                .HasForeignKey(u => u.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(u=>u.UserType)
                .WithMany(ut => ut.Users)
                .HasForeignKey(u => u.UserTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            //builder.HasMany(u => u.UserRoles)
            //    .WithOne(ur => ur.Users);
            //builder.HasMany(u => u.Periods)
            //    .WithOne(p => p.CreatedByUser)
            //    .HasForeignKey(p => p.CreatedBy)
            //    .OnDelete(DeleteBehavior.Restrict);
            
            //builder.HasMany(u => u.Commissions)
            //    .WithOne(s => s.Secretary)
            //    .HasForeignKey(s => s.SecretaryId)
            //    .OnDelete(DeleteBehavior.Restrict);

            //builder.HasMany(u => u.Directions)
            //    .WithOne(s => s.Supervisor)
            //    .HasForeignKey(s => s.SupervisorId)
            //    .OnDelete(DeleteBehavior.Restrict);


            //builder.HasMany(u => u.StudentWorks)
            //    .WithOne(sw => sw.Student)
            //    .HasForeignKey(sw => sw.StudentId)
            //    .OnDelete(DeleteBehavior.Restrict);

            //builder.HasMany(cm => cm.CommissionMembers)
            //    .WithOne(m=> m.Member)
            //    .HasForeignKey(m => m.MemberId)
            //    .OnDelete(DeleteBehavior.Restrict);

            //builder.HasMany(t => t.Topics)
            //    .WithOne(s=> s.SuperVisor)
            //    .HasForeignKey(s => s.SuperVisorId)
            //    .OnDelete(DeleteBehavior.Restrict);

            ////1 студент дофига апликейшн жасай ала ма? 
            //builder.HasMany(a => a.Applications)
            //    .WithOne(s => s.Student)
            //    .HasForeignKey(s => s.StudentId)
            //    .OnDelete(DeleteBehavior.Restrict);

            //builder.HasMany(de => de.DepartmentExperts)
            //    .WithOne(u => u.User)
            //    .HasForeignKey(de => de.UserId)
            //    .OnDelete(DeleteBehavior.Restrict);


            //builder.HasMany(de => de.AssignedDepartmentExperts)
            //    .WithOne(a=>a.AssignedByUser)
            //    .HasForeignKey(a => a.AssignedBy)
            //    .OnDelete(DeleteBehavior.Restrict);

            //builder.HasMany(e => e.EvaluationScores)
            //    .WithOne(es => es.CommissionMember)
            //    .HasForeignKey(es => es.CommissionMemberId)
            //    .OnDelete(DeleteBehavior.Restrict);

            //builder.HasMany(wc => wc.WorkChecks)
            //    .WithOne(w => w.Expert)
            //    .HasForeignKey(w => w.ExpertId)
            //    .OnDelete(DeleteBehavior.Restrict);


            //builder.HasMany(a => a.Attachments)
            //    .WithOne(at => at.UploadedBy)
            //    .HasForeignKey(at => at.UploadedById)
            //    .OnDelete(DeleteBehavior.Restrict);

            //builder.HasMany(s => s.SupervisorApprovals)
            //    .WithOne(sa => sa.User)
            //    .HasForeignKey(sa => sa.UserId)
            //    .OnDelete(DeleteBehavior.Restrict);


            //builder.HasMany(s => s.ApprovedByUsers)
            //    .WithOne(sa => sa.ApprovedByUser)
            //    .HasForeignKey(sa => sa.ApprovedBy)
            //    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
