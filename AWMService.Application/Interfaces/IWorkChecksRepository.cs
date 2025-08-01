using AWMService.Domain.Entities;

namespace AWMService.Application.Interfaces;

public interface IWorkChecksRepository : IGenericRepository<WorkChecks>
{
    Task<IEnumerable<WorkChecks>> GetByStudentWorkIdAsync(int studentWorkId);
    Task<IEnumerable<WorkChecks>> GetByExpertIdAsync(int expertId);
    Task<WorkChecks?> GetWithDetailsAsync(int workCheckId);
    Task<bool> HasPendingChecksAsync(int studentWorkId);
}
