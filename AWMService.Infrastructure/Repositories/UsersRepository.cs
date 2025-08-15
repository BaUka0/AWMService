using AWMService.Application.Abstractions;
using AWMService.Domain.Entities;
using AWMService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AWMService.Infrastructure.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly AppDbContext _context;
        public UsersRepository(AppDbContext context) => _context = context;


        public async Task<Users?> GetByIdAsync(int id, CancellationToken ct)
        {
            return await _context.Set<Users>()
                .FirstOrDefaultAsync(u => u.Id == id, ct);
        }

        public async Task<Users?> GetByLoginAsync(string login, CancellationToken ct)
        {
            return await _context.Set<Users>()
                .FirstOrDefaultAsync(u => u.Login == login, ct);
        }

        public async Task<Users?> GetByEmailAsync(string email, CancellationToken ct)
        {
            return await _context.Set<Users>()
                .FirstOrDefaultAsync(u => u.Email == email, ct);
        }
        public async Task<Users?> GetByEmailWithRolesAsync(string email, CancellationToken ct)
        {
            return await _context.Set<Users>()
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                        .ThenInclude(r => r.RolePermissions)
                            .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(u => u.Email == email, ct);
        }

        public async Task<Users?> GetByRefreshTokenAsync(string refreshToken, CancellationToken ct)
        {
            return await _context.Set<Users>()
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                        .ThenInclude(r => r.RolePermissions)
                            .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken, ct);
        }

        public async Task<IReadOnlyList<Users>> GetByIdsAsync (IEnumerable<int> ids, CancellationToken ct)
        {
            var list = await _context.Set<Users>()
                .Where(u => ids.Contains(u.Id))
                .ToListAsync(ct);

            return list;
        }
        
        public async Task AddUserAsync(Users user, CancellationToken ct)
        {
            await _context.Set<Users>().AddAsync(user, ct);
        }
    }
}