using AWMService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace AWMService.Infrastructure.Configurations
{
    public class AttachmentsConfiguration : IEntityTypeConfiguration<Attachments>
    {
        public void Configure(EntityTypeBuilder<Attachments> builder)
        {
           builder.HasKey(c=>c.AttachmentsId);

            builder.Property(c=>c.EntityType)
                .HasMaxLength(50);
            
            builder.Property(c => c.FileName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(c => c.FileType)
                .HasMaxLength(100);

            builder.HasOne(c => c.UploadedBy)
                .WithMany(c=>c.Attachments)
                .HasForeignKey(c => c.UploadedById)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
