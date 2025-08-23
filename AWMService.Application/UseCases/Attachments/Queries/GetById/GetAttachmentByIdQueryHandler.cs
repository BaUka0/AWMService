using AWMService.Application.Abstractions.Repositories;
using AWMService.Application.DTOs;
using AWMService.Domain.Constants;
using KDS.Primitives.FluentResult;
using MediatR;
using Microsoft.Extensions.Logging;


namespace AWMService.Application.UseCases.Attachments.Queries.GetById
{
    public sealed class GetAttachmentByIdQueryHandler(
        IAttachmentsRepository repo,
        ILogger<GetAttachmentByIdQueryHandler> logger
        ) : IRequestHandler<GetAttachmentByIdQuery, Result<AttachmentDto>>
    {
        public async Task<Result<AttachmentDto>> Handle(GetAttachmentByIdQuery request, CancellationToken ct)
        {
            using var scope = logger.BeginScope(new Dictionary<string, object>
            {
                ["AttachmentId"] = request.Id,
            });
            logger.LogInformation("Getting attachment #{AttachmentId}", request.Id);

            var e = await repo.GetAttachmentByIdAsync(request.Id, ct);
            if (e is null)
            {
                logger.LogWarning("Attachment #{AttachmentId} not found.", request.Id);
                return Result.Failure<AttachmentDto>(new Error(ErrorCode.NotFound, "Файл не найден."));
            }

            var dto = new AttachmentDto(
                e.Id,
                e.FileName,
                e.FileType,
                e.FileSize,
                e.StudentWorkId,
                e.WorkCheckId,
                e.UploadedBy,
                e.UploadedOn,
                e.ModifiedBy,
                e.ModifiedOn
            );
            logger.LogInformation("Attachment #{AttachmentId} found.", request.Id);
            return dto;
        }
    }
}
