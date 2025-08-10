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
    public class InstitutesRepository: IInstitutesRepository
    {
        private readonly AppDbContext _context;
        public InstitutesRepository(AppDbContext context) => _context = context;


        public async Task<Institutes?> GetInstitutesByIdAsync(int id, CancellationToken ct)
        {
            return await _context.Set<Institutes>()
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == id, ct);
        }


        public async Task<Institutes?> GetInstitutesByNameAsync(string name, CancellationToken ct)
        {
            return await _context.Set<Institutes>()
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Name == name, ct);
        }



        public async Task<IReadOnlyList<Institutes>> ListAllAsync(CancellationToken ct)
        {
            return await _context.Set<Institutes>()
                .AsNoTracking()
                .OrderBy(i => i.Name)
                .ToListAsync(ct);
        }


    }
}
