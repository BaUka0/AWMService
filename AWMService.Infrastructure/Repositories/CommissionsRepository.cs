using AWMService.Application.Interfaces;
using AWMService.Domain.Entities;
using AWMService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AWMService.Infrastructure.Repositories;

public class CommissionsRepository : GenericRepository<Commissions>, ICommissionsRepository
{
    private readonly AppDbContext _context;

    public CommissionsRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Commissions>> GetByDepartmentIdAsync(int departmentId)
    {
        return await _context.Commissions
            .Where(c => c.DepartmentId == departmentId)
            .ToListAsync();
    }

    public async Task<Commissions?> GetWithMembersAsync(int commissionId)
    {
        return await _context.Commissions
            .Include(c => c.CommissionMembers)
            .Include(c => c.DefenseSchedules)
            .FirstOrDefaultAsync(c => c.CommissionId == commissionId);
    }
}
