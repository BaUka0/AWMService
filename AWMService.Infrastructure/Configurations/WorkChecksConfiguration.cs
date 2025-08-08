using AWMService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AWMService.Infrastructure.Configurations
{
    public class WorkChecksConfiguration : IEntityTypeConfiguration<WorkChecks>
    {
        public void Configure(EntityTypeBuilder<WorkChecks> e)
        {
            e.ToTable("WorkChecks");
            e.HasKey(x => x.Id);

            e.HasIndex(x => new { x.StudentWorkId, x.CheckTypeId, x.StatusId });
            e.HasIndex(x => x.SubmittedOn);
            e.HasIndex(x => x.CheckedOn);

            e.HasOne(x => x.StudentWork).
                WithMany(sw => sw.WorkChecks)
                .HasForeignKey(x => x.StudentWorkId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.CheckType)
                .WithMany(ct => ct.WorkChecks)
                .HasForeignKey(x => x.CheckTypeId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Expert)
                .WithMany()
                .HasForeignKey(x => x.ExpertId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Reviewer)
                .WithMany()
                .HasForeignKey(x => x.ReviewerId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Status)
                .WithMany()
                .HasForeignKey(x => x.StatusId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne<Users>()
                .WithMany()
                .HasForeignKey(x => x.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Users>()
                .WithMany()
                .HasForeignKey(x => x.ModifiedBy)
                .OnDelete(DeleteBehavior.Restrict);

            
            e.ToTable(tb => tb.HasCheckConstraint(
                "CK_WorkChecks_ExpertOrReviewer",
                "([ExpertId] IS NOT NULL) OR ([ReviewerId] IS NOT NULL)"
            ));
        }
    }
}
