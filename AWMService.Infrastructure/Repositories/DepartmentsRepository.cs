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
    public class DepartmentsRepository : IDepartmentsRepository
    {
        private readonly AppDbContext _context;

        public DepartmentsRepository(AppDbContext context) => _context = context;
        public async Task<Departments?> GetDepartmentsByIdAsync(int id, CancellationToken ct)
        {
            return await _context.Set<Departments>()
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Id == id, ct);
        }

        public async Task<Departments?> GetDepartmentsByNameAsync(string name, CancellationToken ct)
        {
            return await _context.Set<Departments>()
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.Name == name, ct);
        }


        public async Task<IReadOnlyList<Departments?>> ListByInstitutesAsync(int instituteId, CancellationToken ct)
        {
            return await _context.Set<Departments>()
                .AsNoTracking()
                .Where(d => d.InstituteId == instituteId)
                .OrderBy(d => d.Name)
                .ToListAsync(ct);
        }
    }
}
