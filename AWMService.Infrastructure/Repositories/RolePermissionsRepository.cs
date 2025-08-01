using AWMService.Application.Interfaces;
using AWMService.Domain.Entities;
using AWMService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AWMService.Infrastructure.Repositories;

public class RolePermissionsRepository : GenericRepository<RolePermissions>, IRolePermissionsRepository
{
    private readonly AppDbContext _context;

    public RolePermissionsRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Permissions>> GetPermissionsByRoleIdAsync(int roleId)
    {
        return await _context.RolePermissions
            .Where(rp => rp.RoleId == roleId)
            .Include(rp => rp.Permission)
            .Select(rp => rp.Permission)
            .ToListAsync();
    }

    public async Task<IEnumerable<Roles>> GetRolesByPermissionIdAsync(int permissionId)
    {
        return await _context.RolePermissions
            .Where(rp => rp.PermissionId == permissionId)
            .Include(rp => rp.Role)
            .Select(rp => rp.Role)
            .ToListAsync();
    }

    public async Task<bool> RoleHasPermissionAsync(int roleId, int permissionId)
    {
        return await _context.RolePermissions
            .AnyAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);
    }
}
