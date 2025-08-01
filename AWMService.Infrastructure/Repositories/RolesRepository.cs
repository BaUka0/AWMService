using AWMService.Application.Interfaces;
using AWMService.Domain.Entities;
using AWMService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AWMService.Infrastructure.Repositories;

public class RolesRepository : GenericRepository<Roles>, IRolesRepository
{
    private readonly AppDbContext _context;

    public RolesRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Roles?> GetByNameAsync(string name)
    {
        return await _context.Roles
            .FirstOrDefaultAsync(r => r.Name == name);
    }

    public async Task<Roles?> GetWithPermissionsAsync(int roleId)
    {
        return await _context.Roles
            .Include(r => r.RolePermissions)
            .ThenInclude(rp => rp.Permission)
            .FirstOrDefaultAsync(r => r.RoleId == roleId);
    }

    public async Task<Roles?> GetWithUsersAsync(int roleId)
    {
        return await _context.Roles
            .Include(r => r.UserRoles)
            .ThenInclude(ur => ur.Users)
            .FirstOrDefaultAsync(r => r.RoleId == roleId);
    }
}