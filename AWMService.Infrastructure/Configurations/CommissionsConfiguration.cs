using AWMService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AWMService.Infrastructure.Configurations
{
    public class CommissionsConfiguration : IEntityTypeConfiguration<Commissions>
    {
        public void Configure(EntityTypeBuilder<Commissions> e)
        {
            e.ToTable("Commissions");
            e.HasKey(x => x.Id);

            e.Property(x => x.Name)
                .HasMaxLength(255);
            e.Property(x => x.IsDeleted)
                .HasDefaultValue(false);

            e.HasIndex(x => x.CommissionTypeId);
            e.HasIndex(x => x.SecretaryId);
            e.HasIndex(x => x.PeriodId);
            e.HasIndex(x => new { x.DepartmentId, x.PeriodId, x.CommissionTypeId })
                .IsUnique(false);

            e.HasOne(x => x.CommissionType)
                .WithMany(ct => ct.Commissions)
                .HasForeignKey(x => x.CommissionTypeId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Secretary)
                .WithMany()
                .HasForeignKey(x => x.SecretaryId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Period)
                .WithMany()
                .HasForeignKey(x => x.PeriodId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Department)
                .WithMany()
                .HasForeignKey(x => x.DepartmentId)
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
