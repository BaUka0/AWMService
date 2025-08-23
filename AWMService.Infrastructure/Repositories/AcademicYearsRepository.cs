using AWMService.Application.Abstractions.Repositories;
using AWMService.Domain.Entities;
using AWMService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AWMService.Infrastructure.Repositories
{
    public class AcademicYearsRepository : IAcademicYearsRepository
    {
        private readonly AppDbContext _context;
        public AcademicYearsRepository(AppDbContext context)=> _context = context;

        public async Task<AcademicYears?> GetAcademicYearsByIdAsync(int id, CancellationToken ct)
        {
           return await _context.Set<AcademicYears>()
                .AsNoTracking()
                .FirstOrDefaultAsync(ay => ay.Id == id && !ay.IsDeleted, ct);
        }

        public async Task<AcademicYears?> GetAcademicYearsByDateAsync(DateTime date, CancellationToken ct)
        {
            return await _context.Set<AcademicYears>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => !x.IsDeleted && date >= x.StartDate && date <= x.EndDate, ct);
        }

        public async Task<IReadOnlyList<AcademicYears>> ListAllAsync(CancellationToken ct)
        {
            return await _context.Set<AcademicYears>()
                .AsNoTracking()
                .Where(x => !x.IsDeleted)
                .OrderByDescending(x => x.StartDate)
                .ToListAsync(ct);
        }
        
        public async Task AddAcademicYearsAsync(string yearName, DateTime startDate, DateTime endDate, int actorUserId, CancellationToken ct)
        {
            if(string.IsNullOrWhiteSpace(yearName)) throw new ArgumentException("Year name is required.",nameof(yearName));
            if (startDate > endDate) throw new ArgumentException("StartDate must be <= EndDate.");
            var now = DateTime.UtcNow;

            var entity = new AcademicYears
            {
                YearName = yearName.Trim(),
                StartDate = startDate.Date,
                EndDate = endDate.Date,
                CreatedOn = now,
                CreatedBy = actorUserId,
                ModifiedBy = null,
                ModifiedOn = null,
                IsDeleted = false,
                DeletedOn = null,
                DeletedBy = null
            };

            await _context.Set<AcademicYears>().AddAsync(entity, ct);
        }

        public Task UpdateAsync(AcademicYears entity, CancellationToken ct)
        {
            _context.Set<AcademicYears>().Update(entity);
            return Task.CompletedTask;
        }


        public Task SoftDeleteAsync(AcademicYears entity, CancellationToken ct)
        {
            _context.Set<AcademicYears>().Update(entity);
            return Task.CompletedTask;
        }
    }
}
