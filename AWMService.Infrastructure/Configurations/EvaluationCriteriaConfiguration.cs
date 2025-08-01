using AWMService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace AWMService.Infrastructure.Configurations
{
    public class EvaluationCriteriaConfiguration : IEntityTypeConfiguration<EvaluationCriteria>
    {
        public void Configure(EntityTypeBuilder<EvaluationCriteria> builder)
        {
            builder.HasKey(ec => ec.EvaluationCriteriaId);
            builder.Property(ec => ec.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasOne(ec => ec.CommissionType)
                .WithMany(ct => ct.EvaluationCriteria)
                .HasForeignKey(ec => ec.CommissionTypeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
