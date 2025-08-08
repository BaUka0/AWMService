using AWMService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace AWMService.Infrastructure.Configurations
{
    public class EvaluationScoresConfiguration : IEntityTypeConfiguration<EvaluationScores>
    {
        public void Configure(EntityTypeBuilder<EvaluationScores> e)
        {
            e.ToTable("EvaluationScores");
            e.HasKey(x => x.Id);

            e.HasIndex(x => x.DefenseGradeId);
            e.HasIndex(x => x.CriteriaId);
            e.HasIndex(x => x.CommissionMemberId);

            e.HasOne(x => x.DefenseGrade)
                .WithMany(g => g.EvaluationScores)
                .HasForeignKey(x => x.DefenseGradeId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Criteria)
                .WithMany(c => c.EvaluationScores)
                .HasForeignKey(x => x.CriteriaId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.CommissionMember)
                .WithMany()
                .HasForeignKey(x => x.CommissionMemberId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasOne<Users>()
                .WithMany()
                .HasForeignKey(x => x.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne<Users>()
                .WithMany()
                .HasForeignKey(x => x.ModifiedBy)
                .OnDelete(DeleteBehavior.Restrict);

            
            e.ToTable(tb => tb.HasCheckConstraint("CK_EvaluationScores_ScoreValue_NonNegative", "[ScoreValue] >= 0"));
        }
    }
}
