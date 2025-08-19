using AWMService.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWMService.Application.Abstractions.Repositories
{
    public interface IStatusesRepository
    {
        Task<Statuses?> GetStatusesByIdAsync(int id, CancellationToken ct);
        Task<IReadOnlyList<Statuses>> GetAllAsync(CancellationToken ct);
    }
}
