using AWMService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWMService.Application.Abstractions
{
    public interface IUsersRepository
    {
        Task<Users?> GetByIdAsync(int id, CancellationToken ct);
        Task<Users?> GetByLoginAsync(string login, CancellationToken ct);
        Task<Users?> GetByEmailAsync(string email, CancellationToken ct);
        Task<Users?> GetByEmailWithRolesAsync(string email, CancellationToken ct);
        Task<IReadOnlyList<Users>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken ct);
        Task AddUserAsync(Users user, CancellationToken ct);
    }
}
