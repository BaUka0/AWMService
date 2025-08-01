using AWMService.Domain.Entities;

namespace AWMService.Application.Interfaces;

public interface IEvaluationCriteriaRepository : IGenericRepository<EvaluationCriteria>
{
    Task<IEnumerable<EvaluationCriteria>> GetByCommissionTypeIdAsync(int commissionTypeId);
    Task<EvaluationCriteria?> GetByNameAndTypeAsync(string name, int commissionTypeId);
}
