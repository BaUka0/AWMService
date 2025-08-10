using AWMService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWMService.Application.Abstractions
{
    public interface IPermissionsRepository
    {
        Task<Permissions?> GetPermissionsByIdAsync(int id, CancellationToken ct);
        Task<Permissions?> GetPermissionsByNameAsync(string name, CancellationToken ct);
        Task<IReadOnlyList<Permissions>> ListActiveAsync(CancellationToken ct);

        Task AddPermissionAsync(string name, string? description, int actorUserId, CancellationToken ct);
        Task SoftDeletePermissionAsync(int id,int actorUserId, CancellationToken ct);

        Task UpdatePermissionAsync(int id, string name, string? description, int actorUserId, CancellationToken ct);



    }
}
