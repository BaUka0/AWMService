using AWMService.Domain.Entities;

namespace AWMService.Application.Interfaces;

public interface IUserRolesRepository : IGenericRepository<UserRoles>
{
    Task<IEnumerable<Roles>> GetRolesByUserIdAsync(int userId);
    Task<IEnumerable<Users>> GetUsersByRoleIdAsync(int roleId);
    Task<bool> UserHasRoleAsync(int userId, int roleId);
}
