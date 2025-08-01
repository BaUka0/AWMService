using AWMService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace AWMService.Infrastructure.Configurations
{
    public class RolePermissionsConfiguration : IEntityTypeConfiguration<RolePermissions>
    {
        public void Configure(EntityTypeBuilder<RolePermissions> builder)
        {
            builder.HasKey(cm => new { cm.RoleId, cm.PermissionId });

            builder.HasOne(r => r.Role)
                 .WithMany(r => r.RolePermissions)
                 .HasForeignKey(r => r.RoleId);

            builder.HasOne(p => p.Permission)
                    .WithMany(p => p.RolePermissions)
                    .HasForeignKey(p => p.PermissionId);
        }
    }
}
