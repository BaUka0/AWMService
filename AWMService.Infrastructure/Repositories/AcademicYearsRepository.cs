using AWMService.Application.Abstractions;
using AWMService.Domain.Entities;
using AWMService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task UpdateAcademicYearsAsync(int id, string yearName, DateTime startDate, DateTime endDate, int actorUserId, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(yearName)) throw new ArgumentException("Year name is required.", nameof(yearName));
            if (startDate > endDate) throw new ArgumentException("StartDate must be <= EndDate.");

            var entity = await _context.Set<AcademicYears>()
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct)
                ?? throw new KeyNotFoundException($"AcademicYear #{id} not found.");

            entity.YearName = yearName.Trim();
            entity.StartDate = startDate.Date;
            entity.EndDate = endDate.Date;
            entity.ModifiedBy = actorUserId;
            entity.ModifiedOn = DateTime.UtcNow;
        }


        public async Task SoftDeleteAcademicYearsAsync(int id, int actorUserId, CancellationToken ct)
        {
            var entity = await _context.Set<AcademicYears>().FirstOrDefaultAsync(x => x.Id == id, ct);
            if (entity is null || entity.IsDeleted) return;

            entity.IsDeleted = true;
            entity.DeletedOn = DateTime.UtcNow;
            entity.DeletedBy = actorUserId;
        }
    }
}
