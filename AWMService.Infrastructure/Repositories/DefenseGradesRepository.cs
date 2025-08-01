using AWMService.Application.Interfaces;
using AWMService.Domain.Entities;
using AWMService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AWMService.Infrastructure.Repositories;

public class DefenseGradesRepository : GenericRepository<DefenseGrades>, IDefenseGradesRepository
{
    private readonly AppDbContext _context;

    public DefenseGradesRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<DefenseGrades?> GetByScheduleIdAsync(int defenseScheduledId)
    {
        return await _context.DefenseGrades
            .Include(dg => dg.Status)
            .Include(dg => dg.EvaluationScores)
            .FirstOrDefaultAsync(dg => dg.DefenseScheduledId == defenseScheduledId);
    }

    public async Task<IEnumerable<DefenseGrades>> GetByStatusAsync(int statusId)
    {
        return await _context.DefenseGrades
            .Where(dg => dg.StatusId == statusId)
            .Include(dg => dg.DefenseScheduled)
            .ToListAsync();
    }
}
