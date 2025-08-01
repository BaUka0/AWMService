using AWMService.Domain.Entities;

namespace AWMService.Application.Interfaces;

public interface ITopicsRepository : IGenericRepository<Topics>
{
    Task<IEnumerable<Topics>> GetByDirectionIdAsync(int directionId);
    Task<IEnumerable<Topics>> GetBySupervisorIdAsync(int supervisorId);
    Task<Topics?> GetWithDetailsAsync(int topicId);
    Task<bool> IsFullAsync(int topicId);
}
