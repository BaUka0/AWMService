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
    public class WorkChecksRepository : IWorkChecksRepository
    {
        private readonly AppDbContext _context;
        public WorkChecksRepository(AppDbContext context) => _context = context;



        public async Task<WorkChecks?> GetWorkCheckByIdAsync(int id, CancellationToken ct)
        {
            return await _context.Set<WorkChecks>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public async Task<IReadOnlyList<WorkChecks>> ListByStudentWorkAsync(int studentWorkId, CancellationToken ct)
        {
            return await _context.Set<WorkChecks>()
                .AsNoTracking()
                .Where(x => x.StudentWorkId == studentWorkId)
                .OrderByDescending(x => x.SubmittedOn)
                .ThenBy(x => x.Id)
                .ToListAsync(ct);
        }

        public async Task<IReadOnlyList<WorkChecks>> ListByExpertAsync(int expertUserId, int? statusId, CancellationToken ct)
        {
            var q = _context.Set<WorkChecks>()
                .AsNoTracking()
                .Where(x => x.ExpertId == expertUserId);

            if (statusId is not null)
                q = q.Where(x => x.StatusId == statusId);

            return await q
                .OrderByDescending(x => x.SubmittedOn)
                .ThenBy(x => x.Id)
                .ToListAsync(ct);
        }

        public async Task<IReadOnlyList<WorkChecks>> ListByReviewerAsync(int externalContactId, int? statusId, CancellationToken ct)
        {
            var q = _context.Set<WorkChecks>()
                .AsNoTracking()
                .Where(x => x.ReviewerId == externalContactId);

            if (statusId is not null)
                q = q.Where(x => x.StatusId == statusId);

            return await q
                .OrderByDescending(x => x.SubmittedOn)
                .ThenBy(x => x.Id)
                .ToListAsync(ct);
        }


        public async Task AddWorkCheckAsync(int studentWorkId, int checkTypeId, int? expertUserId, int? reviewerId, int statusId, string? comment, int actorUserId, CancellationToken ct)
        {
            var now = DateTime.UtcNow;
            var entity = new WorkChecks
            {
                StudentWorkId = studentWorkId,
                CheckTypeId = checkTypeId,
                ExpertId = expertUserId,
                ReviewerId = reviewerId,
                StatusId = statusId,
                Comment = comment.Trim(),
                ResultData = null,
                SubmittedOn = null,
                CheckedOn = null,
                CreatedBy = actorUserId,
                CreatedOn = now,
                ModifiedBy = null,
                ModifiedOn = null
            };

            await _context.Set<WorkChecks>().AddAsync(entity, ct);
        }

        public async Task SubmitAsync(int id, string? comment, int actorUserId, CancellationToken ct)
        {
            var entity = await _context.Set<WorkChecks>()
                .FirstOrDefaultAsync(x => x.Id == id, ct)
                ?? throw new KeyNotFoundException($"WorkCheck #{id} not found.");
            entity.SubmittedOn = entity.SubmittedOn ?? DateTime.UtcNow;

            if (comment is not null)
                entity.Comment = string.IsNullOrWhiteSpace(comment) ? null : comment.Trim();

            entity.ModifiedBy = actorUserId;
            entity.ModifiedOn = DateTime.UtcNow;
        }


        public async Task SetResultAsync(int id, int statusId, string? resultData, string? comment, int actorUserId, CancellationToken ct)
        {
            var entity = await _context.Set<WorkChecks>()
                .FirstOrDefaultAsync(x => x.Id == id, ct)
                ?? throw new KeyNotFoundException($"WorkCheck #{id} not found.");

            entity.StatusId = statusId;
            entity.ResultData = resultData is null ? null : (string.IsNullOrWhiteSpace(resultData) ? null : resultData);
            if (comment is not null)
                entity.Comment = string.IsNullOrWhiteSpace(comment) ? null : comment.Trim();

            entity.CheckedOn = DateTime.UtcNow;
            entity.ModifiedBy = actorUserId;
            entity.ModifiedOn = entity.CheckedOn;
        }

        public async Task ChangeStatusAsync(int id, int statusId, int actorUserId, CancellationToken ct)
        {
            var entity = await _context.Set<WorkChecks>()
                .FirstOrDefaultAsync(x => x.Id == id, ct)
                ?? throw new KeyNotFoundException($"WorkCheck #{id} not found.");

            entity.StatusId = statusId;
            entity.ModifiedBy = actorUserId;
            entity.ModifiedOn = DateTime.UtcNow;
        }

        public async Task UpdateCommentAsync(int id, string? comment, int actorUserId, CancellationToken ct)
        {
            var entity = await _context.Set<WorkChecks>()
                .FirstOrDefaultAsync(x => x.Id == id, ct)
                ?? throw new KeyNotFoundException($"WorkCheck #{id} not found.");

            entity.Comment = comment is null ? entity.Comment : (string.IsNullOrWhiteSpace(comment) ? null : comment.Trim());
            entity.ModifiedBy = actorUserId;
            entity.ModifiedOn = DateTime.UtcNow;
        }

        public async Task UpdateResultDataAsync(int id, string? resultData, int actorUserId, CancellationToken ct)
        {
            var entity = await _context.Set<WorkChecks>()
                .FirstOrDefaultAsync(x => x.Id == id, ct)
                ?? throw new KeyNotFoundException($"WorkCheck #{id} not found.");

            entity.ResultData = resultData is null ? entity.ResultData : (string.IsNullOrWhiteSpace(resultData) ? null : resultData);
            entity.ModifiedBy = actorUserId;
            entity.ModifiedOn = DateTime.UtcNow;
        }
    }
}
