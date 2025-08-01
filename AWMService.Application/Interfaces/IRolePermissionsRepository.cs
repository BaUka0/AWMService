using AWMService.Domain.Entities;

namespace AWMService.Application.Interfaces;

public interface IRolePermissionsRepository : IGenericRepository<RolePermissions>
{
    Task<IEnumerable<Permissions>> GetPermissionsByRoleIdAsync(int roleId);
    Task<IEnumerable<Roles>> GetRolesByPermissionIdAsync(int permissionId);
    Task<bool> RoleHasPermissionAsync(int roleId, int permissionId);
}
