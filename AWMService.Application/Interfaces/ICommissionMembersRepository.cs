using AWMService.Domain.Entities;

namespace AWMService.Application.Interfaces;

public interface ICommissionMembersRepository : IGenericRepository<CommissionMembers>
{
    Task<IEnumerable<CommissionMembers>> GetByCommissionIdAsync(int commissionId);
    Task<IEnumerable<CommissionMembers>> GetByMemberIdAsync(int memberId);
}
