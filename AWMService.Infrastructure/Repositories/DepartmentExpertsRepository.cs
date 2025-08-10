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
    public class DepartmentExpertsRepository : IDepartmentExpertsRepository
    {
        private readonly AppDbContext _context;
        public DepartmentExpertsRepository(AppDbContext context) => _context = context;

        public async Task<DepartmentExperts?> GetDepartmentExpertsByIdAsync(int id, CancellationToken ct)
        {
            return await _context.Set<DepartmentExperts>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public async Task<IReadOnlyList<DepartmentExperts>> ListByDepartmentAndYearAsync(int departmentId, int academicYearId, int? checkTypeId, CancellationToken ct)
        {
            var q = _context.Set<DepartmentExperts>()
                .AsNoTracking()
                .Where(x => x.DepartmentId == departmentId && x.AcademicYearId == academicYearId);

            if (checkTypeId is not null)
                q = q.Where(x => x.CheckTypeId == checkTypeId);

            return await q
                .OrderByDescending(x => x.AssignedOn)
                .ToListAsync(ct);
        }

        public async Task<IReadOnlyList<DepartmentExperts>> ListByUserAndYearAsync(int userId, int academicYearId, CancellationToken ct) 
        {
            return await _context.Set<DepartmentExperts>()
                .AsNoTracking()
                .Where(x => x.UserId == userId && x.AcademicYearId == academicYearId)
                .OrderByDescending(x => x.AssignedOn)
                .ToListAsync(ct);
        }


        public Task<bool> IsAssignedAsync(int userId, int departmentId, int checkTypeId, int academicYearId, CancellationToken ct) =>
          _context.Set<DepartmentExperts>()
              .AsNoTracking()
              .AnyAsync(x =>
                  x.UserId == userId &&
                  x.DepartmentId == departmentId &&
                  x.CheckTypeId == checkTypeId &&
                  x.AcademicYearId == academicYearId &&
                  x.RevokedOn == null, ct);


        public async Task AssignAsync(int userId, int departmentId, int checkTypeId, int academicYearId, int actorUserId, CancellationToken ct)
        {
            var entity = await _context.Set<DepartmentExperts>()
                .FirstOrDefaultAsync(x =>
                    x.UserId == userId &&
                    x.DepartmentId == departmentId &&
                    x.CheckTypeId == checkTypeId &&
                    x.AcademicYearId == academicYearId, ct);

            var now = DateTime.UtcNow;

            if (entity is null)
            {
                await _context.Set<DepartmentExperts>().AddAsync(new DepartmentExperts
                {
                    UserId = userId,
                    DepartmentId = departmentId,
                    CheckTypeId = checkTypeId,
                    AcademicYearId = academicYearId,
                    AssignedBy = actorUserId,
                    AssignedOn = now,
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

        public async Task RevokeAsync(int userId, int departmentId, int checkTypeId, int academicYearId, int actorUserId, CancellationToken ct)
        {
            var entity = await _context.Set<DepartmentExperts>()
                .FirstOrDefaultAsync(x =>
                    x.UserId == userId &&
                    x.DepartmentId == departmentId &&
                    x.CheckTypeId == checkTypeId &&
                    x.AcademicYearId == academicYearId, ct);

            if (entity is null || entity.RevokedOn is not null) return;

            var now = DateTime.UtcNow;
            entity.RevokedOn = now;
            entity.RevokedBy = actorUserId;
            entity.ModifiedBy = actorUserId;
            entity.ModifiedOn = now;
        }


        public async Task UpdateAssignmentAsync(int id, int? departmentId, int? checkTypeId, int? academicYearId, int actorUserId, CancellationToken ct)
        {
            var entity = await _context.Set<DepartmentExperts>()
                .FirstOrDefaultAsync(x => x.Id == id, ct)
                ?? throw new KeyNotFoundException($"DepartmentExpert #{id} not found.");
            var newDept = departmentId ?? entity.DepartmentId;
            var newType = checkTypeId ?? entity.CheckTypeId;
            var newYear = academicYearId ?? entity.AcademicYearId;
            var duplicateExists = await _context.Set<DepartmentExperts>()
                .AsNoTracking()
                .AnyAsync(x =>
                    x.Id != id &&
                    x.UserId == entity.UserId &&
                    x.DepartmentId == newDept &&
                    x.CheckTypeId == newType &&
                    x.AcademicYearId == newYear &&
                    x.RevokedOn == null, ct);

            if (duplicateExists)
                throw new InvalidOperationException("Active assignment with the same parameters already exists.");
            entity.DepartmentId = newDept;
            entity.CheckTypeId = newType;
            entity.AcademicYearId = newYear;
            entity.ModifiedBy = actorUserId;
            entity.ModifiedOn = DateTime.UtcNow;
        }


    }
}
