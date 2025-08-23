using AWMService.Domain.Entities;

namespace AWMService.Application.Abstractions.Repositories
{
    public interface IAcademicYearsRepository 
    {
        Task<AcademicYears?> GetAcademicYearsByIdAsync(int id, CancellationToken ct);
        Task<AcademicYears?> GetAcademicYearsByDateAsync(DateTime date, CancellationToken ct); 
        Task<IReadOnlyList<AcademicYears>> ListAllAsync(CancellationToken ct);

        Task AddAcademicYearsAsync(string yearName, DateTime startDate, DateTime endDate, int actorUserId, CancellationToken ct);

        Task UpdateAsync(AcademicYears entity, CancellationToken ct);
        Task SoftDeleteAsync(AcademicYears entity, CancellationToken ct);
    }
}
