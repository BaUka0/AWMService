using AWMService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AWMService.Infrastructure.Configurations
{
    public class InstitutesConfiguration : IEntityTypeConfiguration<Institutes>
    {
        public void Configure(EntityTypeBuilder<Institutes> e)
        {
            e.ToTable("Institutes");
            e.HasKey(x => x.Id);
            e.Property(x => x.Name)
             .IsRequired()
             .HasMaxLength(255);
            e.HasIndex(x => x.Name);

        }
    }
}
