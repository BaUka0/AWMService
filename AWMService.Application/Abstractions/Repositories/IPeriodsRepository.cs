using AWMService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWMService.Application.Abstractions.Repositories
{
    public interface IPeriodsRepository
    {
        Task<Periods?> GetPeriodsByIdAsync(int id, CancellationToken ct);
        Task<IReadOnlyList<Periods>> ListByAcademicYearAsync(int academicYearId, CancellationToken ct);
        Task<Periods?> GetActivePeriodsAsync(int periodTypeId, DateTime asOfDate, CancellationToken ct);


         Task AddPeriodsAsync(int periodTypeId, int academicYearId, DateTime startDate, DateTime endDate, int statusId, int actorUserId, CancellationToken ct);
        Task UpdatePeriodDatesAsync(int id, DateTime startDate, DateTime endDate, int actorUserId, CancellationToken ct);
        Task ChangePeriodStatusAsync(int id, int statusId, int actorUserId, CancellationToken ct);
        Task SoftDeletePeriodsAsync(int id, int actorUserId, CancellationToken ct);
    }
}
