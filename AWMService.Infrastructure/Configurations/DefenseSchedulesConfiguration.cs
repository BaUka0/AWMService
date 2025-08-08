using AWMService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AWMService.Infrastructure.Configurations
{
    public class DefenseSchedulesConfiguration : IEntityTypeConfiguration<DefenseSchedules>
    {
        public void Configure(EntityTypeBuilder<DefenseSchedules> e)
        {
            e.ToTable("DefenseSchedules");
            e.HasKey(x => x.Id);

            e.Property(x => x.Location)
                .HasMaxLength(255);
            e.Property(x => x.IsDeleted)
                .HasDefaultValue(false);

            e.HasIndex(x => x.CommissionId);
            e.HasIndex(x => x.StudentWorkId);
            e.HasIndex(x => x.DefenseDate);

            e.HasOne(x => x.Commission)
                .WithMany()
                .HasForeignKey(x => x.CommissionId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.StudentWork)
                .WithMany(sw => sw.DefenseSchedules)
                .HasForeignKey(x => x.StudentWorkId)
                .OnDelete(DeleteBehavior.Cascade);

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
