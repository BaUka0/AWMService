using AWMService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace AWMService.Infrastructure.Configurations
{
    public class TopicsConfiguration : IEntityTypeConfiguration<Topics>
    {
        public void Configure(EntityTypeBuilder<Topics> e)
        {
            e.ToTable("Topics");
            e.HasKey(x => x.Id);

            e.Property(x => x.TitleKz)
                .HasMaxLength(500);
            e.Property(x => x.TitleRu)
                .HasMaxLength(500);
            e.Property(x => x.TitleEn)
                .HasMaxLength(500);
            e.Property(x => x.MaxParticipants)
                .HasDefaultValue(1);
            e.Property(x => x.IsDeleted)
                .HasDefaultValue(false);

            e.HasIndex(x => x.DirectionId);
            e.HasIndex(x => x.SupervisorId);
            e.HasIndex(x => x.StatusId);
            e.HasIndex(x => x.CreatedOn);

            e.HasOne(x => x.Direction)
                .WithMany(d => d.Topics)
                .HasForeignKey(x => x.DirectionId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Supervisor)
                .WithMany()
                .HasForeignKey(x => x.SupervisorId)
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
            e.HasOne<Users>()
                .WithMany()
                .HasForeignKey(x => x.DeletedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
