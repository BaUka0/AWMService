using AWMService.Domain.Entities;

namespace AWMService.Application.Abstractions.Repositories
{
    public interface IPeriodsRepository
    {
        Task<IReadOnlyList<Periods>> GetAllAsync(CancellationToken ct);
        Task<Periods> GetByIdAsync(int id, CancellationToken ct);
        Task<int> AddAsync(Periods period, CancellationToken ct);
        Task UpdateAsync(Periods period, CancellationToken ct);
        Task SoftDeleteAsync(Periods period, CancellationToken ct);
    }
}