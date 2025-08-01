using AWMService.Application.Interfaces;
using AWMService.Domain.Entities;
using AWMService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AWMService.Infrastructure.Repositories
{
    public class ApplicationsRepository : GenericRepository<Applications>, IApplicationsRepository
    {
        private readonly AppDbContext _context;

        public ApplicationsRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Applications>> GetByStudentIdAsync(int studentId)
        {
            return await _context.Applications
                .Where(app => app.StudentId == studentId)
                .Include(app => app.Topic)
                .Include(app => app.Status)
                .ToListAsync();
        }

        public async Task<IEnumerable<Applications>> GetByStatusIdAsync(int statusId)
        {
            return await _context.Applications
                .Where(app => app.StatusId == statusId)
                .Include(app => app.Student)
                .Include(app => app.Topic)
                .ToListAsync();
        }
    }
}