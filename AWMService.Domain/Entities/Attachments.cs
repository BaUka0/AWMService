using System.Numerics;

namespace AWMService.Domain.Entities
{
    public class Attachments
    {
        public int AttachmentsId { get; set; }
        public int AssociatedEntityId {get; set;}
        public string EntityType { get; set; } 
        public string FileName { get; set; }
        public byte FileData { get; set; }
        public string FileType { get; set; }
        public long FileSize { get; set; }
        public int UploadedById { get; set; }
        public DateTime UploadedOn { get; set; }

        public Users UploadedBy { get; set; }
    }
}
