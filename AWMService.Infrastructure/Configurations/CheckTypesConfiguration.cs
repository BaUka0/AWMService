using AWMService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace AWMService.Infrastructure.Configurations
{
    public class CheckTypesConfiguration : IEntityTypeConfiguration<CheckTypes>
    {
        public void Configure(EntityTypeBuilder<CheckTypes> builder)
        {
            builder.HasKey(ct => ct.CheckTypeId);
            builder.Property(ct => ct.Name)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
