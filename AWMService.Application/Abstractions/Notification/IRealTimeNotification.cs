using AWMService.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWMService.Application.Abstractions.Notification
{
    public interface IRealTimeNotification
    {
        Task NotifyUserAsync(string userId, NotificationDto payload, CancellationToken ct);
        Task NotifyGroupAsync(string group, NotificationDto payload, CancellationToken ct);
    }
}
