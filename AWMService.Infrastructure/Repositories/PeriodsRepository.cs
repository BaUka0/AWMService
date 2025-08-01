using AWMService.Application.Interfaces;
using AWMService.Domain.Entities;
using AWMService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AWMService.Infrastructure.Repositories;


public class PeriodsRepository : GenericRepository<Periods>, IPeriodsRepository
{
    private readonly AppDbContext _context;

    public PeriodsRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Periods>> GetByAcademicYearIdAsync(int yearId)
    {
        return await _context.Periods
            .Where(p => p.AcademicYearId == yearId)
            .Include(p => p.PeriodType)
            .ToListAsync();
    }

    public async Task<IEnumerable<Periods>> GetByTypeIdAsync(int typeId)
    {
        return await _context.Periods
            .Where(p => p.PeriodTypeId == typeId)
            .ToListAsync();
    }

    public async Task<Periods?> GetCurrentPeriodAsync(int typeId, DateTime date)
    {
        return await _context.Periods
            .FirstOrDefaultAsync(p =>
                p.PeriodTypeId == typeId &&
                p.StartDate <= date &&
                p.EndDate >= date);
    }
}