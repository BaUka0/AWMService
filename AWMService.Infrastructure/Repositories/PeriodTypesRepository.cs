using AWMService.Application.Interfaces;
using AWMService.Domain.Entities;
using AWMService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AWMService.Infrastructure.Repositories;

public class PeriodTypesRepository : GenericRepository<PeriodTypes>, IPeriodTypesRepository
{
    private readonly AppDbContext _context;

    public PeriodTypesRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<PeriodTypes?> GetByNameAsync(string name)
    {
        return await _context.PeriodTypes
            .FirstOrDefaultAsync(pt => pt.Name == name);
    }
}
