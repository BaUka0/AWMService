using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWMService.Application.UseCases.Attachments.Commands.UploadAttachment.ForWorkCheck
{
    public sealed class UploadAttachmentForWorkCheckCommandValidator : AbstractValidator<UploadAttachmentForWorkCheckCommand>
    {
        public UploadAttachmentForWorkCheckCommandValidator()
        {
          
            RuleFor(x => x.WorkCheckId).GreaterThan(0);
            RuleFor(x => x.FileName).NotEmpty();
            RuleFor(x => x.FileType).NotEmpty();
            RuleFor(x => x.FileData).NotNull().Must(d => d.Length > 0).WithMessage("Файл пустой.");
            RuleFor(x => x.ActorUserId).GreaterThan(0);
        }
    }
}
