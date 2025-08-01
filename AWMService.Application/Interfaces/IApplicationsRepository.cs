using AWMService.Domain.Entities;

namespace AWMService.Application.Interfaces;

public interface IApplicationsRepository : IGenericRepository<Applications>
{
    Task<IEnumerable<Applications>> GetByStudentIdAsync(int studentId);
    Task<IEnumerable<Applications>> GetByStatusIdAsync(int statusId);
}
