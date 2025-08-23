using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWMService.Application.UseCases.Attachments.Queries.GetContent
{
    public sealed class DownloadAttachmentQueryValidator : AbstractValidator<DownloadAttachmentQuery>
    {
        public DownloadAttachmentQueryValidator() 
        {
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }
}
