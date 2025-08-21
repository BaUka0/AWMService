using AWMService.Application.Abstractions.Repositories;
using AWMService.Domain.Entities;
using AWMService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AWMService.Infrastructure.Repositories
{
    public class PeriodTypesRepository : IPeriodTypesRepository
    {
        private readonly AppDbContext _context;

        public PeriodTypesRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<PeriodTypes>> ListAllAsync(CancellationToken ct)
        {
            return await _context.PeriodTypes.AsNoTracking().ToListAsync(ct);
        }

        public async Task<PeriodTypes> GetByIdAsync(int id, CancellationToken ct)
        {
            return await _context.PeriodTypes.FindAsync(new object[] { id }, ct);
        }

        public async Task<PeriodTypes> GetByNameAsync(string name, CancellationToken ct)
        {
            return await _context.PeriodTypes.FirstOrDefaultAsync(x => x.Name == name, ct);
        }

        public async Task<PeriodTypes> AddAsync(PeriodTypes entity, CancellationToken ct)
        {
            await _context.PeriodTypes.AddAsync(entity, ct);
            return entity;
        }

        public Task UpdateAsync(PeriodTypes entity, CancellationToken ct)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public Task SoftDeleteAsync(PeriodTypes entity, CancellationToken ct)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }
    }
}