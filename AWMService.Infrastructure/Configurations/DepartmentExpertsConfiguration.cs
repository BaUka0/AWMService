using AWMService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace AWMService.Infrastructure.Configurations
{
    public class DepartmentExpertsConfiguration : IEntityTypeConfiguration<DepartmentExperts>
    {
        public void Configure(EntityTypeBuilder<DepartmentExperts> e)
        {
            e.ToTable("DepartmentExperts");
            e.HasKey(x => x.Id);

            e.Property(x => x.AssignedOn)
                .IsRequired();

            e.HasIndex(x => new { x.DepartmentId, x.CheckTypeId, x.AcademicYearId, x.UserId })
                .IsUnique();
            e.HasIndex(x => x.AssignedBy);

            e.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Department)
                .WithMany()
                .HasForeignKey(x => x.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.CheckType)
                .WithMany()
                .HasForeignKey(x => x.CheckTypeId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.AcademicYear)
                .WithMany()
                .HasForeignKey(x => x.AcademicYearId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(x => x.AssignedByUser)
                .WithMany()
                .HasForeignKey(x => x.AssignedBy)
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
