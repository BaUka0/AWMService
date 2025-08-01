using AWMService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AWMService.Infrastructure.Configurations
{
    public class CommissionsConfiguration : IEntityTypeConfiguration<Commissions>
    {
        public void Configure(EntityTypeBuilder<Commissions> builder)
        {
          builder.HasKey(c => c.CommissionId);
            builder.Property(c => c.Name)
                     .IsRequired()
                     .HasMaxLength(255);

            builder.HasOne(c => c.Periods)
                   .WithMany(p => p.Commissions)
                   .HasForeignKey(c => c.PeriodId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.CommissionTypes)
                     .WithMany(ct => ct.Commissions)
                     .HasForeignKey(c => c.CommissionTypeId)
                     .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Secretary)
                        .WithMany(u => u.Commissions)
                        .HasForeignKey(c => c.SecretaryId)
                        .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Department)
                        .WithMany(d => d.Commissions)
                        .HasForeignKey(c => c.DepartmentId)
                        .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
