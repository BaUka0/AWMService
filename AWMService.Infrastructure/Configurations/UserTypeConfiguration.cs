using AWMService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AWMService.Infrastructure.Configurations
{
    public class UserTypeConfiguration : IEntityTypeConfiguration<UserType>
    {
        public void Configure(EntityTypeBuilder<UserType> builder)
        {
            builder.HasKey(builder => builder.UserTypeId);

            builder.Property(builder => builder.Name)
                .HasMaxLength(100);

            //builder.HasMany(builder => builder.Users)
            //    .WithOne(user => user.UserType)
            //    .HasForeignKey(user => user.UserTypeId)
            //    .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
