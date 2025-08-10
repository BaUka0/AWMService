using AWMService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWMService.Application.Abstractions
{
    public interface IWorkTypesRepository
    {
        Task<WorkTypes?> GetWorkTypeByIdAsync(int id, CancellationToken ct);
        Task<IReadOnlyList<WorkTypes>> GetAllWorkTypesAsync(CancellationToken ct);

        Task DeleteWorkTypeAsync(int id, int actorUserId, CancellationToken ct);

    }
}
