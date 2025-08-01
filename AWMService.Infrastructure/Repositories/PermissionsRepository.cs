using AWMService.Application.Interfaces;
using AWMService.Domain.Entities;
using AWMService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AWMService.Infrastructure.Repositories;

public class PermissionsRepository : GenericRepository<Permissions>, IPermissionsRepository
{
    private readonly AppDbContext _context;

    public PermissionsRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Permissions?> GetByNameAsync(string name)
    {
        return await _context.Permissions
            .FirstOrDefaultAsync(p => p.Name == name);
    }

    public async Task<IEnumerable<Permissions>> GetWithRolesAsync()
    {
        return await _context.Permissions
            .Include(p => p.RolePermissions)
            .ThenInclude(rp => rp.Role)
            .ToListAsync();
    }
}
