using AWMService.Application.Interfaces;
using AWMService.Domain.Entities;
using AWMService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AWMService.Infrastructure.Repositories;

public class UserTypeRepository : GenericRepository<UserType>, IUserTypeRepository
{
    private readonly AppDbContext _context;

    public UserTypeRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<UserType?> GetByNameAsync(string name)
    {
        return await _context.UserTypes
            .FirstOrDefaultAsync(ut => ut.Name == name);
    }

    public async Task<UserType?> GetWithUsersAsync(int userTypeId)
    {
        return await _context.UserTypes
            .Include(ut => ut.Users)
            .FirstOrDefaultAsync(ut => ut.UserTypeId == userTypeId);
    }
}