using AWMService.Application.Interfaces;
using AWMService.Domain.Entities;
using AWMService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AWMService.Infrastructure.Repositories;

public class TopicsRepository : GenericRepository<Topics>, ITopicsRepository
{
    private readonly AppDbContext _context;

    public TopicsRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Topics>> GetByDirectionIdAsync(int directionId)
    {
        return await _context.Topics
            .Where(t => t.DirectionId == directionId)
            .Include(t => t.Status)
            .ToListAsync();
    }

    public async Task<IEnumerable<Topics>> GetBySupervisorIdAsync(int supervisorId)
    {
        return await _context.Topics
            .Where(t => t.SuperVisorId == supervisorId)
            .Include(t => t.Status)
            .ToListAsync();
    }

    public async Task<Topics?> GetWithDetailsAsync(int topicId)
    {
        return await _context.Topics
            .Include(t => t.Direction)
            .Include(t => t.Status)
            .Include(t => t.SuperVisor)
            .Include(t => t.Applications)
            .Include(t => t.StudentWorks)
            .FirstOrDefaultAsync(t => t.TopicId == topicId);
    }

    public async Task<bool> IsFullAsync(int topicId)
    {
        var topic = await _context.Topics
            .Include(t => t.StudentWorks)
            .FirstOrDefaultAsync(t => t.TopicId == topicId);

        return topic != null && topic.StudentWorks.Count >= topic.MaxParticipants;
    }
}