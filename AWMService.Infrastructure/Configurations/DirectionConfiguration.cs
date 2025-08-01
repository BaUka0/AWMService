using AWMService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AWMService.Infrastructure.Configurations
{
    public class DirectionConfiguration : IEntityTypeConfiguration<Directions>
    {
        public void Configure(EntityTypeBuilder<Directions> builder)
        {
            builder.HasKey(d => d.DirectionId);
            builder.Property(d => d.NameKz)
                .HasMaxLength(255);
            builder.Property(d => d.NameRu)
                .HasMaxLength(255);
            builder.Property(d => d.NameEn)
                .HasMaxLength(255);

            builder.HasOne(d => d.Supervisor)
                .WithMany(u => u.Directions)
                .HasForeignKey(d => d.SupervisorId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(d => d.Status)
                .WithMany(s => s.Directions)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.Restrict);
        }


    }
}
