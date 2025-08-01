using AWMService.Domain.Entities;

namespace AWMService.Application.Interfaces;

public interface IStatusesRepository : IGenericRepository<Statuses>
{
    Task<IEnumerable<Statuses>> GetByEntityTypeAsync(string entityType);
    Task<Statuses?> GetByNameAndEntityTypeAsync(string name, string entityType);
}
