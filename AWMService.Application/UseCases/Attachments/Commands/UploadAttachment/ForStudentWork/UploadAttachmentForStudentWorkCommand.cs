using KDS.Primitives.FluentResult;
using MediatR;


namespace AWMService.Application.UseCases.Attachments.Commands.UploadAttachment.ForStudentWork
{
    public sealed record UploadAttachmentForStudentWorkCommand : IRequest<Result>
    {
        public int StudentWorkId { get; set; }
        public string FileName { get; set; } = default!;
        public string FileType { get; set; } = default!;
        public byte[] FileData { get; set; } = default!;
        public int ActorUserId { get; set; }
       
    }
}
