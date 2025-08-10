using AWMService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWMService.Application.Abstractions
{
    public interface IEvaluationCriteriaRepository
    {
        Task<EvaluationCriteria?> GetCriteriaByIdAsync(int id, CancellationToken ct);
        Task<IReadOnlyList<EvaluationCriteria>> ListActiveAsync(int? commissionTypeId, CancellationToken ct);
        Task AddCriteriaAsync(string name, int commissionTypeId, CancellationToken ct);
        Task SoftDeleteAsync(int id, int actorUserId, CancellationToken ct);
    }
}
