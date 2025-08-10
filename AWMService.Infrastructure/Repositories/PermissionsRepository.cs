using AWMService.Application.Abstractions;
using AWMService.Domain.Entities;
using AWMService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AWMService.Infrastructure.Repositories
{
    public class PermissionsRepository : IPermissionsRepository
    {
     
        private readonly AppDbContext _context;
        public PermissionsRepository(AppDbContext context) => _context = context;

        public async Task<Permissions?> GetPermissionsByIdAsync(int id, CancellationToken ct)
        {
           return await _context.Set<Permissions>()
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted, ct);
        }
        public async Task<Permissions?> GetPermissionsByNameAsync(string name, CancellationToken ct)
        {
           return await  _context.Set<Permissions>()
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Name == name && !p.IsDeleted, ct);

        }

        public async Task<IReadOnlyList<Permissions>> ListActiveAsync(CancellationToken ct) =>
           await _context.Set<Permissions>()
               .AsNoTracking()
               .Where(p => !p.IsDeleted)
               .OrderBy(p => p.Name)
               .ToListAsync(ct);


        public async Task AddPermissionAsync(string name, string? description, int actorUserId, CancellationToken ct)
        {
            var now = DateTime.UtcNow;

            var perm = new Permissions
            {
                Name = name.Trim(),
                Description = string.IsNullOrWhiteSpace(description) ? null : description,
                CreatedBy = actorUserId,
                CreatedOn = now,
                ModifiedBy = null,
                ModifiedOn = null,
                IsDeleted = false,
                DeletedOn = null,
                DeletedBy = null
            };

            await _context.Set<Permissions>().AddAsync(perm, ct);
        }

        public async Task SoftDeletePermissionAsync(int id, int actorUserId, CancellationToken ct)
        {
            var perm = await _context.Set<Permissions>()
                .FirstOrDefaultAsync(p => p.Id == id, ct);

            if (perm is null) return;                 
            if (perm.IsDeleted) return;               

            perm.IsDeleted = true;
            perm.DeletedOn = DateTime.UtcNow;
            perm.DeletedBy = actorUserId;
        }


        public async Task UpdatePermissionAsync(int id, string name, string? description, int actorUserId, CancellationToken ct)
        {
            var perm = await _context.Set<Permissions>()
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted, ct);

            if (perm is null)
                throw new KeyNotFoundException($"Permission #{id} не найден или удалён.");

            perm.Name = name.Trim();
            perm.Description = string.IsNullOrWhiteSpace(description) ? null : description;
            perm.ModifiedBy = actorUserId;
            perm.ModifiedOn = DateTime.UtcNow;
        }
    }
}
