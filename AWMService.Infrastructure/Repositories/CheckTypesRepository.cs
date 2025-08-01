using AWMService.Application.Interfaces;
using AWMService.Domain.Entities;
using AWMService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AWMService.Infrastructure.Repositories;

public class CheckTypesRepository : GenericRepository<CheckTypes>, ICheckTypesRepository
{
    private readonly AppDbContext _context;

    public CheckTypesRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<CheckTypes?> GetByNameAsync(string name)
    {
        return await _context.CheckTypes
            .FirstOrDefaultAsync(ct => ct.Name == name);
    }
}
