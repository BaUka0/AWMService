using AWMService.Domain.Entities;

namespace AWMService.Application.Interfaces;

public interface IPermissionsRepository : IGenericRepository<Permissions>
{
    Task<Permissions?> GetByNameAsync(string name);
    Task<IEnumerable<Permissions>> GetWithRolesAsync();
}
