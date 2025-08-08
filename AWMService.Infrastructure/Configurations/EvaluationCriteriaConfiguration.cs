using AWMService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace AWMService.Infrastructure.Configurations
{
    public class EvaluationCriteriaConfiguration : IEntityTypeConfiguration<EvaluationCriteria>
    {
        public void Configure(EntityTypeBuilder<EvaluationCriteria> e)
        {
            e.ToTable("EvaluationCriteria");
            e.HasKey(x => x.Id);

            e.Property(x => x.Name)
                .HasMaxLength(255);
            e.Property(x => x.IsDeleted)
                .HasDefaultValue(false);

            e.HasIndex(x => new { x.CommissionTypeId, x.Name });

            e.HasOne(x => x.CommissionType)
                .WithMany()
                .HasForeignKey(x => x.CommissionTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne<Users>()
                .WithMany()
                .HasForeignKey(x => x.DeletedBy)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
