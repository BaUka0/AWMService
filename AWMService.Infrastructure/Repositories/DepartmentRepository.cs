using AWMService.Application.Interfaces;
using AWMService.Domain.Entities;
using AWMService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AWMService.Infrastructure.Repositories;

public class DepartmentRepository : GenericRepository<Departments>, IDepartmentRepository
{
    private readonly AppDbContext _context;

    public DepartmentRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Departments>> GetByInstituteIdAsync(int instituteId)
    {
        return await _context.Departments
            .Where(d => d.InstituteId == instituteId)
            .ToListAsync();
    }

    public async Task<Departments?> GetWithUsersAsync(int departmentId)
    {
        return await _context.Departments
            .Include(d => d.Users)
            .FirstOrDefaultAsync(d => d.DepartmentId == departmentId);
    }
}
