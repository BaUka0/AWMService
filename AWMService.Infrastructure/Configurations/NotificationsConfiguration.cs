using AWMService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AWMService.Infrastructure.Configurations
{
    public class NotificationsConfiguration : IEntityTypeConfiguration<Notifications>
    {
        public void Configure(EntityTypeBuilder<Notifications> e)
        {
            e.ToTable("Notifications");
            e.HasKey(x => x.Id);

            e.Property(x => x.Id)
                .ValueGeneratedOnAdd();
            e.Property(x => x.Message)
                .IsRequired();
            e.Property(x => x.IsRead)
                .HasDefaultValue(false);

            e.HasIndex(x => new { x.UserId, x.IsRead, x.CreatedOn });
            e.HasIndex(x => x.CreatedOn);

            e.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
