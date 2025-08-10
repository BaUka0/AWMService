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
    public class PeriodTypesRepository : IPeriodTypesRepository
    {
        private readonly AppDbContext _context;

        public PeriodTypesRepository(AppDbContext context)=> _context = context;


        public async Task<PeriodTypes?> GetPeriodTypeByIdAsync(int id, CancellationToken ct)
        {
            return await _context.Set<PeriodTypes>()
                .AsNoTracking()
                .FirstOrDefaultAsync(pt => pt.Id == id, ct);
        }

        public async Task<IReadOnlyList<PeriodTypes>> GetAllPeriodTypesAsync(CancellationToken ct)
        {
            return await _context.Set<PeriodTypes>()
                .AsNoTracking()
                .Where(pt => !pt.IsDeleted)
                .OrderBy(pt => pt.Name)
                .ToListAsync(ct);
        }

        public async Task AddPeriodTypeAsync(string name, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.", nameof(name));

            var entity = new PeriodTypes
            {
                Name = name.Trim(),
                IsDeleted = false,
                DeletedOn = null,
                DeletedBy = null
            };

            await _context.Set<PeriodTypes>().AddAsync(entity, ct);
        }


        public async Task DeletePeriodTypeAsync(int id, int actorUserId, CancellationToken ct)
        {
            var entity = await _context.Set<PeriodTypes>()
                .FirstOrDefaultAsync(pt => pt.Id == id, ct);

            if (entity is null || entity.IsDeleted) return;

            entity.IsDeleted = true;
            entity.DeletedOn = DateTime.UtcNow;
            entity.DeletedBy = actorUserId;
        }
    }
}
