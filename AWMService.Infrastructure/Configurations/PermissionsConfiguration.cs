using AWMService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace AWMService.Infrastructure.Configurations
{
    public class PermissionsConfiguration : IEntityTypeConfiguration<Permissions>
    {
        public void Configure(EntityTypeBuilder<Permissions> e)
        {
            e.ToTable("Permissions");
            e.HasKey(x => x.Id);

            e.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);
            e.Property(x => x.Description)
                .HasMaxLength(255);
            e.Property(x => x.IsDeleted)
                .HasDefaultValue(false);
            e.Property(x => x.CreatedOn)
                .IsRequired();

            e.HasIndex(x => x.Name)
                .IsUnique();

            e.HasOne<Users>()
                .WithMany()
                .HasForeignKey(x => x.CreatedBy).
                OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Users>()
                .WithMany()
                .HasForeignKey(x => x.ModifiedBy)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Users>()
                .WithMany()
                .HasForeignKey(x => x.DeletedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
