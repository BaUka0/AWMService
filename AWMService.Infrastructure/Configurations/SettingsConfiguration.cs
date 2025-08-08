using AWMService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AWMService.Infrastructure.Configurations
{
    public class SettingsConfiguration : IEntityTypeConfiguration<Settings>
    {
        public void Configure(EntityTypeBuilder<Settings> e)
        {
            e.ToTable("Settings");
            e.HasKey(x => x.SettingKey);

            e.Property(x => x.SettingKey)
                .HasMaxLength(100);
            e.Property(x => x.SettingValue)
                .IsRequired();
            e.Property(x => x.Description)
                .HasMaxLength(255);

            e.HasIndex(x => x.SettingKey)
                .IsUnique();

            e.HasOne<Users>()
                .WithMany()
                .HasForeignKey(x => x.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Users>()
                .WithMany()
                .HasForeignKey(x => x.ModifiedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
