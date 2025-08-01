using AWMService.Domain.Entities;

namespace AWMService.Application.Interfaces;

public interface IUserTypeRepository : IGenericRepository<UserType>
{
    Task<UserType?> GetByNameAsync(string name);
    Task<UserType?> GetWithUsersAsync(int userTypeId);
}
