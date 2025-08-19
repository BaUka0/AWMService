using AWMService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWMService.Application.Abstractions.Repositories
{
    public interface IPeriodTypesRepository
    {
        Task<PeriodTypes?> GetPeriodTypeByIdAsync(int id, CancellationToken ct);
        Task<IReadOnlyList<PeriodTypes>> GetAllPeriodTypesAsync(CancellationToken ct);
        Task AddPeriodTypeAsync(string name, CancellationToken ct);
        Task DeletePeriodTypeAsync(int id, int actorUserId, CancellationToken ct);
    }
}
