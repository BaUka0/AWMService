using AWMService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace AWMService.Infrastructure.Configurations
{
    public class PermissionsConfiguration : IEntityTypeConfiguration<Permissions>
    {
        public void Configure(EntityTypeBuilder<Permissions> builder)
        {
            builder.HasKey(p => p.PermissionId);
            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(p => p.Description)
                .HasMaxLength(500);
            
            builder.HasIndex(p => p.Name)
                .IsUnique();

            //builder.HasMany(p => p.RolePermissions)
            //    .WithOne(rp => rp.Permission);
                
                
        }
    }
}
