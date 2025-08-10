using AWMService.Application.Abstractions;
using AWMService.Domain.Entities;
using AWMService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AWMService.Infrastructure.Repositories
{
    public class CommissionTypesRepository : ICommissionTypesRepository
    {
        private readonly AppDbContext _context;

        public CommissionTypesRepository(AppDbContext context) => _context = context;

        public Task<CommissionTypes?> GetCommissionTypesByIdAsync(int id, CancellationToken ct) =>
            _context.Set<CommissionTypes>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);

        public async Task<IReadOnlyList<CommissionTypes>> GetAllCommissionTypesAsync(CancellationToken ct) =>
            await _context.Set<CommissionTypes>()
                .AsNoTracking()
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.Name)
                .ToListAsync(ct);

        public async Task AddCommissionTypesAsync(string name, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.", nameof(name));

            var entity = new CommissionTypes
            {
                Name = name.Trim(),
                IsDeleted = false,
                DeletedOn = null,
                DeletedBy = null
            };

            await _context.Set<CommissionTypes>().AddAsync(entity, ct);
            
        }

        public async Task DeleteCommissionTypesAsync(int id, int actorUserId, CancellationToken ct)
        {
            var entity = await _context.Set<CommissionTypes>()
                .FirstOrDefaultAsync(x => x.Id == id, ct);

            if (entity is null || entity.IsDeleted) return;

            entity.IsDeleted = true;
            entity.DeletedOn = DateTime.UtcNow;
            entity.DeletedBy = actorUserId;
          
        }
    }
}
