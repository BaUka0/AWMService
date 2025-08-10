using AWMService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWMService.Application.Abstractions
{
    public interface INotificationsRepository
    {
        Task EnqueueAsync(int userId, string message, CancellationToken ct);
        Task MarkReadAsync(long notificationId, CancellationToken ct);
        Task MarkAllReadByUserAsync(int userId, CancellationToken ct);
        Task<IReadOnlyList<Notifications>> ListByUserAsync(int userId, bool unreadOnly, CancellationToken ct);
    }
}
