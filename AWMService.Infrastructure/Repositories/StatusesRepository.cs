using AWMService.Application.Abstractions.Repositories;
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
    public class StatusesRepository : IStatusesRepository
    {
        private readonly AppDbContext _context;

        public StatusesRepository(AppDbContext context) => _context = context;


        public async Task<Statuses?> GetStatusesByIdAsync(int id, CancellationToken ct)
        {
            return await _context.Set<Statuses>()
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id, ct);
        }

        public async Task<IReadOnlyList<Statuses>> GetAllAsync(CancellationToken ct)
        {
            return await _context.Set<Statuses>()
                .AsNoTracking()
                .ToListAsync(ct);
        }
    }
}
