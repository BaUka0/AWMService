using AWMService.Application.Interfaces;
using AWMService.Domain.Entities;
using AWMService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AWMService.Infrastructure.Repositories;

public class DirectionsRepository : GenericRepository<Directions>, IDirectionsRepository
{
    private readonly AppDbContext _context;

    public DirectionsRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Directions>> GetBySupervisorIdAsync(int supervisorId)
    {
        return await _context.Directions
            .Where(d => d.SupervisorId == supervisorId)
            .Include(d => d.Status)
            .ToListAsync();
    }

    public async Task<IEnumerable<Directions>> GetByStatusIdAsync(int statusId)
    {
        return await _context.Directions
            .Where(d => d.StatusId == statusId)
            .Include(d => d.Supervisor)
            .ToListAsync();
    }

    public async Task<Directions?> GetWithTopicsAsync(int directionId)
    {
        return await _context.Directions
            .Include(d => d.Topics)
            .FirstOrDefaultAsync(d => d.DirectionId == directionId);
    }
}
