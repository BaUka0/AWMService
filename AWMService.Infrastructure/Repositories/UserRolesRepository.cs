using AWMService.Application.Interfaces;
using AWMService.Domain.Entities;
using AWMService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AWMService.Infrastructure.Repositories;

public class UserRolesRepository : GenericRepository<UserRoles>, IUserRolesRepository
{
    private readonly AppDbContext _context;

    public UserRolesRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Roles>> GetRolesByUserIdAsync(int userId)
    {
        return await _context.UserRoles
            .Where(ur => ur.UserId == userId)
            .Include(ur => ur.Roles)
            .Select(ur => ur.Roles)
            .ToListAsync();
    }

    public async Task<IEnumerable<Users>> GetUsersByRoleIdAsync(int roleId)
    {
        return await _context.UserRoles
            .Where(ur => ur.RoleId == roleId)
            .Include(ur => ur.Users)
            .Select(ur => ur.Users)
            .ToListAsync();
    }

    public async Task<bool> UserHasRoleAsync(int userId, int roleId)
    {
        return await _context.UserRoles
            .AnyAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
    }
}
