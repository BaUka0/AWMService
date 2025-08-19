using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace AWMService.Infrastructure.Hubs
{
    public sealed class JwtSubAsUserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        => connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
           ?? connection.User?.FindFirst("sub")?.Value;
    }
}
