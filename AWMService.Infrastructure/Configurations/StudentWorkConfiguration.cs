using AWMService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AWMService.Infrastructure.Configurations
{
    public class StudentWorkConfiguration : IEntityTypeConfiguration<StudentWork>
    {
        public void Configure(EntityTypeBuilder<StudentWork> e)
        {
            e.ToTable("StudentWork");
            e.HasKey(x => x.Id);

            e.Property(x => x.FinalGrade)
                .HasMaxLength(50);
            e.Property(x => x.IsDeleted)
                .HasDefaultValue(false);

            e.HasIndex(x => x.StudentId);
            e.HasIndex(x => x.TopicId)
                .IsUnique(false);
            e.HasIndex(x => x.AcademicYearId);
            e.HasIndex(x => new { x.StudentId, x.AcademicYearId, x.WorkTypeId })
                .IsUnique(false);
            e.HasIndex(x => x.StatusId);

            e.HasOne(x => x.Student)
                .WithMany()
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Topic)
                .WithMany()
                .HasForeignKey(x => x.TopicId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.AcademicYear)
                .WithMany()
                .HasForeignKey(x => x.AcademicYearId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.WorkType)
                .WithMany()
                .HasForeignKey(x => x.WorkTypeId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Status)
                .WithMany()
                .HasForeignKey(x => x.StatusId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne<Users>()
                .WithMany()
                .HasForeignKey(x => x.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Users>()
                .WithMany()
                .HasForeignKey(x => x.ModifiedBy)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Users>()
                .WithMany()
                .HasForeignKey(x => x.DeletedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
