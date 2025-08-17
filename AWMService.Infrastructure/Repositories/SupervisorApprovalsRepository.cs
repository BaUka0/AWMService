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
    public class SupervisorApprovalsRepository : ISupervisorApprovalsRepository
    {
        private readonly AppDbContext _context;
        public SupervisorApprovalsRepository(AppDbContext context) => _context = context;

        public async Task<SupervisorApprovals?> GetApprovalByIdAsync(int id, CancellationToken ct)
        {
           return await _context.Set<SupervisorApprovals>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public async Task<IReadOnlyList<SupervisorApprovals>> ListByDepartmentAndYearAsync(int departmentId, int academicYearId, CancellationToken ct)
        {

            return await _context.Set<SupervisorApprovals>()
                .AsNoTracking()
                .Where(x => x.DepartmentId == departmentId && x.AcademicYearId == academicYearId)
                .OrderByDescending(x => x.ApprovedOn)
                .ToListAsync(ct);
        }
        
        public async Task<IReadOnlyList<int>> ListApprovedUserIdsByDepartmentAndYearAsync(int departmentId, int academicYearId, CancellationToken ct)
            => await _context.Set<SupervisorApprovals>()
                .AsNoTracking()
                .Where(x => x.DepartmentId == departmentId && x.AcademicYearId == academicYearId && x.RevokedOn == null)
                .Select(x => x.UserId)
                .ToListAsync(ct);

        public async Task<bool> IsApprovedAsync(int userId, int departmentId, int academicYearId, CancellationToken ct)
        { 
          return await _context.Set<SupervisorApprovals>()
               .AsNoTracking()
               .AnyAsync(x =>
                   x.UserId == userId &&
                   x.DepartmentId == departmentId &&
                   x.AcademicYearId == academicYearId &&
                   x.RevokedOn == null, ct);
        }

        public async Task ApproveAsync(int userId, int departmentId, int academicYearId, int actorUserId, CancellationToken ct)
        {
            var entity = await _context.Set<SupervisorApprovals>()
               .FirstOrDefaultAsync(x =>
                   x.UserId == userId &&
                   x.DepartmentId == departmentId &&
                   x.AcademicYearId == academicYearId, ct);

            var now = DateTime.UtcNow;

            if (entity is null)
            {
                await _context.Set<SupervisorApprovals>().AddAsync(new SupervisorApprovals
                {
                    UserId = userId,
                    DepartmentId = departmentId,
                    AcademicYearId = academicYearId,
                    ApprovedBy = actorUserId,
                    ApprovedOn = now,
                    ModifiedBy = null,
                    ModifiedOn = null,
                    RevokedBy = null,
                    RevokedOn = null
                }, ct);
                return;
            }
            if (entity.RevokedOn is not null)
            {
                entity.RevokedOn = null;
                entity.RevokedBy = null;
                entity.ModifiedBy = actorUserId;
                entity.ModifiedOn = now;
            }
        }

        public async Task RevokeAsync(int userId, int departmentId, int academicYearId, int actorUserId, CancellationToken ct)
        {
            var entity = await _context.Set<SupervisorApprovals>()
                .FirstOrDefaultAsync(x =>
                    x.UserId == userId &&
                    x.DepartmentId == departmentId &&
                    x.AcademicYearId == academicYearId, ct);

            if (entity is null || entity.RevokedOn is not null) return;

            entity.RevokedOn = DateTime.UtcNow;
            entity.RevokedBy = actorUserId;
            entity.ModifiedBy = actorUserId;
            entity.ModifiedOn = entity.RevokedOn;
        }
    }
}
