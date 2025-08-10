using AWMService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWMService.Application.Abstractions
{
    public interface IWorkChecksRepository
    {
        Task<WorkChecks?> GetWorkCheckByIdAsync(int id, CancellationToken ct);
        Task<IReadOnlyList<WorkChecks>> ListByStudentWorkAsync(int studentWorkId, CancellationToken ct);
        Task<IReadOnlyList<WorkChecks>> ListByExpertAsync(int expertUserId, int? statusId, CancellationToken ct);
        Task<IReadOnlyList<WorkChecks>> ListByReviewerAsync(int externalContactId, int? statusId, CancellationToken ct);
        Task AddWorkCheckAsync(int studentWorkId, int checkTypeId, int? expertUserId, int? reviewerId, int statusId, string? comment, int actorUserId, CancellationToken ct);
       
        Task SubmitAsync(int id, string? comment, int actorUserId, CancellationToken ct);

        Task SetResultAsync(int id, int statusId, string? resultData, string? comment, int actorUserId, CancellationToken ct);

        Task ChangeStatusAsync(int id, int statusId, int actorUserId, CancellationToken ct);
        Task UpdateCommentAsync(int id, string? comment, int actorUserId, CancellationToken ct);
        Task UpdateResultDataAsync(int id, string? resultData, int actorUserId, CancellationToken ct);
    }
}
