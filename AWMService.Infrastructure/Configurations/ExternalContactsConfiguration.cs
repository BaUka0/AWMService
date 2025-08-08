using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AWMService.Domain.Entities;

namespace AWMService.Infrastructure.Configurations;

public class ExternalContactsConfiguration : IEntityTypeConfiguration<ExternalContacts>
{
    public void Configure(EntityTypeBuilder<ExternalContacts> e)
    {
        e.ToTable("ExternalContacts");
        e.HasKey(x => x.Id);

        e.Property(x => x.FullName)
            .IsRequired()
            .HasMaxLength(255);
        e.Property(x => x.Position)
            .HasMaxLength(255);
        e.Property(x => x.Organization)
            .HasMaxLength(255);
        e.Property(x => x.Email)
            .HasMaxLength(100);
        e.Property(x => x.PhoneNumber)
            .HasMaxLength(50);

        e.Property(x => x.CreatedOn)
            .IsRequired();
        e.Property(x => x.IsDeleted)
            .HasDefaultValue(false);

        e.HasIndex(x => x.FullName);
        e.HasIndex(x => new { x.Organization, x.FullName });

        e.HasOne<Users>()
            .WithMany()
            .HasForeignKey(x => x.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);
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
