using AWMService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace AWMService.Infrastructure.Configurations
{
    public class EvaluationScoresConfiguration : IEntityTypeConfiguration<EvaluationScores>
    {
        public void Configure(EntityTypeBuilder<EvaluationScores> builder)
        {
            builder.HasKey(es => es.EvaluationScoresId);

            builder.HasOne(es => es.DefenseGrade)
                .WithMany(dg => dg.EvaluationScores)
                .HasForeignKey(es => es.DefenseGradeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(es => es.Criteria)
                .WithMany(ec => ec.EvaluationScores)
                .HasForeignKey(es => es.CriteriaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(es => es.CommissionMember)
                .WithMany(u => u.EvaluationScores)
                .HasForeignKey(es => es.CommissionMemberId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
