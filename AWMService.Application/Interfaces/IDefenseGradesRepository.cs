using AWMService.Domain.Entities;

namespace AWMService.Application.Interfaces;

public interface IDefenseGradesRepository : IGenericRepository<DefenseGrades>
{
    Task<DefenseGrades?> GetByScheduleIdAsync(int defenseScheduledId);
    Task<IEnumerable<DefenseGrades>> GetByStatusAsync(int statusId);
}
