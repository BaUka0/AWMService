using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWMService.Application.UseCases.Attachments.Queries.GetById
{
    public sealed class GetAttachmentByIdQueryValidator : AbstractValidator<GetAttachmentByIdQuery>
    {
        public GetAttachmentByIdQueryValidator() 
        {
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }
}
