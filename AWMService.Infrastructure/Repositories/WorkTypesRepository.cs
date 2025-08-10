using AWMService.Application.Abstractions;
using AWMService.Domain.Entities;
using AWMService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AWMService.Infrastructure.Repositories
{
    public class WorkTypesRepository : IWorkTypesRepository
    {
        private readonly AppDbContext _context;
        public WorkTypesRepository(AppDbContext context) => _context = context;

        public Task<WorkTypes?> GetWorkTypeByIdAsync(int id, CancellationToken ct) =>
            _context.Set<WorkTypes>()
                .AsNoTracking()
                .FirstOrDefaultAsync(wt => wt.Id == id && !wt.IsDeleted, ct);

        public async Task<IReadOnlyList<WorkTypes>> GetAllWorkTypesAsync(CancellationToken ct) =>
            await _context.Set<WorkTypes>()
                .AsNoTracking()
                .Where(wt => !wt.IsDeleted)
                .OrderBy(wt => wt.Name) 
                .ToListAsync(ct);

        public async Task DeleteWorkTypeAsync(int id, int actorUserId, CancellationToken ct)
        {
            var workType = await _context.Set<WorkTypes>()
                .FirstOrDefaultAsync(wt => wt.Id == id, ct);

            if (workType is null || workType.IsDeleted) return;

            workType.IsDeleted = true;
            workType.DeletedOn = DateTime.UtcNow;
            workType.DeletedBy = actorUserId;

          
        }
    }
}
