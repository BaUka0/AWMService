using AWMService.Application.Abstractions.Repositories;
using AWMService.Domain.Entities;
using AWMService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AWMService.Infrastructure.Repositories
{
    public class PeriodsRepository : IPeriodsRepository
    {
        private readonly AppDbContext _context;

        public PeriodsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Periods>> GetAllAsync(CancellationToken ct)
        {
            return await _context.Periods
                .Include(p => p.PeriodType)
                .Include(p => p.AcademicYear)
                .Include(p => p.Status)
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<Periods> GetByIdAsync(int id, CancellationToken ct)
        {
            return await _context.Periods
                .Include(p => p.PeriodType)
                .Include(p => p.AcademicYear)
                .Include(p => p.Status)
                .FirstOrDefaultAsync(p => p.Id == id, ct);
        }

        public async Task AddAsync(Periods period, CancellationToken ct)
        {
            await _context.Periods.AddAsync(period, ct);
        }

        public Task UpdateAsync(Periods period, CancellationToken ct)
        {
            _context.Periods.Update(period);
            return Task.CompletedTask;
        }

        public Task SoftDeleteAsync(Periods period, CancellationToken ct)
        {
            _context.Periods.Update(period);
            return Task.CompletedTask;
        }
    }
}