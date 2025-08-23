using AWMService.Application.Abstractions.Repositories;
using AWMService.Application.DTOs;
using AWMService.Domain.Constants;
using KDS.Primitives.FluentResult;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace AWMService.Application.UseCases.Attachments.Queries.GetContent
{
    public sealed class DownloadAttachmentQueryHandler(
        IAttachmentsRepository repo,
        ILogger<DownloadAttachmentQueryHandler> logger

        ) : IRequestHandler<DownloadAttachmentQuery, Result<AttachmentContentDto>>
    {
        public async Task<Result<AttachmentContentDto>> Handle(DownloadAttachmentQuery request, CancellationToken ct)
        {
            using var scope = logger.BeginScope(new Dictionary<string, object>
            {
                ["AttachmentId"] = request.Id
            });

            logger.LogInformation("Downloading attachment #{AttachmentId}", request.Id);

            var content = await repo.GetContentAsync(request.Id, ct);
            if (content is null)
            {
                logger.LogWarning("Attachment #{AttachmentId} not found or deleted.", request.Id);
                return Result.Failure<AttachmentContentDto>(new Error(ErrorCode.NotFound, "Файл не найден."));
            }

            var dto = new AttachmentContentDto(content.Value.Data, content.Value.FileName, content.Value.FileType);
            logger.LogInformation("Attachment #{AttachmentId} content retrieved.", request.Id);
            return dto;
        }
    }
}
