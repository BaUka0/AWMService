using AWMService.Application.Abstractions.Notification.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace AWMService.Infrastructure.Hubs
{
    [Authorize]
    public sealed class NotificationHub : Hub
    {
        private readonly INotificationAccessService _access;
        public NotificationHub(INotificationAccessService access) => _access = access;


        public override async Task OnConnectedAsync()
        {
           
            if (!string.IsNullOrWhiteSpace(Context.UserIdentifier))
                await Groups.AddToGroupAsync(Context.ConnectionId, $"user:{Context.UserIdentifier}");

            var roles = Context.User?.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                ?? Enumerable.Empty<string>();

            foreach (var r in roles)
                if (!string.IsNullOrWhiteSpace(r))
                    await Groups.AddToGroupAsync(Context.ConnectionId, $"role:{r.ToLowerInvariant()}");

            await base.OnConnectedAsync();
        }

        public async Task JoinEntityGroup(string entity, string id)
        {
            var userId = Context.UserIdentifier ?? throw new HubException("Unauthenticated");
            if (!await _access.CanSubscribeAsync(userId, entity, id, Context.ConnectionAborted))
                throw new HubException("Access denied");

            await Groups.AddToGroupAsync(Context.ConnectionId, BuildGroupName(entity, id));
        }

        public Task LeaveEntityGroup(string entity, string id) =>
            Groups.RemoveFromGroupAsync(Context.ConnectionId, BuildGroupName(entity, id));

        private static string BuildGroupName(string entity, string id)
        {
            var e = (entity ?? "").Trim().ToLowerInvariant();
            return e switch
            {
                "role" => $"role:{id.ToLowerInvariant()}",
                "work" => $"work:{id}",
                "defense" => $"defense:{id}",
                _ => $"{e}:{id}"
            };
        }
    }
}
