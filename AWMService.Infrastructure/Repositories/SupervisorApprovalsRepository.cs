using AWMService.Application.Interfaces;
using AWMService.Domain.Entities;
using AWMService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AWMService.Infrastructure.Repositories;

public class SupervisorApprovalsRepository : GenericRepository<SupervisorApprovals>, ISupervisorApprovalsRepository
{
    private readonly AppDbContext _context;

    public SupervisorApprovalsRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<SupervisorApprovals>> GetByUserIdAsync(int userId)
    {
        return await _context.SupervisorApprovals
            .Where(sa => sa.UserId == userId)
            .Include(sa => sa.Department)
            .Include(sa => sa.AcademicYear)
            .ToListAsync();
    }

    public async Task<SupervisorApprovals?> GetByUserAndYearAsync(int userId, int academicYearId)
    {
        return await _context.SupervisorApprovals
            .Include(sa => sa.Department)
            .Include(sa => sa.ApprovedByUser)
            .FirstOrDefaultAsync(sa => sa.UserId == userId && sa.AcademicYearId == academicYearId);
    }

    public async Task<IEnumerable<SupervisorApprovals>> GetByDepartmentAndYearAsync(int departmentId, int academicYearId)
    {
        return await _context.SupervisorApprovals
            .Where(sa => sa.DepartmentId == departmentId && sa.AcademicYearId == academicYearId)
            .Include(sa => sa.User)
            .Include(sa => sa.ApprovedByUser)
            .ToListAsync();
    }
}
