using AWMService.Domain.Entities;

namespace AWMService.Application.Interfaces;

public interface ICommissionTypesRepository : IGenericRepository<CommissionTypes>
{
    Task<CommissionTypes?> GetByNameAsync(string name);
}
