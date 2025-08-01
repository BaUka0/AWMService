using AWMService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace AWMService.Infrastructure.Configurations
{
    public class SupervisorApprovalsConfiguration : IEntityTypeConfiguration<SupervisorApprovals>
    {
        public void Configure(EntityTypeBuilder<SupervisorApprovals> builder)
        {
            builder.HasKey(sa => sa.Id);
            builder.HasOne(sa => sa.User)
                   .WithMany(sa=>sa.SupervisorApprovals)
                   .HasForeignKey(sa => sa.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(sa => sa.Department)
                     .WithMany(sa => sa.SupervisorApprovals)
                     .HasForeignKey(sa => sa.DepartmentId)
                     .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(sa => sa.AcademicYear)
                        .WithMany(sa => sa.SupervisorApprovals)
                        .HasForeignKey(sa => sa.AcademicYearId)
                        .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(sa => sa.ApprovedByUser)
                        .WithMany(sa => sa.ApprovedByUsers)
                        .HasForeignKey(sa => sa.ApprovedBy)
                        .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
