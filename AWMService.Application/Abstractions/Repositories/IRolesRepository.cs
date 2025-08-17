using AWMService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWMService.Application.Abstractions.Repositories
{
    public interface IRolesRepository
    {
        Task<Roles?> GetByIdAsync(int id, CancellationToken ct);
        Task<Roles?> GetByNameAsync(string name, CancellationToken ct);
        Task<IReadOnlyList<Roles>> GetByIdsWithPermissionsAsync(IEnumerable<int> roleIds, CancellationToken ct);

        Task AddPermissionToRoleAsync(int roleId, int permissionId, int actorUserId, CancellationToken ct);
        Task RemovePermissionFromRoleAsync(int roleId, int permissionId, int actorUserId, CancellationToken ct);

        Task AssignToUserAsync(int roleId, int userId, int actorUserId, CancellationToken ct);
        Task RemoveFromUserAsync(int roleId, int userId, int actorUserId, CancellationToken ct);

  

    }
}
