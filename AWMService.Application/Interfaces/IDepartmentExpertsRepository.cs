using AWMService.Domain.Entities;

namespace AWMService.Application.Interfaces;

public interface IDepartmentExpertsRepository : IGenericRepository<DepartmentExperts>
{
    Task<IEnumerable<DepartmentExperts>> GetByDepartmentAsync(int departmentId);
    Task<IEnumerable<DepartmentExperts>> GetByAcademicYearAsync(int yearId);
    Task<IEnumerable<DepartmentExperts>> GetByCheckTypeAsync(int checkTypeId);
    Task<DepartmentExperts?> GetByExpertAndCheckTypeAsync(int userId, int checkTypeId, int yearId);
}
