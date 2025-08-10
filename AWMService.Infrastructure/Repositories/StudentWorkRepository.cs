using AWMService.Application.Abstractions;
using AWMService.Domain.Entities;
using AWMService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWMService.Infrastructure.Repositories
{
    public class StudentWorkRepository : IStudentWorkRepository
    {
        private readonly AppDbContext _context;
        public StudentWorkRepository(AppDbContext context) => _context = context;

        public async Task<StudentWork?> GetStudentWorkByIdAsync(int id, CancellationToken ct)
        {
            return await _context.Set<StudentWork>()
                .AsNoTracking()
                .FirstOrDefaultAsync(w => w.Id == id && !w.IsDeleted, ct);
        }

        public async Task<StudentWork?> GetByStudentAndYearAsync(int studentId, int academicYearId, CancellationToken ct)
        {
            return await _context.Set<StudentWork>()
                .AsNoTracking()
                .FirstOrDefaultAsync(w => !w.IsDeleted && w.StudentId == studentId && w.AcademicYearId == academicYearId, ct);
        }

        public async Task<IReadOnlyList<StudentWork>> ListByStudentAsync(int studentId, CancellationToken ct)
        { 
        return await _context.Set<StudentWork>()
                        .AsNoTracking()
                        .Where(w => !w.IsDeleted && w.StudentId == studentId)
                        .OrderByDescending(w => w.CreatedOn)
                        .ToListAsync(ct);
        }

        public async Task<IReadOnlyList<StudentWork>> ListByTopicAsync(int topicId, CancellationToken ct) 
        {
            return await _context.Set<StudentWork>()
                .AsNoTracking()
                .Where(w => !w.IsDeleted && w.TopicId == topicId)
                .OrderBy(w => w.Id)
                .ToListAsync(ct);
         }


        public async Task<IReadOnlyList<StudentWork>> ListByYearAsync(int academicYearId, CancellationToken ct)
        {
           return  await _context.Set<StudentWork>()
                .AsNoTracking()
                .Where(w => !w.IsDeleted && w.AcademicYearId == academicYearId)
                .OrderBy(w => w.StudentId)
                .ToListAsync(ct);
        }


        public async Task AddStudentWorkAsync(int studentId, int topicId, int academicYearId, int workTypeId, int initialStatusId, int actorUserId, CancellationToken ct)
        {
            var now = DateTime.UtcNow;

            var entity = new StudentWork
            {
                StudentId = studentId,
                TopicId = topicId,
                AcademicYearId = academicYearId,
                WorkTypeId = workTypeId,
                StatusId = initialStatusId,
                FinalGrade = null,
                CreatedOn = now,
                CreatedBy = actorUserId,
                ModifiedBy = null,
                ModifiedOn = null,
                IsDeleted = false,
                DeletedOn = null,
                DeletedBy = null
            };

            await _context.Set<StudentWork>().AddAsync(entity, ct);

        }


        public async Task ChangeStatusAsync(int id, int statusId, int actorUserId, CancellationToken ct)
        {
            var entity = await _context.Set<StudentWork>()
                .FirstOrDefaultAsync(w => w.Id == id && !w.IsDeleted, ct)
                ?? throw new KeyNotFoundException($"StudentWork #{id} not found.");

            entity.StatusId = statusId;
            entity.ModifiedBy = actorUserId;
            entity.ModifiedOn = DateTime.UtcNow;
        }


        public async Task SetFinalGradeAsync(int id, string? finalGrade, int actorUserId, CancellationToken ct)
        {
            var entity = await _context.Set<StudentWork>()
                .FirstOrDefaultAsync(w => w.Id == id && !w.IsDeleted, ct)
                ?? throw new KeyNotFoundException($"StudentWork #{id} not found.");

            entity.FinalGrade = finalGrade.Trim();
            entity.ModifiedBy = actorUserId;
            entity.ModifiedOn = DateTime.UtcNow;
        }

        public async Task SoftDeleteAsync(int id, int actorUserId, CancellationToken ct)
        {
            var entity = await _context.Set<StudentWork>()
                .FirstOrDefaultAsync(w => w.Id == id, ct);

            if (entity is null || entity.IsDeleted) return;

            entity.IsDeleted = true;
            entity.DeletedOn = DateTime.UtcNow;
            entity.DeletedBy = actorUserId;
        }
    }
}
