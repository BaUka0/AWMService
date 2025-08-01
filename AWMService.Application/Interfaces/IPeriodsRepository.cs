using AWMService.Domain.Entities;

namespace AWMService.Application.Interfaces;

public interface IPeriodsRepository : IGenericRepository<Periods>
{
    Task<IEnumerable<Periods>> GetByAcademicYearIdAsync(int yearId);
    Task<IEnumerable<Periods>> GetByTypeIdAsync(int typeId);
    Task<Periods?> GetCurrentPeriodAsync(int typeId, DateTime date);
}
