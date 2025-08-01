using AWMService.Application.Interfaces;
using AWMService.Domain.Entities;
using AWMService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AWMService.Infrastructure.Repositories;

public class DefenseSchedulesRepository : GenericRepository<DefenseSchedules>, IDefenseSchedulesRepository
{
    private readonly AppDbContext _context;

    public DefenseSchedulesRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DefenseSchedules>> GetByCommissionIdAsync(int commissionId)
    {
        return await _context.DefenseSchedules
            .Where(ds => ds.CommissionId == commissionId)
            .Include(ds => ds.StudentWorks)
            .ToListAsync();
    }

    public async Task<IEnumerable<DefenseSchedules>> GetByDateAsync(DateTime date)
    {
        return await _context.DefenseSchedules
            .Where(ds => ds.DefenseDate.Date == date.Date)
            .Include(ds => ds.Commissions)
            .ToListAsync();
    }

    public async Task<DefenseSchedules?> GetWithGradesAsync(int defenseScheduleId)
    {
        return await _context.DefenseSchedules
            .Include(ds => ds.DefenseGrades)
            .Include(ds => ds.Commissions)
            .Include(ds => ds.StudentWorks)
            .FirstOrDefaultAsync(ds => ds.DefenseSchedulesId == defenseScheduleId);
    }
}
