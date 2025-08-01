using AWMService.Application.Interfaces;
using AWMService.Domain.Entities;
using AWMService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AWMService.Infrastructure.Repositories;

public class DepartmentExpertsRepository : GenericRepository<DepartmentExperts>, IDepartmentExpertsRepository
{
    private readonly AppDbContext _context;

    public DepartmentExpertsRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DepartmentExperts>> GetByDepartmentAsync(int departmentId)
    {
        return await _context.DepartmentExperts
            .Where(de => de.DepartmentId == departmentId)
            .Include(de => de.User)
            .Include(de => de.CheckType)
            .ToListAsync();
    }

    public async Task<IEnumerable<DepartmentExperts>> GetByAcademicYearAsync(int yearId)
    {
        return await _context.DepartmentExperts
            .Where(de => de.AcademicYearId == yearId)
            .ToListAsync();
    }

    public async Task<IEnumerable<DepartmentExperts>> GetByCheckTypeAsync(int checkTypeId)
    {
        return await _context.DepartmentExperts
            .Where(de => de.CheckTypeId == checkTypeId)
            .ToListAsync();
    }

    public async Task<DepartmentExperts?> GetByExpertAndCheckTypeAsync(int userId, int checkTypeId, int yearId)
    {
        return await _context.DepartmentExperts
            .FirstOrDefaultAsync(de =>
                de.UserId == userId &&
                de.CheckTypeId == checkTypeId &&
                de.AcademicYearId == yearId);
    }
}
