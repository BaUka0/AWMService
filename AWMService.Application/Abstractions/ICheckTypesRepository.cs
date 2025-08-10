using AWMService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWMService.Application.Abstractions
{
    public interface ICheckTypesRepository
    {
        Task<CheckTypes?> GetCheckTypeByIdAsync(int id, CancellationToken ct);
        Task<IReadOnlyList<CheckTypes>> ListActiveAsync(CancellationToken ct);

        Task AddCheckTypeAsync(string name, CancellationToken ct);
        Task SoftDeleteAsync(int id, int actorUserId, CancellationToken ct);
    }
}
