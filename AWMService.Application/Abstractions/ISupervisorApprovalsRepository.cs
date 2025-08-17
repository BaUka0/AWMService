using AWMService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWMService.Application.Abstractions
{
    public interface ISupervisorApprovalsRepository 
    {
        Task<SupervisorApprovals?> GetApprovalByIdAsync(int id, CancellationToken ct);
        Task<IReadOnlyList<SupervisorApprovals>> ListByDepartmentAndYearAsync(int departmentId, int academicYearId, CancellationToken ct);
        Task<IReadOnlyList<int>> ListApprovedUserIdsByDepartmentAndYearAsync(int departmentId, int academicYearId, CancellationToken ct);
        Task<bool> IsApprovedAsync(int userId, int departmentId, int academicYearId, CancellationToken ct);
        Task ApproveAsync(int userId, int departmentId, int academicYearId, int actorUserId, CancellationToken ct);
        Task RevokeAsync(int userId, int departmentId, int academicYearId, int actorUserId, CancellationToken ct);

    }
}
