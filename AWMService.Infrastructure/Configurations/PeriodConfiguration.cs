using AWMService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace AWMService.Infrastructure.Configurations
{
    public class PeriodsConfiguration : IEntityTypeConfiguration<Periods>
    {
        public void Configure(EntityTypeBuilder<Periods> e)
        {
            e.ToTable("Periods");
            e.HasKey(x => x.Id);

            e.Property(x => x.StartDate)
                .IsRequired();
            e.Property(x => x.EndDate)
                .IsRequired();
            e.Property(x => x.IsDeleted)
                .HasDefaultValue(false);

            e.HasIndex(x => new { x.AcademicYearId, x.PeriodTypeId })
                .IsUnique(false);
            e.HasIndex(x => x.StatusId);
            e.HasIndex(x => x.StartDate);
            e.HasIndex(x => x.EndDate);

            e.HasOne(x => x.PeriodType)
                .WithMany(pt => pt.Periods)
                .HasForeignKey(x => x.PeriodTypeId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.AcademicYear)
                .WithMany(ay => ay.Periods)
                .HasForeignKey(x => x.AcademicYearId)
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

            e.ToTable(tb => tb.HasCheckConstraint("CK_Periods_Dates", "[StartDate] <= [EndDate]"));
        }
    }
}
