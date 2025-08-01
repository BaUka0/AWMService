using AWMService.Domain.Entities;

namespace AWMService.Application.Interfaces;

public interface IDirectionsRepository : IGenericRepository<Directions>
{
    Task<IEnumerable<Directions>> GetBySupervisorIdAsync(int supervisorId);
    Task<IEnumerable<Directions>> GetByStatusIdAsync(int statusId);
    Task<Directions?> GetWithTopicsAsync(int directionId);
}
