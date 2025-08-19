using AWMService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWMService.Application.Abstractions.Repositories
{
    public interface IUserTypesRepository
    {
        Task<UserTypes?> GetUserTypeByIdAsync(int id, CancellationToken ct);
        Task<IReadOnlyList<UserTypes>> GetAllAsync(CancellationToken ct);
    }
}
