using AWMService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWMService.Application.Abstractions.Repositories
{
    public interface IDefenseGradesRepository
    {
        Task<DefenseGrades?> GetDefenseGradeByIdAsync(int id, CancellationToken ct);
        Task<DefenseGrades?> GetByScheduleAsync(int defenseScheduleId, CancellationToken ct);
        Task UpsertByScheduleAsync(int defenseScheduleId, double? finalScore, string? finalGrade, int statusId, int actorUserId, CancellationToken ct);

        Task UpdateAsync(int id, double? finalScore, string? finalGrade, int? statusId, int actorUserId, CancellationToken ct);

        Task ChangeStatusAsync(int id, int statusId, int actorUserId, CancellationToken ct);
    }
}
