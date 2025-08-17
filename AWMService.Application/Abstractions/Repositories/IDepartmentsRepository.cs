using AWMService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWMService.Application.Abstractions.Repositories
{
    public interface IDepartmentsRepository
    {
        Task<Departments?> GetDepartmentsByIdAsync(int id, CancellationToken ct);
        Task<Departments?> GetDepartmentsByNameAsync(string name, CancellationToken ct);
        Task<IReadOnlyList<Departments?>> ListByInstitutesAsync(int instituteId, CancellationToken ct);
    }
}
