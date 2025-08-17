using AWMService.Application.Abstractions.Repositories;
using AWMService.Domain.Entities;
using AWMService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AWMService.Infrastructure.Repositories
{
    public class ApplicationsRepository : IApplicationsRepository
    {
        private readonly AppDbContext _context;

        public ApplicationsRepository(AppDbContext context) => _context = context;

        public async Task<Applications?> GetApplicationsByIdAsync(int id, CancellationToken ct)
        {
            return await _context.Set<Applications>()
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id, ct);
        }


        public async Task<IReadOnlyList<Applications>> ListByStudentAsync(int studentId, CancellationToken ct)
        {
            return await _context.Set<Applications>()
                .AsNoTracking()
                .Where(a => a.StudentId == studentId)
                .ToListAsync(ct);
        }


        public async Task<IReadOnlyList<Applications>> ListByTopicAsync(int topicId, CancellationToken ct)
        {
            return await _context.Set<Applications>()
                .AsNoTracking()
                .Where(a => a.TopicId == topicId)
                .OrderByDescending(a => a.CreatedOn)
                .ToListAsync(ct);
        }


        public async Task AddApplicationAsync(int studentId, int topicId, int initialStatusId, int actorUserId, CancellationToken ct)
        {
            var now = DateTime.UtcNow;

            var Entity = new Applications
            {
                StudentId = studentId,
                TopicId = topicId,
                StatusId = initialStatusId,
                CreatedOn = now,
                CreatedBy = actorUserId,
                ModifiedBy = null,
                ModifiedOn = null
            };

            await _context.Set<Applications>().AddAsync(Entity, ct);
        }


        public async Task ChangeStatusAsync(int applicationId, int newStatusId, int actorUserId, CancellationToken ct)
        {
            var entity = await _context.Set<Applications>()
                .FirstOrDefaultAsync(a => a.Id == applicationId, ct)
                ?? throw new KeyNotFoundException($"Application #{applicationId} not found.");

            entity.StatusId = newStatusId;
            entity.ModifiedBy = actorUserId;
            entity.ModifiedOn = DateTime.UtcNow;
        }
    }
}
