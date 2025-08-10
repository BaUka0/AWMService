using AWMService.Application.Abstractions;
using AWMService.Domain.Entities;
using AWMService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWMService.Infrastructure.Repositories
{
    public class UserTypesRepository : IUserTypesRepository
    {


        private readonly AppDbContext _context;
        public UserTypesRepository(AppDbContext context) => _context = context;
        public async Task<IReadOnlyList<UserTypes?>> GetAllAsync(CancellationToken ct)
        {
            return await _context.Set<UserTypes>()
                .AsNoTracking()
                .OrderBy(u => u.Name)
                .ToListAsync(ct);
        }

        public async Task<UserTypes?> GetUserTypeByIdAsync(int id, CancellationToken ct)
        {
            return await _context.Set<UserTypes>()
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id, ct);
        }
    }
}
