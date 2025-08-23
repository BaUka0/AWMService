using KDS.Primitives.FluentResult;
using MediatR;


namespace AWMService.Application.UseCases.Attachments.Commands.UploadAttachment.ForWorkCheck
{
    public sealed record UploadAttachmentForWorkCheckCommand : IRequest<Result>
    {
        
        public int WorkCheckId { get; set; }
        public string FileName { get; set; } = default!;
        public string FileType { get; set; } = default!;
        public byte[] FileData { get; set; } = default!;
        public int ActorUserId { get; set; }

       
    }
}
