using AWMService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWMService.Application.Abstractions.Repositories
{
    public interface ITopicsRepository
    {
        Task<Topics?> GetTopicsByIdAsync(int id, CancellationToken ct);
        Task<IReadOnlyList<Topics>> ListByDirectionAsync(int directionId, CancellationToken ct);
        Task<int> CountAssignedAsync(int topicId, CancellationToken ct);     
        Task<bool> HasFreeSlotAsync(int topicId, CancellationToken ct);
        Task AddTopicAsync(int directionId,
            string? titleKz, string? titleRu, string? titleEn,
            string? description,
            int supervisorId,
            int statusId,
            int maxParticipants,
            int actorUserId,
            CancellationToken ct);

        Task UpdateTopicAsync(int id,int? directionId,
            string? titleKz, string? titleRu, string? titleEn,
            string? description,
            int? supervisorId,
            int? maxParticipants,
            int actorUserId,
            CancellationToken ct);

        Task ChangeStatusAsync(int id, int statusId, int actorUserId, CancellationToken ct);
        Task SoftDeleteTopicAsync(int id, int actorUserId, CancellationToken ct);
    }
}
