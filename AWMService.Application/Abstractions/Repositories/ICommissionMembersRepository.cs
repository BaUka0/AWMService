using AWMService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWMService.Application.Abstractions.Repositories
{
    public interface ICommissionMembersRepository
    {
        Task<CommissionMembers?> GetMemberByIdAsync(int id, CancellationToken ct);
        Task<IReadOnlyList<CommissionMembers>> ListActiveByCommissionAsync(int commissionId, CancellationToken ct);
        Task<IReadOnlyList<CommissionMembers>> ListAllByCommissionAsync(int commissionId, CancellationToken ct);
        Task<bool> IsUserActiveAsync(int commissionId, int userId, CancellationToken ct);
        Task<bool> IsExternalActiveAsync(int commissionId, int externalContactId, CancellationToken ct);
        Task AssignInternalAsync(int commissionId, int memberUserId, string roleInCommission, int actorUserId, CancellationToken ct);
        Task AssignExternalAsync(int commissionId, int externalContactId, string roleInCommission, int actorUserId, CancellationToken ct);
        Task RemoveAsync(int id, int actorUserId, CancellationToken ct);
        Task RemoveUserAsync(int commissionId, int memberUserId, int actorUserId, CancellationToken ct);
        Task RemoveExternalAsync(int commissionId, int externalContactId, int actorUserId, CancellationToken ct);
    }
}
