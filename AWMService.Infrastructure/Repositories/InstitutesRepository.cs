using AWMService.Application.Interfaces;
using AWMService.Domain.Entities;
using AWMService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AWMService.Infrastructure.Repositories;

public class InstitutesRepository : GenericRepository<Institutes>, IInstitutesRepository
{
    private readonly AppDbContext _context;

    public InstitutesRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Institutes?> GetByNameAsync(string name)
    {
        return await _context.Institutes
            .FirstOrDefaultAsync(i => i.Name == name);
    }

    public async Task<Institutes?> GetWithDepartmentsAsync(int instituteId)
    {
        return await _context.Institutes
            .Include(i => i.Departments)
            .FirstOrDefaultAsync(i => i.InstituteId == instituteId);
    }
}
