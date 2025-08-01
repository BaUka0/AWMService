using AWMService.Domain.Entities;

namespace AWMService.Application.Interfaces;

public interface IUsersRepository : IGenericRepository<Users>
{
    Task<Users?> GetByEmailAsync(string email);
    Task<Users?> GetByLoginAsync(string login);
    Task<Users?> GetByIINAsync(string iin);
    Task<Users?> GetWithRolesAsync(int userId);
    Task<IEnumerable<Users>> GetByDepartmentIdAsync(int departmentId);
}
