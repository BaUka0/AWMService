using AWMService.Domain.Entities;

namespace AWMService.Application.Interfaces;

public interface IRolesRepository : IGenericRepository<Roles>
{
    Task<Roles?> GetByNameAsync(string name);
    Task<Roles?> GetWithPermissionsAsync(int roleId);
    Task<Roles?> GetWithUsersAsync(int roleId);
}
