using AWMService.Application.Abstractions;
using AWMService.Domain.Entities;
using AWMService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWMService.Infrastructure.Repositories
{
    public class TopicsRepository : ITopicsRepository
    {
        private readonly AppDbContext _context;
        public TopicsRepository(AppDbContext context)=> _context = context;

        public async Task<Topics?> GetTopicsByIdAsync(int id, CancellationToken ct)
        {
            return await _context.Set<Topics?>()
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted, ct);
        }

        public async Task<int> CountAssignedAsync(int topicId, CancellationToken ct)
        {
            return await _context.Set<Topics>()
                .AsNoTracking()
                .CountAsync(t => t.Id == topicId && !t.IsDeleted, ct);
        }


        public async Task<bool> HasFreeSlotAsync(int topicId, CancellationToken ct)
        {
            var topic = await _context.Set<Topics>()
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == topicId && !t.IsDeleted, ct);

            if (topic == null) return false;
            var assigned = await CountAssignedAsync(topicId, ct);
            return assigned < (topic.MaxParticipants <= 0 ? 1 : topic.MaxParticipants);
            
        }
        public async Task<IReadOnlyList<Topics>> ListByDirectionAsync(int directionId, CancellationToken ct)
        {
            return await _context.Set<Topics>()
                .AsNoTracking()
                .Where(t => t.DirectionId == directionId && !t.IsDeleted)
                .OrderBy(t=>t.TitleRu ?? t.TitleKz ?? t.TitleEn)
                .ThenBy(t => t.Id)
                .ToListAsync(ct);
        }

        public async Task AddTopicAsync(int directionId,
            string? titleKz, string? titleRu, string? titleEn,
            string? description,
            int supervisorId,
            int statusId,
            int maxParticipants,
            int actorUserId,
            CancellationToken ct)
        {
            if(string.IsNullOrWhiteSpace(titleEn) &
                string.IsNullOrWhiteSpace(titleRu) &
                string.IsNullOrWhiteSpace(titleKz)) throw new ArgumentException("All titles is required.");

            var now = DateTime.UtcNow;

            var entity = new Topics
            {
                DirectionId = directionId,
                TitleKz = titleKz,
                TitleRu = titleRu,
                TitleEn = titleEn,
                Description = description,
                MaxParticipants = maxParticipants > 0 ? maxParticipants : 1,
                SupervisorId = supervisorId,
                StatusId = statusId,
                CreatedOn = now,
                CreatedBy = actorUserId,
                ModifiedBy = null,
                ModifiedOn = null,
                IsDeleted = false,
                DeletedOn = null,
                DeletedBy = null
            };

            await _context.Set<Topics>().AddAsync(entity, ct);
        }


        public async Task UpdateTopicAsync(int id, int? directionId,
            string? titleKz, string? titleRu, string? titleEn,
            string? description,
            int? supervisorId,
            int? maxParticipants,
            int actorUserId,
            CancellationToken ct)
        {
            var entity = await _context.Set<Topics>()
                .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted, ct)
            ?? throw new KeyNotFoundException($"Topic with ID {id} not found.");

            if(directionId is not null) entity.DirectionId = directionId.Value;
            if (supervisorId is not null) entity.SupervisorId = supervisorId.Value;
            if (maxParticipants is not null && maxParticipants.Value > 0) entity.MaxParticipants = maxParticipants.Value;
            if (titleKz is not null) entity.TitleKz = titleKz.Trim();
            if (titleRu is not null) entity.TitleRu =  titleRu.Trim();
            if (titleEn is not null) entity.TitleEn = titleEn.Trim();
            if (description is not null) entity.Description = description.Trim();
            entity.ModifiedBy = actorUserId;
            entity.ModifiedOn = DateTime.UtcNow;
        }

        public async Task ChangeStatusAsync(int id, int statusId, int actorUserId, CancellationToken ct)
        {
            var entity = await _context.Set<Topics>()
                .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted, ct)
                ?? throw new KeyNotFoundException($"Topic #{id} not found.");

            entity.StatusId = statusId;
            entity.ModifiedBy = actorUserId;
            entity.ModifiedOn = DateTime.UtcNow;
        }


        public async Task SoftDeleteTopicAsync(int id, int actorUserId, CancellationToken ct)
        {
            var entity = await _context.Set<Topics>()
                .FirstOrDefaultAsync(t => t.Id == id, ct);

            if (entity is null || entity.IsDeleted) return;

            entity.IsDeleted = true;
            entity.DeletedOn = DateTime.UtcNow;
            entity.DeletedBy = actorUserId;
        }
    }
}
