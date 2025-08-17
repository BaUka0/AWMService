using AWMService.Application.Abstractions.Repositories;
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
    public class PeriodsRepository : IPeriodsRepository
    {
        private readonly AppDbContext _context;

        public PeriodsRepository(AppDbContext context) => _context = context;


        public async Task<Periods?> GetPeriodsByIdAsync(int id, CancellationToken ct)
        {
            return await
           _context.Set<Periods>()
               .AsNoTracking()
               .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
        }

        public async Task<IReadOnlyList<Periods>> ListByAcademicYearAsync(int academicYearId, CancellationToken ct)
        {
            return await _context.Set<Periods>()
                .AsNoTracking()
                .Where(x => !x.IsDeleted && x.AcademicYearId == academicYearId)
                .OrderBy(x => x.StartDate)
                .ThenBy(x => x.EndDate)
                .ToListAsync(ct);
        }
        public async Task<Periods?> GetActivePeriodsAsync(int periodTypeId, DateTime asOfDate, CancellationToken ct)
        {
           return await _context.Set<Periods>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    !x.IsDeleted &&
                    x.PeriodTypeId == periodTypeId &&
                    asOfDate.Date >= x.StartDate && asOfDate.Date <= x.EndDate, ct);
        }

        public async Task AddPeriodsAsync(int periodTypeId, int academicYearId, DateTime startDate, DateTime endDate, int statusId, int actorUserId, CancellationToken ct)
        {
            if (startDate > endDate) throw new ArgumentException("StartDate must be <= EndDate.");

            var entity = new Periods
            {
                PeriodTypeId = periodTypeId,
                AcademicYearId = academicYearId,
                StartDate = startDate.Date,
                EndDate = endDate.Date,
                StatusId = statusId,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = actorUserId,
                ModifiedBy = null,
                ModifiedOn = null,
                IsDeleted = false,
                DeletedOn = null,
                DeletedBy = null
            };

            await _context.Set<Periods>().AddAsync(entity, ct);
        }

        public async Task UpdatePeriodDatesAsync(int id, DateTime startDate, DateTime endDate, int actorUserId, CancellationToken ct)
        {
            if (startDate > endDate) throw new ArgumentException("StartDate must be <= EndDate.");

            var entity = await _context.Set<Periods>()
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct)
                ?? throw new KeyNotFoundException($"Period #{id} not found.");

            entity.StartDate = startDate.Date;
            entity.EndDate = endDate.Date;
            entity.ModifiedBy = actorUserId;
            entity.ModifiedOn = DateTime.UtcNow;
        }

        public async Task ChangePeriodStatusAsync(int id, int statusId, int actorUserId, CancellationToken ct)
        {
            var entity = await _context.Set<Periods>()
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct)
                ?? throw new KeyNotFoundException($"Period #{id} not found.");

            entity.StatusId = statusId;
            entity.ModifiedBy = actorUserId;
            entity.ModifiedOn = DateTime.UtcNow;
        }

        public async Task SoftDeletePeriodsAsync(int id, int actorUserId, CancellationToken ct)
        {
            var entity = await _context.Set<Periods>().FirstOrDefaultAsync(x => x.Id == id, ct);
            if (entity is null || entity.IsDeleted) return;

            entity.IsDeleted = true;
            entity.DeletedOn = DateTime.UtcNow;
            entity.DeletedBy = actorUserId;
        }
    }
}
