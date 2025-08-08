using AWMService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AWMService.Infrastructure.Configurations
{
    public class CommissionTypesConfiguration : IEntityTypeConfiguration<CommissionTypes>
    {
        public void Configure(EntityTypeBuilder<CommissionTypes> e)
        {
            e.ToTable("CommissionTypes");
            e.HasKey(x => x.Id);

            e.Property(x => x.Name)
                .HasMaxLength(100);
            e.Property(x => x.IsDeleted)
                .HasDefaultValue(false);

            e.HasIndex(x => x.Name);

            e.HasOne<Users>()
                .WithMany()
                .HasForeignKey(x => x.DeletedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
