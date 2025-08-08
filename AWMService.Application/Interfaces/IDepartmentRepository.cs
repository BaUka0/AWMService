using AWMService.Domain.Entities;

namespace AWMService.Application.Interfaces;

public interface IDepartmentRepository : IGenericRepository<Departments>
{
    Task<IEnumerable<Departments>> GetByInstituteIdAsync(int instituteId);
    Task<Departments?> GetWithUsersAsync(int departmentId);
}
