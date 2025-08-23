using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWMService.Application.UseCases.Attachments.Commands.DeleteAttachment
{
    public sealed class DeleteAttachmentCommandValidator : AbstractValidator<DeleteAttachmentCommand>
    {
        public DeleteAttachmentCommandValidator() 
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.ActorUserId).GreaterThan(0);
        }
    }
}
