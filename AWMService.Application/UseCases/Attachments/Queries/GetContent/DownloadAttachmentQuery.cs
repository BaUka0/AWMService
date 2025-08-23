using AWMService.Application.DTOs;
using KDS.Primitives.FluentResult;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWMService.Application.UseCases.Attachments.Queries.GetContent
{
    public sealed record DownloadAttachmentQuery(int Id) : IRequest<Result<AttachmentContentDto>>;
  
}
