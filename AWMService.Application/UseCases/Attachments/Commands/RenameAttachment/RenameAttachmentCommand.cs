using KDS.Primitives.FluentResult;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWMService.Application.UseCases.Attachments.Commands.RenameAttachment
{
    public sealed record RenameAttachmentCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public string NewFileName { get; set; } = default!;
        public int ActorUserId { get; set; }
    }
}
