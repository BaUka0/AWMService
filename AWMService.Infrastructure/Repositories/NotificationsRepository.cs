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
    public class NotificationsRepository : INotificationsRepository
    {
        private readonly AppDbContext _context;
        public NotificationsRepository(AppDbContext context) => _context = context;

        public async Task EnqueueAsync(int userId, string message, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Message is required.", nameof(message));

            var entity = new Notifications
            {
                UserId = userId,
                Message = message, 
                IsRead = false,
                ReadOn = null,
                CreatedOn = DateTime.UtcNow
            };

            await _context.Set<Notifications>().AddAsync(entity, ct);
        }

        public async Task MarkReadAsync(long notificationId, CancellationToken ct)
        {
            var entity = await _context.Set<Notifications>()
                .FirstOrDefaultAsync(n => n.Id == notificationId, ct);

            if (entity is null) return;
            if (!entity.IsRead)
            {
                entity.IsRead = true;
                entity.ReadOn = DateTime.UtcNow;
            }
        }

        public async Task MarkAllReadByUserAsync(int userId, CancellationToken ct)
        {
            var toUpdate = await _context.Set<Notifications>()
                .Where(n => n.UserId == userId && !n.IsRead)
                .ToListAsync(ct);

            if (toUpdate.Count == 0) return;

            var now = DateTime.UtcNow;
            foreach (var n in toUpdate)
            {
                n.IsRead = true;
                n.ReadOn = now;
            }
        }

        public async Task<IReadOnlyList<Notifications>> ListByUserAsync(int userId, bool unreadOnly, CancellationToken ct)
        {
            var q = _context.Set<Notifications>()
                .AsNoTracking()
                .Where(n => n.UserId == userId);

            if (unreadOnly) q = q.Where(n => !n.IsRead);

            return await q
                .OrderByDescending(n => n.CreatedOn)
                .ThenByDescending(n => n.Id)
                .ToListAsync(ct);
        }
    }
}
