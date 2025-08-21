using AWMService.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AWMService.Application.Abstractions.Repositories
{
    public interface IPeriodTypesRepository
    {
        Task<IReadOnlyList<PeriodTypes>> ListAllAsync(CancellationToken ct);
        Task<PeriodTypes> GetByIdAsync(int id, CancellationToken ct);
        Task<PeriodTypes> GetByNameAsync(string name, CancellationToken ct);
        Task<PeriodTypes> AddAsync(PeriodTypes entity, CancellationToken ct);
        Task UpdateAsync(PeriodTypes entity, CancellationToken ct);
        Task SoftDeleteAsync(PeriodTypes entity, CancellationToken ct);
    }
}