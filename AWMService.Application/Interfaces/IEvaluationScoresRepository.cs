using AWMService.Domain.Entities;

namespace AWMService.Application.Interfaces;

public interface IEvaluationScoresRepository : IGenericRepository<EvaluationScores>
{
    Task<IEnumerable<EvaluationScores>> GetByDefenseGradeIdAsync(int defenseGradeId);
    Task<IEnumerable<EvaluationScores>> GetByCommissionMemberIdAsync(int commissionMemberId);
    Task<EvaluationScores?> GetByGradeAndCriteriaAsync(int defenseGradeId, int criteriaId, int memberId);
}
