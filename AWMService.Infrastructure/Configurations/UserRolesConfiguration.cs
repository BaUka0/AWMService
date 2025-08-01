using AWMService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace AWMService.Infrastructure.Configurations
{
    public class UserRolesConfiguration : IEntityTypeConfiguration<UserRoles>
    {

        public void Configure(EntityTypeBuilder<UserRoles> builder)
        {

            builder.HasKey(cm => new { cm.UserId, cm.RoleId });

            builder.HasOne(u => u.Users)
                .WithMany(ur => ur.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ur => ur.Roles)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
