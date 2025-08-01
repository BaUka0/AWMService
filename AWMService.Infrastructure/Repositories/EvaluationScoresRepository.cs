using AWMService.Application.Interfaces;
using AWMService.Domain.Entities;
using AWMService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AWMService.Infrastructure.Repositories;

public class EvaluationScoresRepository : GenericRepository<EvaluationScores>, IEvaluationScoresRepository
{
    private readonly AppDbContext _context;

    public EvaluationScoresRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<EvaluationScores>> GetByDefenseGradeIdAsync(int defenseGradeId)
    {
        return await _context.EvaluationScores
            .Where(es => es.DefenseGradeId == defenseGradeId)
            .Include(es => es.Criteria)
            .Include(es => es.CommissionMember)
            .ToListAsync();
    }

    public async Task<IEnumerable<EvaluationScores>> GetByCommissionMemberIdAsync(int commissionMemberId)
    {
        return await _context.EvaluationScores
            .Where(es => es.CommissionMemberId == commissionMemberId)
            .Include(es => es.Criteria)
            .ToListAsync();
    }

    public async Task<EvaluationScores?> GetByGradeAndCriteriaAsync(int defenseGradeId, int criteriaId, int memberId)
    {
        return await _context.EvaluationScores
            .FirstOrDefaultAsync(es =>
                es.DefenseGradeId == defenseGradeId &&
                es.CriteriaId == criteriaId &&
                es.CommissionMemberId == memberId);
    }
}