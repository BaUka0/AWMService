using AWMService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWMService.Application.Abstractions
{
    public interface IDepartmentExpertsRepository
    {
        Task<DepartmentExperts?> GetDepartmentExpertsByIdAsync(int id, CancellationToken ct);
        Task<IReadOnlyList<DepartmentExperts>> ListByDepartmentAndYearAsync(int departmentId, int academicYearId, int? checkTypeId, CancellationToken ct);
        Task<IReadOnlyList<DepartmentExperts>> ListByUserAndYearAsync(int userId, int academicYearId, CancellationToken ct);
        Task<bool> IsAssignedAsync(int userId, int departmentId, int checkTypeId, int academicYearId, CancellationToken ct);

        Task AssignAsync(int userId, int departmentId, int checkTypeId, int academicYearId, int actorUserId, CancellationToken ct);
        Task RevokeAsync(int userId, int departmentId, int checkTypeId, int academicYearId, int actorUserId, CancellationToken ct);
        Task UpdateAssignmentAsync(int id, int? departmentId, int? checkTypeId, int? academicYearId, int actorUserId, CancellationToken ct);
    }
}
