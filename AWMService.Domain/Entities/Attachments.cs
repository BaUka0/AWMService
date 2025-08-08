using AWMService.Domain.Commons;

namespace AWMService.Domain.Entities
{
    public class Attachments : AuditableSoftDeletableEntity
    {
        public int? StudentWorkId { get; set; }
        public StudentWork? StudentWork { get; set; }

        public int? WorkCheckId { get; set; }
        public WorkChecks? WorkCheck { get; set; }

        public string FileName { get; set; } = null!;
        public byte[] FileData { get; set; } = Array.Empty<byte>();
        public string? FileType { get; set; }
        public long FileSize { get; set; }

        public int UploadedBy { get; set; }
        public Users UploadedByUser { get; set; } = null!;
        public DateTime UploadedOn { get; set; }
    }
}
