using AWMService.Domain.Entities;

namespace AWMService.Application.Interfaces;

public interface ISupervisorApprovalsRepository : IGenericRepository<SupervisorApprovals>
{
    Task<IEnumerable<SupervisorApprovals>> GetByUserIdAsync(int userId);
    Task<SupervisorApprovals?> GetByUserAndYearAsync(int userId, int academicYearId);
    Task<IEnumerable<SupervisorApprovals>> GetByDepartmentAndYearAsync(int departmentId, int academicYearId);
}
