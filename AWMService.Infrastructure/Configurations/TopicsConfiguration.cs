using AWMService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace AWMService.Infrastructure.Configurations
{
    public class TopicsConfiguration : IEntityTypeConfiguration<Topics>
    {
        public void Configure(EntityTypeBuilder<Topics> builder)
        {
            builder.HasKey(t => t.TopicId);
            builder.Property(t => t.TitleKz)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(t => t.TitleRu)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(t => t.TitleEn)
                .IsRequired()
                .HasMaxLength(500);

            builder.HasOne(t => t.Direction)
                .WithMany(d => d.Topics)
                .HasForeignKey(t => t.DirectionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.SuperVisor)
                .WithMany(u => u.Topics)
                .HasForeignKey(t => t.SuperVisorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.Status)
                .WithMany(s => s.Topics)
                .HasForeignKey(t => t.StatusId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
