using AWMService.Domain.Entities;
namespace AWMService.Application.Interfaces;

public interface IAttachmentsRepository : IGenericRepository<Attachments>
{
    Task<IEnumerable<Attachments>> GetByEntityAsync(string entityType, int entityId);
    Task<IEnumerable<Attachments>> GetByUploaderAsync(int userId);
}