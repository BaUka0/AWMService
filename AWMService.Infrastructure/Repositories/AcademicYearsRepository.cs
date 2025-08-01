using AWMService.Application.Interfaces;
using AWMService.Domain.Entities;
using AWMService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AWMService.Infrastructure.Repositories
{
    public class AcademicYearsRepository : GenericRepository<AcademicYears>, IAcademicYearsRepository
    {
        private readonly AppDbContext _context;

        public AcademicYearsRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<AcademicYears?> GetByYearAsync(string year)
        {
            return await _context.AcademicYears
                .FirstOrDefaultAsync(ay => ay.YearName == year);
        }
    }
}