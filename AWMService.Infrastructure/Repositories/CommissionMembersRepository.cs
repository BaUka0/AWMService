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
    public class CommissionMembersRepository : ICommissionMembersRepository
    {
        private readonly AppDbContext _context;
        public CommissionMembersRepository(AppDbContext context) => _context = context;

        public async Task<CommissionMembers?> GetMemberByIdAsync(int id, CancellationToken ct) 
        { 
           return await _context.Set<CommissionMembers>()
               .AsNoTracking()
               .FirstOrDefaultAsync(x => x.Id == id, ct);
        }
        public async Task<IReadOnlyList<CommissionMembers>> ListActiveByCommissionAsync(int commissionId, CancellationToken ct)
        {
           return await _context.Set<CommissionMembers>()
                .AsNoTracking()
                .Where(x => x.CommissionId == commissionId && x.RemovedOn == null)
                .OrderBy(x => x.RoleInCommission)
                .ThenBy(x => x.Id)
                .ToListAsync(ct);
        }

        public async Task<IReadOnlyList<CommissionMembers>> ListAllByCommissionAsync(int commissionId, CancellationToken ct)
        {
           return await _context.Set<CommissionMembers>()
                .AsNoTracking()
                .Where(x => x.CommissionId == commissionId)
                .OrderByDescending(x => x.AssignedOn)
                .ThenBy(x => x.Id)
                .ToListAsync(ct);
        }

        public Task<bool> IsUserActiveAsync(int commissionId, int userId, CancellationToken ct) =>
            _context.Set<CommissionMembers>()
                .AsNoTracking()
                .AnyAsync(x => x.CommissionId == commissionId
                               && x.MemberId == userId
                               && x.ExternalContactId == null
                               && x.RemovedOn == null, ct);

        public Task<bool> IsExternalActiveAsync(int commissionId, int externalContactId, CancellationToken ct) =>
            _context.Set<CommissionMembers>()
                .AsNoTracking()
                .AnyAsync(x => x.CommissionId == commissionId
                               && x.ExternalContactId == externalContactId
                               && x.MemberId == null
                               && x.RemovedOn == null, ct);

        public async Task AssignInternalAsync(int commissionId, int memberUserId, string roleInCommission, int actorUserId, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(roleInCommission))
                throw new ArgumentException("Role is required.", nameof(roleInCommission));

            var link = await _context.Set<CommissionMembers>()
                .FirstOrDefaultAsync(x =>
                    x.CommissionId == commissionId &&
                    x.MemberId == memberUserId &&
                    x.ExternalContactId == null, ct);

            var now = DateTime.UtcNow;

            if (link is null)
            {
                await _context.Set<CommissionMembers>().AddAsync(new CommissionMembers
                {
                    CommissionId = commissionId,
                    MemberId = memberUserId,
                    ExternalContactId = null,
                    RoleInCommission = roleInCommission.Trim(),
                    AssignedOn = now,
                    AssignedBy = actorUserId,
                    RemovedOn = null,
                    RemovedBy = null
                }, ct);
                return;
            }

            if (link.RemovedOn is not null)
            {
                link.RemovedOn = null;
                link.RemovedBy = null;
                link.RoleInCommission = roleInCommission.Trim();
            }
            else
            {
                if (!string.Equals(link.RoleInCommission, roleInCommission.Trim(), StringComparison.Ordinal))
                    link.RoleInCommission = roleInCommission.Trim();
            }
        }

        public async Task AssignExternalAsync(int commissionId, int externalContactId, string roleInCommission, int actorUserId, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(roleInCommission))
                throw new ArgumentException("Role is required.", nameof(roleInCommission));

            var link = await _context.Set<CommissionMembers>()
                .FirstOrDefaultAsync(x =>
                    x.CommissionId == commissionId &&
                    x.ExternalContactId == externalContactId &&
                    x.MemberId == null, ct);

            var now = DateTime.UtcNow;

            if (link is null)
            {
                await _context.Set<CommissionMembers>().AddAsync(new CommissionMembers
                {
                    CommissionId = commissionId,
                    MemberId = null,
                    ExternalContactId = externalContactId,
                    RoleInCommission = roleInCommission.Trim(),
                    AssignedOn = now,
                    AssignedBy = actorUserId,
                    RemovedOn = null,
                    RemovedBy = null
                }, ct);
                return;
            }

            if (link.RemovedOn is not null)
            {
                link.RemovedOn = null;
                link.RemovedBy = null;
                link.RoleInCommission = roleInCommission.Trim();
            }
            else
            {
                if (!string.Equals(link.RoleInCommission, roleInCommission.Trim(), StringComparison.Ordinal))
                    link.RoleInCommission = roleInCommission.Trim();
            }
        }

        public async Task RemoveAsync(int id, int actorUserId, CancellationToken ct)
        {
            var entity = await _context.Set<CommissionMembers>()
                .FirstOrDefaultAsync(x => x.Id == id, ct)
                ?? throw new KeyNotFoundException($"CommissionMember #{id} not found.");

            if (entity.RemovedOn is null)
            {
                var now = DateTime.UtcNow;
                entity.RemovedOn = now;
                entity.RemovedBy = actorUserId;
            }
        }


        public async Task RemoveUserAsync(int commissionId, int memberUserId, int actorUserId, CancellationToken ct)
        {
            var entity = await _context.Set<CommissionMembers>()
                .FirstOrDefaultAsync(x =>
                    x.CommissionId == commissionId &&
                    x.MemberId == memberUserId &&
                    x.ExternalContactId == null, ct);

            if (entity is null || entity.RemovedOn is not null) return;

            var now = DateTime.UtcNow;
            entity.RemovedOn = now;
            entity.RemovedBy = actorUserId;
        }


        public async Task RemoveExternalAsync(int commissionId, int externalContactId, int actorUserId, CancellationToken ct)
        {
            var entity = await _context.Set<CommissionMembers>()
                .FirstOrDefaultAsync(x =>
                    x.CommissionId == commissionId &&
                    x.ExternalContactId == externalContactId &&
                    x.MemberId == null, ct);

            if (entity is null || entity.RemovedOn is not null) return;

            var now = DateTime.UtcNow;
            entity.RemovedOn = now;
            entity.RemovedBy = actorUserId;
        }
    }
}
