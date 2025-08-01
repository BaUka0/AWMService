using AWMService.Domain.Entities;

namespace AWMService.Application.Interfaces;

public interface IStudentWorkRepository : IGenericRepository<StudentWork>
{
    Task<IEnumerable<StudentWork>> GetByStudentIdAsync(int studentId);
    Task<IEnumerable<StudentWork>> GetByAcademicYearIdAsync(int academicYearId);
    Task<StudentWork?> GetWithDetailsAsync(int studentWorkId);
    Task<bool> HasActiveWorkAsync(int studentId, int academicYearId);
}
