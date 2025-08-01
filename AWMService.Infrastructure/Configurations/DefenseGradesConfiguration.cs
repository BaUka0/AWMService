using AWMService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AWMService.Infrastructure.Configurations
{
    public class DefenseGradesConfiguration : IEntityTypeConfiguration<DefenseGrades>
    {
        public void Configure(EntityTypeBuilder<DefenseGrades> builder)
        {
            builder.HasKey(b=> b.DefenseGradeId);
            
            builder.Property(b => b.FinalGrade)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasOne(c=>c.DefenseScheduled)
                .WithMany(c => c.DefenseGrades)
                .HasForeignKey(c => c.DefenseScheduledId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.Status)
                .WithMany(c => c.DefenseGrades)
                .HasForeignKey(c => c.StatusId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
