using AWMService.Application.Interfaces;
using AWMService.Domain.Entities;
using AWMService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AWMService.Infrastructure.Repositories;

public class CommissionTypesRepository : GenericRepository<CommissionTypes>, ICommissionTypesRepository
{
    private readonly AppDbContext _context;

    public CommissionTypesRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<CommissionTypes?> GetByNameAsync(string name)
    {
        return await _context.CommissionTypes
            .FirstOrDefaultAsync(ct => ct.Name == name);
    }
}
