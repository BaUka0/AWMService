// Infrastructure/Repositories/StudentWorkRepository.cs

using AWMService.Application.Interfaces;
using AWMService.Domain.Entities;
using AWMService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AWMService.Infrastructure.Repositories
{
    public class StudentWorkRepository : GenericRepository<StudentWork>, IStudentWorkRepository
    {
        private readonly AppDbContext _context;

        public StudentWorkRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StudentWork>> GetByStudentIdAsync(int studentId)
        {
            return await _context.StudentWorks
                .Where(sw => sw.StudentId == studentId)
                .ToListAsync();
        }

        public async Task<IEnumerable<StudentWork>> GetByAcademicYearIdAsync(int academicYearId)
        {
            return await _context.StudentWorks
                .Where(sw => sw.AcademicYearId == academicYearId)
                .ToListAsync();
        }

        public async Task<StudentWork?> GetWithDetailsAsync(int studentWorkId)
        {
            return await _context.StudentWorks
                .Include(sw => sw.Topic)
                .Include(sw => sw.Student)
                .Include(sw => sw.Status)
                .Include(sw => sw.AcademicYear)
                .Include(sw => sw.WorkType)
                .Include(sw => sw.DefenseSchedules)
                .Include(sw => sw.WorkChecks)
                .FirstOrDefaultAsync(sw => sw.StudentWorkId == studentWorkId);
        }

        public async Task<bool> HasActiveWorkAsync(int studentId, int academicYearId)
        {
            return await _context.StudentWorks.AnyAsync(sw =>
                sw.StudentId == studentId &&
                sw.AcademicYearId == academicYearId &&
                sw.Status.Name != "Отменено"); // Или использовать StatusId при необходимости
        }
    }
}