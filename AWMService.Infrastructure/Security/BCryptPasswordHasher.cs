using AWMService.Application.Abstractions.Services;
using Microsoft.Extensions.Logging;

namespace AWMService.Infrastructure.Security
{
    public class BCryptPasswordHasher : IPasswordHasher
    {
        private readonly ILogger<BCryptPasswordHasher> _logger;

        public BCryptPasswordHasher(ILogger<BCryptPasswordHasher> logger)
        {
            _logger = logger;
        }

        public string HashPassword(string password)
        {
            _logger.LogInformation("Hashing a new password.");
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            _logger.LogInformation("Verifying a password.");
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}
