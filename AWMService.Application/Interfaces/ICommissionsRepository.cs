using AWMService.Domain.Entities;

namespace AWMService.Application.Interfaces;

public interface ICommissionsRepository : IGenericRepository<Commissions>
{
    Task<IEnumerable<Commissions>> GetByDepartmentIdAsync(int departmentId);
    Task<Commissions?> GetWithMembersAsync(int commissionId);
}
