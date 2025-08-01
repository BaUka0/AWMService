using AWMService.Application.Interfaces;
using AWMService.Domain.Entities;
using AWMService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AWMService.Infrastructure.Repositories;

public class StatusesRepository : GenericRepository<Statuses>, IStatusesRepository
{
    private readonly AppDbContext _context;

    public StatusesRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Statuses>> GetByEntityTypeAsync(string entityType)
    {
        return await _context.Statuses
            .Where(s => s.EntityType == entityType)
            .ToListAsync();
    }

    public async Task<Statuses?> GetByNameAndEntityTypeAsync(string name, string entityType)
    {
        return await _context.Statuses
            .FirstOrDefaultAsync(s => s.Name == name && s.EntityType == entityType);
    }
}