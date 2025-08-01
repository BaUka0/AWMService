using AWMService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AWMService.Infrastructure.Configurations
{
    class StudentWorkConfiguration : IEntityTypeConfiguration<StudentWork>
    {
        public void Configure(EntityTypeBuilder<StudentWork> builder)
        {
            builder.HasKey(c => c.StudentWorkId);
            builder.Property(c=>c.FinalGrade)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasOne(c=> c.Student)
                .WithMany(c => c.StudentWorks)
                .HasForeignKey(c => c.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Topic)
                .WithMany(c => c.StudentWorks)
                .HasForeignKey(c => c.TopicId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.AcademicYear)
                .WithMany(c => c.StudentWorks)
                .HasForeignKey(c => c.AcademicYearId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.WorkType)
                .WithMany(c => c.StudentWorks)
                .HasForeignKey(c => c.WorkTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Status)
                .WithMany(c => c.StudentWorks)
                .HasForeignKey(c => c.StatusId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
