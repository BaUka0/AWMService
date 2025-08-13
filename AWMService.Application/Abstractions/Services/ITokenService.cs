using AWMService.Domain.Entities;
using System.Security.Claims;

namespace AWMService.Application.Abstractions.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(Users user, IEnumerable<string> roles, IEnumerable<string> permissions);
        string GenerateRefreshToken();
        ClaimsPrincipal ValitadeToken(string token);
    }
}
