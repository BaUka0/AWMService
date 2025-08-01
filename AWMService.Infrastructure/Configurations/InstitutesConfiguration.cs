using AWMService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AWMService.Infrastructure.Configurations
{
    public class InstitutesConfiguration : IEntityTypeConfiguration<Institutes>
    {
        public void Configure(EntityTypeBuilder<Institutes> builder)
        {
            builder.HasKey(i => i.InstituteId);
            builder.Property(i => i.Name)
                .HasMaxLength(255)
                .IsRequired();

        }
    }
}
