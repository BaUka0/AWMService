using AWMService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWMService.Application.Abstractions.Repositories
{
    public interface IDirectionsRepository
    {
        Task<Directions?> GetDirectionByIdAsync(int id, CancellationToken ct);
       
        Task<IReadOnlyList<Directions>> ListBySupervisorDepartmentAsync(int departmentId, CancellationToken ct);
        Task AddDirectionAsync(string nameKz, string nameRu,string nameEn, string description, int supervisorId, int statusId, int actorUserId,
            CancellationToken ct);

        Task UpdateDirectionAsync(int id, string nameKz, string nameRu, string nameEn, string description, int actorUserId, CancellationToken ct);
        Task ChangeDirectionStatusAsync(int id, int statusId, int actorUserId, CancellationToken ct);
        Task SoftDeleteDirectionAsync(int id, int actorUserId, CancellationToken ct);

    }
}
