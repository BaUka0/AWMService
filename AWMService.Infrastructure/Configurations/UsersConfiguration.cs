using AWMService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace AWMService.Infrastructure.Configurations
{
    public class UsersConfiguration : IEntityTypeConfiguration<Users>
    {
        public void Configure(EntityTypeBuilder<Users> e)
        {
            e.ToTable("Users");
            e.HasKey(x => x.Id);

            e.Property(x => x.FirstName)
             .IsRequired()
             .HasMaxLength(100);
            e.Property(x => x.LastName)
             .IsRequired()
             .HasMaxLength(100);
            e.Property(x => x.SurName)
             .HasMaxLength(100);
            e.Property(x => x.Email)
             .IsRequired()
             .HasMaxLength(100);
            e.Property(x => x.Login)
             .IsRequired()
             .HasMaxLength(100);
            e.Property(x => x.PhoneNumber)
             .HasMaxLength(50);
            e.Property(x => x.IIN)
             .HasMaxLength(12);

            e.HasIndex(x => x.Email)
             .IsUnique();
            e.HasIndex(x => x.Login)
             .IsUnique();
            e.HasIndex(x => x.IIN)
             .IsUnique()
             .HasFilter("[IIN] IS NOT NULL");
            e.HasIndex(x => x.DepartmentId);
            e.HasIndex(x => new { x.LastName, x.FirstName });

            e.HasOne(x => x.UserType)
             .WithMany()
             .HasForeignKey(x => x.UserTypeId)
             .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Department)
             .WithMany()
             .HasForeignKey(x => x.DepartmentId)
             .OnDelete(DeleteBehavior.SetNull);
        }
    }
}

