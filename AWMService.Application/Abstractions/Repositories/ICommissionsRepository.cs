using AWMService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWMService.Application.Abstractions.Repositories
{
    public interface ICommissionsRepository
    {
        Task<Commissions?> GetCommissionsByIdAsync(int id, CancellationToken ct);
        Task<IReadOnlyList<Commissions>> ListByPeriodAsync(int periodId, CancellationToken ct);
        Task<IReadOnlyList<Commissions>> ListByDepartmentAsync(int departmentId, CancellationToken ct);
        Task AddAsync(string name, int commissionTypeId, int secretaryId, int periodId, int departmentId, int actorUserId, CancellationToken ct);

        Task UpdateAsync(int id, string? name, int? commissionTypeId, int? secretaryId, int? periodId, int? departmentId, int actorUserId, CancellationToken ct);

        Task SoftDeleteAsync(int id, int actorUserId, CancellationToken ct);
    }
}
