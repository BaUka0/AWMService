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
    public class DirectionsRepository : IDirectionsRepository
    {
        private readonly AppDbContext _context;
        public DirectionsRepository(AppDbContext context)=> _context = context;

        public async Task<Directions?> GetDirectionByIdAsync(int id, CancellationToken ct)
        {
            return await _context.Set<Directions>()
                .AsNoTracking()
                .FirstOrDefaultAsync(d=>d.Id == id && !d.IsDeleted, ct);
        }

        public async Task<IReadOnlyList<Directions>> ListBySupervisorDepartmentAsync(
             int departmentId, CancellationToken ct)
        {
            return await _context.Set<Directions>()
                .AsNoTracking()
                .Where(d => !d.IsDeleted)
                .Join(_context.Set<Users>(),
                      d => d.SupervisorId,
                      u => u.Id,
                      (d, u) => new { d, u })
                .Where(x => x.u.DepartmentId == departmentId)
                .Select(x => x.d)
                .OrderBy(d => d.NameRu ?? d.NameKz ?? d.NameEn)
                .ToListAsync(ct);
        }

        public async Task AddDirectionAsync(string nameKz, string nameRu, string nameEn, string description, int supervisorId, int statusId, int actorUserId,
            CancellationToken ct)
        {
            if(string.IsNullOrWhiteSpace(nameKz) &
                string.IsNullOrWhiteSpace(nameEn)&
                string.IsNullOrWhiteSpace(nameRu)) throw new ArgumentException("All names must be provided.");

            var now = DateTime.UtcNow;

            var directionEntity = new Directions
            {
                NameKz = nameKz,
                NameRu = nameRu,
                NameEn = nameEn,
                Description = description,
                SupervisorId = supervisorId,
                StatusId = statusId,
                CreatedBy = actorUserId,
                CreatedOn = now,
                ModifiedBy = null,
                ModifiedOn = null,
                IsDeleted = false,
                DeletedOn = null,
                DeletedBy = null

            };

            await _context.Set<Directions>().AddAsync(directionEntity, ct);
        }


        public async Task UpdateDirectionAsync(int id, string nameKz, string nameRu, string nameEn, string description, int actorUserId, CancellationToken ct)
        {
            var entity = await _context.Set<Directions>()
                .FirstOrDefaultAsync(d => d.Id == id && !d.IsDeleted, ct) ?? throw new KeyNotFoundException($"Direction #{id} not found.");

            if (nameKz is not null) entity.NameKz = string.IsNullOrWhiteSpace(nameKz) ? null : nameKz.Trim();
            if (nameRu is not null) entity.NameRu = string.IsNullOrWhiteSpace(nameRu) ? null : nameRu.Trim();
            if (nameEn is not null) entity.NameEn = string.IsNullOrWhiteSpace(nameEn) ? null : nameEn.Trim();
            if (description is not null) entity.Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();

            entity.ModifiedBy = actorUserId;
            entity.ModifiedOn = DateTime.UtcNow;
            
        }

        public async Task ChangeDirectionStatusAsync(int id, int statusId, int actorUserId, CancellationToken ct)
        {
            
            var entity = await _context.Set<Directions>()
                .FirstOrDefaultAsync(d => d.Id == id && !d.IsDeleted, ct)
                ?? throw new KeyNotFoundException($"Direction #{id} not found.");

            entity.StatusId = statusId;
            entity.ModifiedBy = actorUserId;
            entity.ModifiedOn = DateTime.UtcNow;
        }

        public async Task SoftDeleteDirectionAsync(int id, int actorUserId, CancellationToken ct)
        {
            var entity = await _context.Set<Directions>()
               .FirstOrDefaultAsync(d => d.Id == id, ct);

            if (entity is null || entity.IsDeleted) return;

            entity.IsDeleted = true;
            entity.DeletedOn = DateTime.UtcNow;
            entity.DeletedBy = actorUserId;
        }

    }
}
