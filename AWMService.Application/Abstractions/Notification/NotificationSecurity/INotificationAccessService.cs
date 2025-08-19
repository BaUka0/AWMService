using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWMService.Application.Abstractions.Notification.Security
{
    public interface INotificationAccessService
    {
        Task<bool> CanSubscribeAsync(string userId, string entity, string id, CancellationToken ct);
    }
}
