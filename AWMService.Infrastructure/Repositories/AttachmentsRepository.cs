using AWMService.Application.Interfaces;
using AWMService.Domain.Entities;
using AWMService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AWMService.Infrastructure.Repositories;

public class AttachmentsRepository : GenericRepository<Attachments>, IAttachmentsRepository
{
    private readonly AppDbContext _context;

    public AttachmentsRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Attachments>> GetByEntityAsync(string entityType, int entityId)
    {
        return await _context.Attachments
            .Where(a => a.EntityType == entityType && a.AssociatedEntityId == entityId)
            .Include(a => a.UploadedBy)
            .ToListAsync();
    }

    public async Task<IEnumerable<Attachments>> GetByUploaderAsync(int userId)
    {
        return await _context.Attachments
            .Where(a => a.UploadedById == userId)
            .ToListAsync();
    }
}
