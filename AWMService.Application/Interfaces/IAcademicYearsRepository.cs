using AWMService.Domain.Entities;

namespace AWMService.Application.Interfaces
{
    public interface IAcademicYearsRepository : IGenericRepository<AcademicYears>
    {
        Task<AcademicYears?> GetByYearAsync(string year);
    }
}