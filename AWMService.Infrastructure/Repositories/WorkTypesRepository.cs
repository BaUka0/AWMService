using AWMService.Application.Interfaces;
using AWMService.Domain.Entities;
using AWMService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AWMService.Infrastructure.Repositories;

public class WorkTypesRepository : GenericRepository<WorkTypes>, IWorkTypesRepository
{
    private readonly AppDbContext _context;

    public WorkTypesRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<WorkTypes?> GetByNameAsync(string name)
    {
        return await _context.WorkTypes
            .FirstOrDefaultAsync(wt => wt.Name == name);
    }

    public async Task<WorkTypes?> GetWithStudentWorksAsync(int workTypeId)
    {
        return await _context.WorkTypes
            .Include(wt => wt.StudentWorks)
            .FirstOrDefaultAsync(wt => wt.WorkTypeId == workTypeId);
    }
}
