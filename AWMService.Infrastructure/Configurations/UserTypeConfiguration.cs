using AWMService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AWMService.Infrastructure.Configurations
{
    public class UserTypeConfiguration : IEntityTypeConfiguration<UserTypes>
    {
        public void Configure(EntityTypeBuilder<UserTypes> e)
        {
            e.ToTable("UserTypes");
            e.HasKey(x => x.Id);
            e.Property(x => x.Name)
             .IsRequired()
             .HasMaxLength(100);
        }
    }
}
