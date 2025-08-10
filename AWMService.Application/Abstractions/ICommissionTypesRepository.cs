using AWMService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AWMService.Application.Abstractions
{
    public interface ICommissionTypesRepository
    {
        Task<CommissionTypes?> GetCommissionTypesByIdAsync(int id, CancellationToken ct);
        Task<IReadOnlyList<CommissionTypes>> GetAllCommissionTypesAsync(CancellationToken ct);

        Task AddCommissionTypesAsync(string name, CancellationToken ct);
        Task DeleteCommissionTypesAsync(int id, int actorUserId, CancellationToken ct);
    }
}
