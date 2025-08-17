using AWMService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWMService.Application.Abstractions.Repositories
{
    public interface IInstitutesRepository
    {
        Task<Institutes?> GetInstitutesByIdAsync(int id, CancellationToken ct);
        Task<Institutes?> GetInstitutesByNameAsync(string name, CancellationToken ct);
        Task<IReadOnlyList<Institutes>> ListAllAsync(CancellationToken ct);

    }
}
