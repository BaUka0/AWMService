using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWMService.Application.UseCases.Attachments.Commands.RenameAttachment
{
    public sealed class RenameAttachmentCommandValidator : AbstractValidator<RenameAttachmentCommand>
    {
        public RenameAttachmentCommandValidator() 
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.NewFileName).NotEmpty();
            RuleFor(x => x.ActorUserId).GreaterThan(0);
        }
    }

}
