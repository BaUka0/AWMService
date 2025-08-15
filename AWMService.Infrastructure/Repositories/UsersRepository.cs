using AWMService.Application.Abstractions;
using AWMService.Domain.Entities;
using AWMService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AWMService.Infrastructure.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UsersRepository> _logger;

        public UsersRepository(AppDbContext context, ILogger<UsersRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Users?> GetByIdAsync(int id, CancellationToken ct)
        {
            _logger.LogInformation("Fetching user by ID {UserId}", id);
            return await _context.Set<Users>()
                .FirstOrDefaultAsync(u => u.Id == id, ct);
        }

        public async Task<Users?> GetByLoginAsync(string login, CancellationToken ct)
        {
            _logger.LogInformation("Fetching user by login {Login}", login);
            return await _context.Set<Users>()
                .FirstOrDefaultAsync(u => u.Login == login, ct);
        }

        public async Task<Users?> GetByEmailAsync(string email, CancellationToken ct)
        {
            _logger.LogInformation("Fetching user by email {Email}", email);
            return await _context.Set<Users>()
                .FirstOrDefaultAsync(u => u.Email == email, ct);
        }

        public async Task<Users?> GetByEmailWithRolesAsync(string email, CancellationToken ct)
        {
            _logger.LogInformation("Fetching user by email {Email} with roles and permissions", email);
            return await _context.Set<Users>()
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                        .ThenInclude(r => r.RolePermissions)
                            .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(u => u.Email == email, ct);
        }

        public async Task<Users?> GetByRefreshTokenAsync(string refreshToken, CancellationToken ct)
        {
            _logger.LogInformation("Fetching user by refresh token");
            return await _context.Set<Users>()
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                        .ThenInclude(r => r.RolePermissions)
                            .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken, ct);
        }

        public async Task<IReadOnlyList<Users>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken ct)
        {
            _logger.LogInformation("Fetching users by IDs");
            var list = await _context.Set<Users>()
                .Where(u => ids.Contains(u.Id))
                .ToListAsync(ct);
            return list;
        }

        public async Task AddUserAsync(Users user, CancellationToken ct)
        {
            _logger.LogInformation("Adding new user with email {Email}", user.Email);
            await _context.Set<Users>().AddAsync(user, ct);
        }
    }
}
