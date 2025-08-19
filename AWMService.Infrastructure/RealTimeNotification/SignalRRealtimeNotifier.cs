using AWMService.Application.Abstractions.Notification;
using AWMService.Application.DTOs;
using AWMService.Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace AWMService.Infrastructure.RealTimeNotification
{
    public class SignalRRealtimeNotifier : IRealTimeNotification
    {
        private readonly IHubContext<NotificationHub> _hub;
        private readonly ILogger<SignalRRealtimeNotifier> _log;

        public SignalRRealtimeNotifier(IHubContext<NotificationHub> hub, ILogger<SignalRRealtimeNotifier> log)
        {
            _hub = hub;
            _log = log;
        }

        public Task NotifyUserAsync(string userId, NotificationDto payload, CancellationToken ct)
        {
            _log.LogInformation("Notify user {UserId} with {Type}", userId, payload.Type);
            return _hub.Clients.User(userId).SendAsync("ReceiveNotification", payload, ct);
        }

        public Task NotifyGroupAsync(string group, NotificationDto payload, CancellationToken ct)
        {
            _log.LogInformation("Notify group {Group} with {Type}", group, payload.Type);
            return _hub.Clients.Group(group).SendAsync("ReceiveNotification", payload, ct);
        }

        
    }
}
