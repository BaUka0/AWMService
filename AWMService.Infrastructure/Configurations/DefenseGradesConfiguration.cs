using AWMService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AWMService.Infrastructure.Configurations
{
    public class DefenseGradesConfiguration : IEntityTypeConfiguration<DefenseGrades>
    {
        public void Configure(EntityTypeBuilder<DefenseGrades> e)
        {
            e.ToTable("DefenseGrades");
            e.HasKey(x => x.Id);

            e.Property(x => x.FinalGrade)
                .HasMaxLength(50);

            e.HasIndex(x => x.DefenseScheduleId);
            e.HasIndex(x => x.StatusId);

            e.HasOne(x => x.DefenseSchedule)
                .WithMany(s => s.DefenseGrades)
                .HasForeignKey(x => x.DefenseScheduleId)
                .OnDelete(DeleteBehavior.Cascade);
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
        }
    }
}
