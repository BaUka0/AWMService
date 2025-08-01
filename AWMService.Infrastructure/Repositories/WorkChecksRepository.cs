using AWMService.Application.Interfaces;
using AWMService.Domain.Entities;
using AWMService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AWMService.Infrastructure.Repositories;
   
public class WorkChecksRepository : GenericRepository<WorkChecks>, IWorkChecksRepository
{
    private readonly AppDbContext _context;

    public WorkChecksRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<WorkChecks>> GetByStudentWorkIdAsync(int studentWorkId)
    {
        return await _context.WorkChecks
            .Where(wc => wc.StudentWorkId == studentWorkId)
            .Include(wc => wc.CheckType)
            .Include(wc => wc.Status)
            .Include(wc => wc.Expert)
            .ToListAsync();
    }

    public async Task<IEnumerable<WorkChecks>> GetByExpertIdAsync(int expertId)
    {
        return await _context.WorkChecks
            .Where(wc => wc.ExpertId == expertId)
            .Include(wc => wc.CheckType)
            .Include(wc => wc.Status)
            .Include(wc => wc.StudentWork)
            .ToListAsync();
    }

    public async Task<WorkChecks?> GetWithDetailsAsync(int workCheckId)
    {
        return await _context.WorkChecks
            .Include(wc => wc.StudentWork)
            .Include(wc => wc.CheckType)
            .Include(wc => wc.Status)
            .Include(wc => wc.Expert)
            .FirstOrDefaultAsync(wc => wc.WorkChecksId == workCheckId);
    }

    public async Task<bool> HasPendingChecksAsync(int studentWorkId)
    {
        return await _context.WorkChecks
            .AnyAsync(wc => wc.StudentWorkId == studentWorkId && wc.Status.Name == "Ожидает");
    }
}