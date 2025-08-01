using AWMService.Domain.Entities;

namespace AWMService.Application.Interfaces;

public interface IInstitutesRepository : IGenericRepository<Institutes>
{
    Task<Institutes?> GetByNameAsync(string name);
    Task<Institutes?> GetWithDepartmentsAsync(int instituteId);
}
