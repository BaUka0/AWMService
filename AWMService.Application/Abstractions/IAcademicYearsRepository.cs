using AWMService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWMService.Application.Abstractions
{
    public interface IAcademicYearsRepository 
    {
        Task<AcademicYears?> GetAcademicYearsByIdAsync(int id, CancellationToken ct);
        Task<AcademicYears?> GetAcademicYearsByDateAsync(DateTime date, CancellationToken ct); 
        Task<IReadOnlyList<AcademicYears>> ListAllAsync(CancellationToken ct);

        Task AddAcademicYearsAsync(string yearName, DateTime startDate, DateTime endDate, int actorUserId, CancellationToken ct);

        Task UpdateAcademicYearsAsync(int id, string yearName, DateTime startDate, DateTime endDate, int actorUserId, CancellationToken ct);
        Task SoftDeleteAcademicYearsAsync(int id, int actorUserId, CancellationToken ct);
    }
}
