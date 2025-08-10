using AWMService.Application.Abstractions;
using AWMService.Domain.Entities;
using AWMService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AWMService.Infrastructure.Repositories
{
    public class RolesRepository : IRolesRepository
    {
        private readonly AppDbContext _context;

        public RolesRepository(AppDbContext context) => _context = context;

        public async Task<Roles?> GetByIdAsync(int id, CancellationToken ct)
        {
            return  await _context.Set<Roles>()
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id, ct);
        }

        public async Task<Roles?> GetByNameAsync(string name, CancellationToken ct)
        {
            return await _context.Set<Roles>()
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Name == name, ct);
        }
        
        public async Task AddPermissionToRoleAsync(int roleId, int permissionId, int actorUserId, CancellationToken ct)
        {
          
            var link = await _context.Set<RolePermissions>()
                .FindAsync([roleId, permissionId], ct); 

            var now = DateTime.UtcNow;

            if (link is null)
            {
                await _context.Set<RolePermissions>().AddAsync(new RolePermissions
                {
                    RoleId = roleId,
                    PermissionId = permissionId,
                    AssignedOn = now,
                    AssignedBy = actorUserId,
                    RevokedOn = null,
                    RevokedBy = null
                }, ct);
                return;
            }

            if (link.RevokedOn is not null)
            {
                link.AssignedOn = now;
                link.AssignedBy = actorUserId;
                link.RevokedOn = null;
                link.RevokedBy = null;
            }
        }

        public async Task RemovePermissionFromRoleAsync(int roleId, int permissionId, int actorUserId, CancellationToken ct)
        {
            var link = await _context.Set<RolePermissions>()
                .FindAsync([roleId, permissionId], ct);

            if (link is null || link.RevokedOn is not null) return;

            link.RevokedOn = DateTime.UtcNow;
            link.RevokedBy = actorUserId;
        }

        public async Task AssignToUserAsync(int roleId, int userId, int actorUserId, CancellationToken ct)
        {
            var link = await _context.Set<UserRoles>()
                .FindAsync([userId, roleId], ct);

            var now = DateTime.UtcNow;

            if (link is null)
            {
                await _context.Set<UserRoles>().AddAsync(new UserRoles
                {
                    RoleId = roleId,
                    UserId = userId,
                    AssignedOn = now,
                    AssignedBy = actorUserId,
                    RevokedOn = null,
                    RevokedBy = null
                }, ct);
                return;
            }

            if (link.RevokedOn is not null)
            {
                link.AssignedOn = now;
                link.AssignedBy = actorUserId;
                link.RevokedOn = null;
                link.RevokedBy = null;
            }
        }

        public async Task RemoveFromUserAsync(int roleId, int userId, int actorUserId, CancellationToken ct)
        {
            var link = await _context.Set<UserRoles>()
                .FindAsync([userId, roleId], ct);

            if (link is null || link.RevokedOn is not null) return;

            link.RevokedOn = DateTime.UtcNow;
            link.RevokedBy = actorUserId;
        }
    }
}
