using AWMService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AWMService.Infrastructure.Configurations
{
    public class CommissionTypesConfiguration : IEntityTypeConfiguration<CommissionTypes>
    {
        public void Configure(EntityTypeBuilder<CommissionTypes> builder)
        {
            builder.HasKey(builder => builder.CommissionTypesId);
            builder.Property(builder => builder.Name)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
