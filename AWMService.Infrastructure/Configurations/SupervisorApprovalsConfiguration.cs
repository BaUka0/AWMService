using AWMService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace AWMService.Infrastructure.Configurations
{
    public class SupervisorApprovalsConfiguration : IEntityTypeConfiguration<SupervisorApprovals>
    {
        public void Configure(EntityTypeBuilder<SupervisorApprovals> e)
        {
            e.ToTable("SupervisorApprovals");
            e.HasKey(x => x.Id);

            e.Property(x => x.ApprovedOn)
                .IsRequired();

            e.HasIndex(x => new { x.UserId, x.DepartmentId, x.AcademicYearId })
                .IsUnique();
            e.HasIndex(x => x.ApprovedBy);

            e.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Department)
                .WithMany()
                .HasForeignKey(x => x.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.AcademicYear)
                .WithMany()
                .HasForeignKey(x => x.AcademicYearId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.ApprovedByUser)
                .WithMany()
                .HasForeignKey(x => x.ApprovedBy)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne<Users>()
                .WithMany()
                .HasForeignKey(x => x.ModifiedBy)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Users>()
                .WithMany()
                .HasForeignKey(x => x.RevokedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
