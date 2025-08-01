using AWMService.Domain.Entities;

namespace AWMService.Application.Interfaces;

public interface IDefenseSchedulesRepository : IGenericRepository<DefenseSchedules>
{
    Task<IEnumerable<DefenseSchedules>> GetByCommissionIdAsync(int commissionId);
    Task<IEnumerable<DefenseSchedules>> GetByDateAsync(DateTime date);
    Task<DefenseSchedules?> GetWithGradesAsync(int defenseScheduleId);
}
