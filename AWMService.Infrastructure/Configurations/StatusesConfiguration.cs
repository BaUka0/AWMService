using AWMService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AWMService.Infrastructure.Configurations
{
    public class StatusesConfiguration : IEntityTypeConfiguration<Statuses>
    {
        public void Configure(EntityTypeBuilder<Statuses> builder)
        {
            builder.HasKey(s => s.StatusId);
            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.EntityType)
                .HasMaxLength(50);

            builder.HasIndex(s => s.Name)
                .IsUnique();
        }
    }
}
