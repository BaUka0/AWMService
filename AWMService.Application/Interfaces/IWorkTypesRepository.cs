using AWMService.Domain.Entities;

namespace AWMService.Application.Interfaces;

public interface IWorkTypesRepository : IGenericRepository<WorkTypes>
{
    Task<WorkTypes?> GetByNameAsync(string name);
    Task<WorkTypes?> GetWithStudentWorksAsync(int workTypeId);
}
