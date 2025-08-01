using AWMService.Domain.Entities;

namespace AWMService.Application.Interfaces; 

public interface IPeriodTypesRepository : IGenericRepository<PeriodTypes>
{
    Task<PeriodTypes?> GetByNameAsync(string name);
}
