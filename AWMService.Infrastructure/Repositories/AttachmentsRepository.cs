using AWMService.Application.Abstractions.Repositories;
using AWMService.Domain.Entities;
using AWMService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
namespace AWMService.Infrastructure.Repositories
{
    public class AttachmentsRepository : IAttachmentsRepository
    {
        private readonly AppDbContext _context;

        public AttachmentsRepository(AppDbContext context) => _context = context;


        public async Task<Attachments?> GetAttachmentByIdAsync(int id, CancellationToken ct)
        {
            return await _context.Set<Attachments>()
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted, ct);
        }

        public async Task<(byte[] Data, string FileName, string FileType)?> GetContentAsync(int id, CancellationToken ct)
        {
            var attachment = await _context.Set<Attachments>()
                .AsNoTracking()
                .Where(a => a.Id == id && !a.IsDeleted)
                .Select(a => new { a.FileData, a.FileName, a.FileType })
                .FirstOrDefaultAsync(ct);

            return attachment is null ? null : (attachment.FileData, attachment.FileName, attachment.FileType);

        }


        public async Task AddForStudentWorkAsync(int studentWorkId, string fileName, string fileType, byte[] fileData, int actorUserId, CancellationToken ct)
        {
            var now = DateTime.UtcNow;
            var entity = new Attachments
            {
                StudentWorkId = studentWorkId,
                WorkCheckId = null,
                FileName = fileName.Trim(),
                FileType = fileType.Trim(),
                FileSize = fileData.LongLength,
                FileData = fileData,
                UploadedBy = actorUserId,
                UploadedOn = now,
                ModifiedBy = null,
                ModifiedOn = null,
                IsDeleted = false,
                DeletedOn = null,
                DeletedBy = null
            };
            await _context.Set<Attachments>().AddAsync(entity, ct);
        }


        public async Task AddForWorkCheckAsync(int workCheckId, string fileName, string fileType, byte[] fileData, int actorUserId, CancellationToken ct)
        {
            var now = DateTime.UtcNow;
            var entity = new Attachments
            {
                StudentWorkId = null,
                WorkCheckId = workCheckId,
                FileName = fileName.Trim(),
                FileType = fileType.Trim(),
                FileSize = fileData.LongLength,
                FileData = fileData,
                UploadedBy = actorUserId,
                UploadedOn = now,
                ModifiedBy = null,
                ModifiedOn = null,
                IsDeleted = false,
                DeletedOn = null,
                DeletedBy = null
            };
            await _context.Set<Attachments>().AddAsync(entity, ct);
        }


        public async Task RenameAsync(int id, string newFileName, int actorUserId, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(newFileName))
                throw new ArgumentException("New file name is required.", nameof(newFileName));

            var entity = await _context.Set<Attachments>()
                .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted, ct)
                ?? throw new KeyNotFoundException($"Attachment #{id} not found.");

            entity.FileName = newFileName.Trim();
            entity.ModifiedBy = actorUserId;
            entity.ModifiedOn = DateTime.UtcNow;
        }

        public async Task SoftDeleteAsync(int id, int actorUserId, CancellationToken ct)
        {
            var entity = await _context.Set<Attachments>()
                .FirstOrDefaultAsync(a => a.Id == id, ct);

            if (entity is null || entity.IsDeleted) return;

            entity.IsDeleted = true;
            entity.DeletedOn = DateTime.UtcNow;
            entity.DeletedBy = actorUserId;
        }



    }
}
