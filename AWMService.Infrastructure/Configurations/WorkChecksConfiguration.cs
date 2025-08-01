using AWMService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AWMService.Infrastructure.Configurations
{
    public class WorkChecksConfiguration : IEntityTypeConfiguration<WorkChecks>
    {
        public void Configure(EntityTypeBuilder<WorkChecks> builder)
        {
            builder.HasKey(wc => wc.WorkChecksId);
            builder.Property(wc => wc.ExpertId)
                .IsRequired(false);

            builder.Property(wc => wc.CheckedOn)
                .IsRequired(false);

            builder.HasOne(wc => wc.StudentWork)
                .WithMany(sw => sw.WorkChecks)
                .HasForeignKey(wc => wc.StudentWorkId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(wc => wc.CheckType)
                .WithMany(ct => ct.WorkChecks)
                .HasForeignKey(wc => wc.CheckTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(wc => wc.Expert)
                .WithMany(u => u.WorkChecks)
                .HasForeignKey(wc => wc.ExpertId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(wc => wc.Status)
                .WithMany(s => s.WorkChecks)
                .HasForeignKey(wc => wc.StatusId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
