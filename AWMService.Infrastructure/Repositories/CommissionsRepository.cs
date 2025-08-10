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
    public class CommissionsRepository : ICommissionsRepository
    {
        private readonly AppDbContext _context;
        public CommissionsRepository(AppDbContext context) => _context = context;

        public async Task<Commissions?> GetCommissionsByIdAsync(int id, CancellationToken ct)
        { 
          return await _context.Set<Commissions>()
               .AsNoTracking()
               .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted, ct);
        }

        public async Task<IReadOnlyList<Commissions>> ListByPeriodAsync(int periodId, CancellationToken ct) 
        { 
           return await _context.Set<Commissions>()
               .AsNoTracking()
               .Where(c => !c.IsDeleted && c.PeriodId == periodId)
               .OrderBy(c => c.Name)
               .ThenBy(c => c.Id)
               .ToListAsync(ct);
        }

        public async Task<IReadOnlyList<Commissions>> ListByDepartmentAsync(int departmentId, CancellationToken ct)
        { 
            return await _context.Set<Commissions>()
                .AsNoTracking()
                .Where(c => !c.IsDeleted && c.DepartmentId == departmentId)
                .OrderBy(c => c.Name)
                .ThenBy(c => c.Id)
                .ToListAsync(ct);
        }

        public async Task AddAsync(string name, int commissionTypeId, int secretaryId, int periodId, int departmentId, int actorUserId, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.", nameof(name));

            var now = DateTime.UtcNow;

            var entity = new Commissions
            {
                Name = name.Trim(),
                CommissionTypeId = commissionTypeId,
                SecretaryId = secretaryId,
                PeriodId = periodId,
                DepartmentId = departmentId,
                CreatedOn = now,
                CreatedBy = actorUserId,
                ModifiedBy = null,
                ModifiedOn = null,
                IsDeleted = false,
                DeletedOn = null,
                DeletedBy = null
            };

            await _context.Set<Commissions>().AddAsync(entity, ct);
        }


        public async Task UpdateAsync(int id, string? name, int? commissionTypeId, int? secretaryId, int? periodId, int? departmentId, int actorUserId, CancellationToken ct)
        {
            var entity = await _context.Set<Commissions>()
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted, ct)
                ?? throw new KeyNotFoundException($"Commission #{id} not found.");

            if (name is not null)
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("Name cannot be empty.", nameof(name));
                entity.Name = name.Trim();
            }

            if (commissionTypeId is not null) entity.CommissionTypeId = commissionTypeId.Value;
            if (secretaryId is not null) entity.SecretaryId = secretaryId.Value;
            if (periodId is not null) entity.PeriodId = periodId.Value;
            if (departmentId is not null) entity.DepartmentId = departmentId.Value;

            entity.ModifiedBy = actorUserId;
            entity.ModifiedOn = DateTime.UtcNow;
        }

        public async Task SoftDeleteAsync(int id, int actorUserId, CancellationToken ct)
        {
            var entity = await _context.Set<Commissions>()
                .FirstOrDefaultAsync(c => c.Id == id, ct);

            if (entity is null || entity.IsDeleted) return;

            entity.IsDeleted = true;
            entity.DeletedOn = DateTime.UtcNow;
            entity.DeletedBy = actorUserId;
        }

    }
}

