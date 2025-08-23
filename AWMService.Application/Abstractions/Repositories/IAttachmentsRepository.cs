using AWMService.Domain.Entities;


namespace AWMService.Application.Abstractions.Repositories
{
    public interface IAttachmentsRepository
    {
        Task<Attachments?> GetAttachmentByIdAsync(int id, CancellationToken ct);
        Task<(byte[] Data, string FileName, string FileType)?> GetContentAsync(int id, CancellationToken ct);
        Task AddForStudentWorkAsync(int studentWorkId, string fileName, string fileType, byte[] fileData, int actorUserId, CancellationToken ct);
        Task AddForWorkCheckAsync(int workCheckId, string fileName, string fileType, byte[] fileData, int actorUserId, CancellationToken ct);
        
        Task RenameAsync(int id, string newFileName, int actorUserId, CancellationToken ct);
        Task SoftDeleteAsync(int id, int actorUserId, CancellationToken ct);
    }
}
