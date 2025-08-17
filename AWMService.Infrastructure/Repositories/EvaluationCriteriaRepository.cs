using AWMService.Application.Abstractions.Repositories;
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
    public class EvaluationCriteriaRepository : IEvaluationCriteriaRepository
    {
        private readonly AppDbContext _context;
        public EvaluationCriteriaRepository(AppDbContext context) => _context = context;
        public async Task<EvaluationCriteria?> GetCriteriaByIdAsync(int id, CancellationToken ct) 
        { 
           return await _context.Set<EvaluationCriteria>()
               .AsNoTracking()
               .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
        }
        public async Task<IReadOnlyList<EvaluationCriteria>> ListActiveAsync(int? commissionTypeId, CancellationToken ct)
        {
            var q = _context.Set<EvaluationCriteria>()
                .AsNoTracking()
                .Where(x => !x.IsDeleted);

            if (commissionTypeId is not null)
                q = q.Where(x => x.CommissionTypeId == commissionTypeId);

            return await q
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Id)
                .ToListAsync(ct);
        }

        public async Task AddCriteriaAsync(string name, int commissionTypeId, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.", nameof(name));

            var entity = new EvaluationCriteria
            {
                Name = name.Trim(),
                CommissionTypeId = commissionTypeId,
                IsDeleted = false,
                DeletedOn = null,
                DeletedBy = null
            };

            await _context.Set<EvaluationCriteria>().AddAsync(entity, ct);
        }

        public async Task SoftDeleteAsync(int id, int actorUserId, CancellationToken ct)
        {
            var entity = await _context.Set<EvaluationCriteria>()
                .FirstOrDefaultAsync(x => x.Id == id, ct);

            if (entity is null || entity.IsDeleted) return;

            entity.IsDeleted = true;
            entity.DeletedOn = DateTime.UtcNow;
            entity.DeletedBy = actorUserId;
        }
    }
}
