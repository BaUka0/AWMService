using AWMService.Application.Interfaces;
using AWMService.Domain.Entities;
using AWMService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AWMService.Infrastructure.Repositories;

public class UserTypeRepository : GenericRepository<UserTypes>, IUserTypeRepository
{
    private readonly AppDbContext _context;

    public UserTypeRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<UserTypes?> GetByNameAsync(string name)
    {
        return await _context.UserTypes
            .FirstOrDefaultAsync(ut => ut.Name == name);
    }

    public async Task<UserTypes?> GetWithUsersAsync(int userTypeId)
    {
        return await _context.UserTypes
            .Include(ut => ut.Users)
            .FirstOrDefaultAsync(ut => ut.UserTypeId == userTypeId);
    }
}