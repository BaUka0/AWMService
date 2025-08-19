using AWMService.Application.Abstractions.Repositories;
using AWMService.Domain.Entities;
using AWMService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AWMService.Application.Abstractions.Repositories.IEvaluationScoresRepository;

namespace AWMService.Infrastructure.Repositories
{
    public class EvaluationScoresRepository : IEvaluationScoresRepository
    {
        private readonly AppDbContext _context;
        public EvaluationScoresRepository(AppDbContext context) => _context = context;
        public async Task<EvaluationScores?> GetScoresByIdAsync(int id, CancellationToken ct)
        {
            return await _context.Set<EvaluationScores>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public async Task<IReadOnlyList<EvaluationScores>> ListByDefenseGradeAsync(int defenseGradeId, CancellationToken ct) 
        { 
            return await _context.Set<EvaluationScores>()
                .AsNoTracking()
                .Where(x => x.DefenseGradeId == defenseGradeId)
                .OrderBy(x => x.CriteriaId)
                .ThenBy(x => x.CommissionMemberId)
                .ToListAsync(ct);
        }

        public async Task<IReadOnlyList<EvaluationScores>> ListByMemberAsync(int commissionMemberId, int? defenseGradeId, CancellationToken ct)
        {
            var q = _context.Set<EvaluationScores>()
                .AsNoTracking()
                .Where(x => x.CommissionMemberId == commissionMemberId);

            if (defenseGradeId is not null)
                q = q.Where(x => x.DefenseGradeId == defenseGradeId);

            return await q
                .OrderByDescending(x => x.CreatedOn)
                .ThenBy(x => x.Id)
                .ToListAsync(ct);
        }

        public async Task UpsertAsync(int defenseGradeId, int criteriaId, int commissionMemberId, int scoreValue, string? comment, int actorUserId, CancellationToken ct)
        {
            var entity = await _context.Set<EvaluationScores>()
                .FirstOrDefaultAsync(x =>
                    x.DefenseGradeId == defenseGradeId &&
                    x.CriteriaId == criteriaId &&
                    x.CommissionMemberId == commissionMemberId, ct);

            var now = DateTime.UtcNow;

            if (entity is null)
            {
                await _context.Set<EvaluationScores>().AddAsync(new EvaluationScores
                {
                    DefenseGradeId = defenseGradeId,
                    CriteriaId = criteriaId,
                    CommissionMemberId = commissionMemberId,
                    ScoreValue = scoreValue,
                    Comment = comment.Trim(),
                    CreatedOn = now,
                    CreatedBy = actorUserId,
                    ModifiedBy = null,
                    ModifiedOn = null
                }, ct);
                return;
            }

            entity.ScoreValue = scoreValue;
            if (comment is not null)
                entity.Comment = comment.Trim();
            entity.ModifiedBy = actorUserId;
            entity.ModifiedOn = now;
        }

        public async Task UpdateAsync(int id, int? scoreValue, string? comment, int actorUserId, CancellationToken ct)
        {
            var entity = await _context.Set<EvaluationScores>()
                .FirstOrDefaultAsync(x => x.Id == id, ct)
                ?? throw new KeyNotFoundException($"EvaluationScore #{id} not found.");

            if (scoreValue is not null) entity.ScoreValue = scoreValue.Value;
            if (comment is not null) entity.Comment = string.IsNullOrWhiteSpace(comment) ? null : comment.Trim();

            entity.ModifiedBy = actorUserId;
            entity.ModifiedOn = DateTime.UtcNow;
        }

    }
}
