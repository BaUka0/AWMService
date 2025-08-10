using AWMService.Application.Abstractions;
using AWMService.Domain.Entities;
using AWMService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWMService.Infrastructure.Repositories
{
    public class DefenseGradesRepository : IDefenseGradesRepository
    {
        private readonly AppDbContext _context;
        public DefenseGradesRepository(AppDbContext context) => _context = context;


        public async Task<DefenseGrades?> GetDefenseGradeByIdAsync(int id, CancellationToken ct) 
        { 
            return await _context.Set<DefenseGrades>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }
        public async Task<DefenseGrades?> GetByScheduleAsync(int defenseScheduleId, CancellationToken ct)
            { 
            return await _context.Set<DefenseGrades>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.DefenseScheduleId == defenseScheduleId, ct);
        }

        public async Task UpsertByScheduleAsync(int defenseScheduleId, double? finalScore, string? finalGrade, int statusId, int actorUserId, CancellationToken ct)
        {
            var entity = await _context.Set<DefenseGrades>()
                .FirstOrDefaultAsync(x => x.DefenseScheduleId == defenseScheduleId, ct);

            var now = DateTime.UtcNow;

            if (entity is null)
            {
                var grade = new DefenseGrades
                {
                    DefenseScheduleId = defenseScheduleId,
                    FinalScore = finalScore ?? 0,
                    FinalGrade = string.IsNullOrWhiteSpace(finalGrade) ? null : finalGrade.Trim(),
                    StatusId = statusId,
                    CreatedOn = now,
                    CreatedBy = actorUserId,
                    ModifiedBy = null,
                    ModifiedOn = null
                };
                await _context.Set<DefenseGrades>().AddAsync(grade, ct);
                return;
            }

            if (finalScore is not null) entity.FinalScore = finalScore.Value;
            if (finalGrade is not null) entity.FinalGrade = finalGrade.Trim();
            entity.StatusId = statusId;
            entity.ModifiedBy = actorUserId;
            entity.ModifiedOn = now;
        }

        public async Task UpdateAsync(int id, double? finalScore, string? finalGrade, int? statusId, int actorUserId, CancellationToken ct)
        {
            var entity = await _context.Set<DefenseGrades>()
                .FirstOrDefaultAsync(x => x.Id == id, ct)
                ?? throw new KeyNotFoundException($"DefenseGrade #{id} not found.");

            if (finalScore is not null) entity.FinalScore = finalScore.Value;
            if (finalGrade is not null) entity.FinalGrade = finalGrade.Trim();
            if (statusId is not null) entity.StatusId = statusId.Value;

            entity.ModifiedBy = actorUserId;
            entity.ModifiedOn = DateTime.UtcNow;
        }

        public async Task ChangeStatusAsync(int id, int statusId, int actorUserId, CancellationToken ct)
        {
            var entity = await _context.Set<DefenseGrades>()
                .FirstOrDefaultAsync(x => x.Id == id, ct)
                ?? throw new KeyNotFoundException($"DefenseGrade #{id} not found.");

            entity.StatusId = statusId;
            entity.ModifiedBy = actorUserId;
            entity.ModifiedOn = DateTime.UtcNow;
        }
    }
}
