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
    public class CheckTypesRepository : ICheckTypesRepository
    {
        private readonly AppDbContext _context;
        public CheckTypesRepository(AppDbContext context) => _context = context;

        public async Task<CheckTypes?> GetCheckTypeByIdAsync(int id, CancellationToken ct) 
        { 
            return await _context.Set<CheckTypes>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
        }

        public async Task<IReadOnlyList<CheckTypes>> ListActiveAsync(CancellationToken ct)
        {
            return await _context.Set<CheckTypes>()
                .AsNoTracking()
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.Name)
                .ToListAsync(ct);
        }

        public async Task AddCheckTypeAsync(string name, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.", nameof(name));

            await _context.Set<CheckTypes>().AddAsync(new CheckTypes
            {
                Name = name.Trim(),
                IsDeleted = false,
                DeletedOn = null,
                DeletedBy = null
            }, ct);
        }

        public async Task SoftDeleteAsync(int id, int actorUserId, CancellationToken ct)
        {
            var entity = await _context.Set<CheckTypes>()
                .FirstOrDefaultAsync(x => x.Id == id, ct);

            if (entity is null || entity.IsDeleted) return;

            entity.IsDeleted = true;
            entity.DeletedOn = DateTime.UtcNow;
            entity.DeletedBy = actorUserId;
        }
    }
}
