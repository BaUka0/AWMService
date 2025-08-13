using AWMService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace AWMService.Infrastructure.Configurations
{
    public class AttachmentsConfiguration : IEntityTypeConfiguration<Attachments>
    {
        public void Configure(EntityTypeBuilder<Attachments> e)
        {
            e.ToTable("Attachments");
            e.HasKey(x => x.Id);

            e.Property(x => x.FileName)
                .IsRequired()
                .HasMaxLength(255);
            e.Property(x => x.FileType)
                .HasMaxLength(100);
            e.Property(x => x.FileSize)
                .HasConversion<long>();
            e.Property(x => x.IsDeleted)
                .HasDefaultValue(false);

            e.HasIndex(x => x.StudentWorkId);
            e.HasIndex(x => x.WorkCheckId);
            e.HasIndex(x => x.UploadedOn);

            e.HasOne(x => x.StudentWork)
                .WithMany(sw => sw.Attachments)
                .HasForeignKey(x => x.StudentWorkId)
                .OnDelete(DeleteBehavior.NoAction);
            e.HasOne(x => x.WorkCheck)
                .WithMany(wc => wc.Attachments)
                .HasForeignKey(x => x.WorkCheckId)
                .OnDelete(DeleteBehavior.NoAction);
            e.HasOne(x => x.UploadedByUser)
                .WithMany()
                .HasForeignKey(x => x.UploadedBy)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Users>()
                .WithMany()
                .HasForeignKey(x => x.ModifiedBy)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Users>()
                .WithMany()
                .HasForeignKey(x => x.DeletedBy)
                .OnDelete(DeleteBehavior.Restrict);

            
            e.ToTable(tb => tb.HasCheckConstraint("CK_Attachments_FileSize_NonNegative", "[FileSize] >= 0"));
        }
    }
}
