using AWMService.Domain.Entities;

namespace AWMService.Application.Interfaces;

public interface IUserTypeRepository : IGenericRepository<UserTypes>
{
    Task<UserTypes?> GetByNameAsync(string name);
    Task<UserTypes?> GetWithUsersAsync(int userTypeId);
}
