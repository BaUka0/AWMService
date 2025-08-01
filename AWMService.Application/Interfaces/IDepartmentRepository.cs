using AWMService.Domain.Entities;

namespace AWMService.Application.Interfaces;

public interface IDepartmentRepository : IGenericRepository<Department>
{
    Task<IEnumerable<Department>> GetByInstituteIdAsync(int instituteId);
    Task<Department?> GetWithUsersAsync(int departmentId);
}
