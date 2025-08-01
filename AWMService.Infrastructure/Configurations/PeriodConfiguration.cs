using AWMService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace AWMService.Infrastructure.Configurations
{
    public class PeriodConfiguration : IEntityTypeConfiguration<Periods>
    {
        public void Configure(EntityTypeBuilder<Periods> builder)
        {
            builder.HasKey(a => a.PeriodId);

            builder.Property(a => a.StartDate).IsRequired();
            builder.Property(a => a.EndDate).IsRequired();

            builder.HasOne(a=>a.PeriodType)
                .WithMany(a=>a.Periods)
                .HasForeignKey(a => a.PeriodTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.AcademicYear)
                .WithMany(a => a.Periods)
                .HasForeignKey(a => a.AcademicYearId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Status)
                .WithMany(a => a.Periods)
                .HasForeignKey(a => a.StatusId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.CreatedByUser)
                .WithMany(a => a.Periods)
                .HasForeignKey(a => a.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
