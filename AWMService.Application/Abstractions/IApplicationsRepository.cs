using AWMService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWMService.Application.Abstractions
{
    public interface IApplicationsRepository
    {
        Task<Applications?> GetApplicationsByIdAsync(int id, CancellationToken ct);
        Task<IReadOnlyList<Applications>> ListByStudentAsync(int studentId, CancellationToken ct);
        Task<IReadOnlyList<Applications>> ListByTopicAsync(int topicId, CancellationToken ct);
        Task AddApplicationAsync(int studentId, int topicId, int initialStatusId, int actorUserId, CancellationToken ct);
        Task ChangeStatusAsync(int applicationId, int newStatusId, int actorUserId, CancellationToken ct);
    }
}
