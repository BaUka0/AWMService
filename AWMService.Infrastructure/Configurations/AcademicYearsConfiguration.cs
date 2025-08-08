using AWMService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace AWMService.Infrastructure.Configurations
{
    public class AcademicYearsConfiguration : IEntityTypeConfiguration<AcademicYears>
    {
        public void Configure(EntityTypeBuilder<AcademicYears> e)
        {
            e.ToTable("AcademicYears");
            e.HasKey(x => x.Id);

            e.Property(x => x.YearName)
                .HasMaxLength(50);
            e.Property(x => x.IsDeleted)
                .HasDefaultValue(false);

            e.HasIndex(x => x.YearName)
                .IsUnique(false);

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
