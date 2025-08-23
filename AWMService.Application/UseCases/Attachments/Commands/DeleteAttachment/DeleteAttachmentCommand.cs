using KDS.Primitives.FluentResult;
using MediatR;


namespace AWMService.Application.UseCases.Attachments.Commands.DeleteAttachment
{
    public sealed record DeleteAttachmentCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public int ActorUserId { get; set; }
    }
}
