using AWMService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace AWMService.Infrastructure.Configurations
{
    public class DepartmentExpertsConfiguration : IEntityTypeConfiguration<DepartmentExperts>
    {
        public void Configure(EntityTypeBuilder<DepartmentExperts> builder)
        {
            builder.HasKey(de => de.DepartmentExpertId);

            builder.HasOne(de => de.User)
                .WithMany(u => u.DepartmentExperts)
                .HasForeignKey(de => de.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(de => de.Department)
                .WithMany(d => d.DepartmentExperts)
                .HasForeignKey(de => de.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(de => de.CheckType)
                .WithMany(ct => ct.DepartmentExperts)
                .HasForeignKey(de => de.CheckTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(de => de.AcademicYear)
                .WithMany(ay => ay.DepartmentExperts)
                .HasForeignKey(de => de.AcademicYearId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c=>c.AssignedByUser)
                .WithMany(ce=>ce.AssignedDepartmentExperts)
                .HasForeignKey(de => de.AssignedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
