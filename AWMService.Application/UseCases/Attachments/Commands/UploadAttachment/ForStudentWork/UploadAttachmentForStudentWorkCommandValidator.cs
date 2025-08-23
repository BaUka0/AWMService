using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWMService.Application.UseCases.Attachments.Commands.UploadAttachment.ForStudentWork
{
    public sealed class UploadAttachmentForStudentWorkCommandValidator : AbstractValidator<UploadAttachmentForStudentWorkCommand>
    {
        public UploadAttachmentForStudentWorkCommandValidator()
        {
            RuleFor(x=>x.StudentWorkId).GreaterThan(0);
            RuleFor(x => x.FileName).NotEmpty();
            RuleFor(x=> x.FileType).NotEmpty();
            RuleFor(x => x.FileData).NotNull().Must(d => d.Length > 0).WithMessage("File is empty.");
            RuleFor(x => x.ActorUserId).GreaterThan(0);
        }
    }
}
