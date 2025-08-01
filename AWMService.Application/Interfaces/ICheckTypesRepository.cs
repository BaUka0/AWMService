using AWMService.Domain.Entities;

namespace AWMService.Application.Interfaces;

public interface ICheckTypesRepository : IGenericRepository<CheckTypes>
{
    Task<CheckTypes?> GetByNameAsync(string name);
}
