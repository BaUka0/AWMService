using AWMService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AWMService.Infrastructure.Configurations
{
    public class CommissionMembersConfiguration : IEntityTypeConfiguration<CommissionMembers>
    {
        public void Configure(EntityTypeBuilder<CommissionMembers> e)
        {
            e.ToTable("CommissionMembers");
            e.HasKey(x => x.Id);

            e.Property(x => x.RoleInCommission)
                .HasMaxLength(100);
            e.Property(x => x.AssignedOn)
                .IsRequired();

            e.HasIndex(x => new { x.CommissionId, x.MemberId })
                .IsUnique()
                .HasFilter("[MemberId] IS NOT NULL");

            e.HasIndex(x => new { x.CommissionId, x.ExternalContactId })
                .IsUnique()
                .HasFilter("[ExternalContactId] IS NOT NULL");

            e.HasIndex(x => x.AssignedBy);

            e.HasOne(x => x.Commission)
                .WithMany(c => c.Members)
                .HasForeignKey(x => x.CommissionId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Member)
                .WithMany()
                .HasForeignKey(x => x.MemberId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.ExternalContact)
                .WithMany()
                .HasForeignKey(x => x.ExternalContactId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne<Users>()
                .WithMany()
                .HasForeignKey(x => x.AssignedBy)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Users>()
                .WithMany()
                .HasForeignKey(x => x.RemovedBy)
                .OnDelete(DeleteBehavior.Restrict);


            e.ToTable(tb => tb.HasCheckConstraint(
                "CK_CommissionMembers_EitherMemberOrExternal",
                "([MemberId] IS NOT NULL) OR ([ExternalContactId] IS NOT NULL)"
            ));
        }
    }
}
