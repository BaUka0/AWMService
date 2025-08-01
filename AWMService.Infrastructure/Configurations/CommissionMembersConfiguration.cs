using AWMService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AWMService.Infrastructure.Configurations
{
    class CommissionMembersConfiguration : IEntityTypeConfiguration<CommissionMembers>
    {
        public void Configure(EntityTypeBuilder<CommissionMembers> builder)
        {
            builder.HasKey(cm => new { cm.CommissionId, cm.MemberId });

            builder.Property(cm => cm.RoleInCommission)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasOne(cm => cm.Commission)
                .WithMany(c => c.CommissionMembers)
                .HasForeignKey(cm => cm.CommissionId);


            builder.HasOne(cm => cm.Member)
                .WithMany(u => u.CommissionMembers)
                .HasForeignKey(cm => cm.MemberId);
        }
    }
}
