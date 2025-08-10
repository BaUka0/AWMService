using AWMService.Application.Abstractions;
using AWMService.Domain.Entities;
using AWMService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AWMService.Application.Abstractions.IDefenseSchedulesRepository;

namespace AWMService.Infrastructure.Repositories
{
    public class DefenseSchedulesRepository : IDefenseSchedulesRepository
    {
        private readonly AppDbContext _context;
        public DefenseSchedulesRepository(AppDbContext context) => _context = context;

        public async Task<DefenseSchedules?> GetDefenseScheduleByIdAsync(int id, CancellationToken ct) 
        { 
            return await _context.Set<DefenseSchedules>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct);
        }
        public async Task<IReadOnlyList<DefenseSchedules>> ListByCommissionAsync(int commissionId, CancellationToken ct) 
        { 
           return await _context.Set<DefenseSchedules>()
                .AsNoTracking()
                .Where(x => !x.IsDeleted && x.CommissionId == commissionId)
                .OrderBy(x => x.DefenseDate)
                .ThenBy(x => x.Id)
                .ToListAsync(ct);
        }

        public async Task<IReadOnlyList<DefenseSchedules>> ListByDateAsync(DateTime date, CancellationToken ct)
        {
            var day = date.Date;
            var next = day.AddDays(1);
            return await _context.Set<DefenseSchedules>()
                .AsNoTracking()
                .Where(x => !x.IsDeleted && x.DefenseDate >= day && x.DefenseDate < next)
                .OrderBy(x => x.DefenseDate)
                .ThenBy(x => x.Id)
                .ToListAsync(ct);
        }

        public async Task AddSlotAsync(int commissionId, int studentWorkId, DateTime defenseDate, string location, int actorUserId, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(location))
                throw new ArgumentException("Location is required.", nameof(location));

            var exists = await _context.Set<DefenseSchedules>()
                .AsNoTracking()
                .AnyAsync(x => !x.IsDeleted && x.StudentWorkId == studentWorkId, ct);
            if (exists)
                throw new InvalidOperationException("This student work is already scheduled.");

            var entity = new DefenseSchedules
            {
                CommissionId = commissionId,
                StudentWorkId = studentWorkId,
                DefenseDate = defenseDate,
                Location = location.Trim(),
                CreatedOn = DateTime.UtcNow,
                CreatedBy = actorUserId,
                ModifiedBy = null,
                ModifiedOn = null,
                IsDeleted = false,
                DeletedOn = null,
                DeletedBy = null
            };

            await _context.Set<DefenseSchedules>().AddAsync(entity, ct);
        }

        public async Task AddSlotsAsync(int commissionId, IEnumerable<DefenseSlotCreate> slots, int actorUserId, CancellationToken ct)
        {
            var list = slots?.ToList() ?? new List<DefenseSlotCreate>();
            if (list.Count == 0) return;

            var workIds = list.Select(s => s.StudentWorkId).Distinct().ToList();
            var already = await _context.Set<DefenseSchedules>()
                .AsNoTracking()
                .Where(x => !x.IsDeleted && workIds.Contains(x.StudentWorkId))
                .Select(x => x.StudentWorkId)
                .ToListAsync(ct);
            if (already.Count > 0)
                throw new InvalidOperationException($"Some student works are already scheduled: {string.Join(", ", already)}");

            var now = DateTime.UtcNow;
            var entities = list.Select(s => new DefenseSchedules
            {
                CommissionId = commissionId,
                StudentWorkId = s.StudentWorkId,
                DefenseDate = s.DefenseDate,
                Location = string.IsNullOrWhiteSpace(s.Location) ? "" : s.Location.Trim(),
                CreatedOn = now,
                CreatedBy = actorUserId,
                ModifiedBy = null,
                ModifiedOn = null,
                IsDeleted = false,
                DeletedOn = null,
                DeletedBy = null
            });

            await _context.Set<DefenseSchedules>().AddRangeAsync(entities, ct);
        }

        public async Task RescheduleAsync(int id, DateTime newDate, string? newLocation, int actorUserId, CancellationToken ct)
        {
            var entity = await _context.Set<DefenseSchedules>()
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct)
                ?? throw new KeyNotFoundException($"DefenseSchedule #{id} not found.");

            entity.DefenseDate = newDate;
            if (newLocation is not null)
                entity.Location = string.IsNullOrWhiteSpace(newLocation) ? entity.Location : newLocation.Trim();

            entity.ModifiedBy = actorUserId;
            entity.ModifiedOn = DateTime.UtcNow;
        }


        public async Task CancelSlotAsync(int id, int actorUserId, CancellationToken ct)
        {
            var entity = await _context.Set<DefenseSchedules>()
                .FirstOrDefaultAsync(x => x.Id == id, ct)
                ?? throw new KeyNotFoundException($"DefenseSchedule #{id} not found.");

            if (entity.IsDeleted) return;

            entity.IsDeleted = true;
            entity.DeletedOn = DateTime.UtcNow;
            entity.DeletedBy = actorUserId;
        }


    }
}
