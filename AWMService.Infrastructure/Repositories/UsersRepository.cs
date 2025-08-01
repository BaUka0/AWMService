using AWMService.Application.Interfaces;
using AWMService.Domain.Entities;
using AWMService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AWMService.Infrastructure.Repositories;

public class UsersRepository : GenericRepository<Users>, IUsersRepository
{
    private readonly AppDbContext _context;

    public UsersRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Users?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<Users?> GetByLoginAsync(string login)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Login == login);
    }

    public async Task<Users?> GetByIINAsync(string iin)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.IIN == iin);
    }

    public async Task<Users?> GetWithRolesAsync(int userId)
    {
        return await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Roles)
            .Include(u => u.UserType)
            .Include(u => u.Department)
            .FirstOrDefaultAsync(u => u.UserId == userId);
    }

    public async Task<IEnumerable<Users>> GetByDepartmentIdAsync(int departmentId)
    {
        return await _context.Users
            .Where(u => u.DepartmentId == departmentId)
            .ToListAsync();
    }
}
