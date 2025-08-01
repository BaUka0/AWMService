using AWMService.Application.Interfaces;
using AWMService.Domain.Entities;
using AWMService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AWMService.Infrastructure.Repositories;

public class CommissionMembersRepository : GenericRepository<CommissionMembers>, ICommissionMembersRepository
{
    private readonly AppDbContext _context;

    public CommissionMembersRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CommissionMembers>> GetByCommissionIdAsync(int commissionId)
    {
        return await _context.CommissionMembers
            .Where(cm => cm.CommissionId == commissionId)
            .Include(cm => cm.Member)
            .ToListAsync();
    }

    public async Task<IEnumerable<CommissionMembers>> GetByMemberIdAsync(int memberId)
    {
        return await _context.CommissionMembers
            .Where(cm => cm.MemberId == memberId)
            .Include(cm => cm.Commission)
            .ToListAsync();
    }
}
