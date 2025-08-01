
using AWMService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AWMService.Infrastructure.Configurations
{
    public class PeriodTypesConfiguration : IEntityTypeConfiguration<PeriodTypes>
    {
        public void Configure(EntityTypeBuilder<PeriodTypes> builder)
        {
            builder.HasKey(pt => pt.PeriodTypeId);
            builder.Property(pt => pt.Name)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
