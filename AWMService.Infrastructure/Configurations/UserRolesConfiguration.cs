using AWMService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace AWMService.Infrastructure.Configurations
{
    public class UserRolesConfiguration : IEntityTypeConfiguration<UserRoles>
    {
        public void Configure(EntityTypeBuilder<UserRoles> e)
        {
            e.ToTable("UserRoles");
            e.HasKey(x => new { x.UserId, x.RoleId });

            e.Property(x => x.AssignedOn)
                .IsRequired();

            e.HasIndex(x => x.UserId);
            e.HasIndex(x => x.RoleId);

            e.HasOne(x => x.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne<Users>()
                .WithMany()
                .HasForeignKey(x => x.AssignedBy)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Users>()
                .WithMany()
                .HasForeignKey(x => x.RevokedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
