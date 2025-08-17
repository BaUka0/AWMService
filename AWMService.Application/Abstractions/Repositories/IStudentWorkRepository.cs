using AWMService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWMService.Application.Abstractions.Repositories
{
    public interface IStudentWorkRepository
    {
        Task<StudentWork?> GetStudentWorkByIdAsync (int id, CancellationToken ct);
        Task<StudentWork?> GetByStudentAndYearAsync(int studentId, int academicYearId, CancellationToken ct);
        Task<IReadOnlyList<StudentWork>> ListByStudentAsync(int studentId, CancellationToken ct);
        Task<IReadOnlyList<StudentWork>> ListByTopicAsync(int topicId, CancellationToken ct);
        Task<IReadOnlyList<StudentWork>> ListByYearAsync(int academicYearId, CancellationToken ct);
        Task ChangeStatusAsync(int id, int statusId, int actorUserId, CancellationToken ct);
        Task AddStudentWorkAsync(int studentId, int topicId, int academicYearId, int workTypeId, int initialStatusId, int actorUserId, CancellationToken ct);
        Task SetFinalGradeAsync(int id, string? finalGrade, int actorUserId, CancellationToken ct);
        Task SoftDeleteAsync(int id, int actorUserId, CancellationToken ct);

        
    }
}
