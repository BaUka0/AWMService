using AWMService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWMService.Application.Abstractions
{
    public interface IEvaluationScoresRepository
    {
        
        Task<EvaluationScores?> GetScoresByIdAsync(int id, CancellationToken ct);
        Task<IReadOnlyList<EvaluationScores>> ListByDefenseGradeAsync(int defenseGradeId, CancellationToken ct);
        Task<IReadOnlyList<EvaluationScores>> ListByMemberAsync(int commissionMemberId, int? defenseGradeId, CancellationToken ct);
        Task UpsertAsync(int defenseGradeId, int criteriaId, int commissionMemberId, int scoreValue, string? comment, int actorUserId, CancellationToken ct);
       
        Task UpdateAsync(int id, int? scoreValue, string? comment, int actorUserId, CancellationToken ct);
    }
}
