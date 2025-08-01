using AWMService.Application.Interfaces;
using AWMService.Domain.Entities;
using AWMService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AWMService.Infrastructure.Repositories;

public class EvaluationCriteriaRepository : GenericRepository<EvaluationCriteria>, IEvaluationCriteriaRepository
{
    private readonly AppDbContext _context;

    public EvaluationCriteriaRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<EvaluationCriteria>> GetByCommissionTypeIdAsync(int commissionTypeId)
    {
        return await _context.EvaluationCriteria
            .Where(ec => ec.CommissionTypeId == commissionTypeId)
            .ToListAsync();
    }

    public async Task<EvaluationCriteria?> GetByNameAndTypeAsync(string name, int commissionTypeId)
    {
        return await _context.EvaluationCriteria
            .FirstOrDefaultAsync(ec =>
                ec.Name == name &&
                ec.CommissionTypeId == commissionTypeId);
    }
}
