using AWMService.Application.Abstractions.Data;
using AWMService.Application.Abstractions.Repositories;
using AWMService.Domain.Constants;
using KDS.Primitives.FluentResult;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AWMService.Application.UseCases.Attachments.Commands.UploadAttachment.ForWorkCheck
{
    public sealed class UploadAttachmentForWorkCheckCommandHandler(
        IAttachmentsRepository repo,
        IUnitOfWork uow,
        ILogger<UploadAttachmentForWorkCheckCommandHandler> logger
        ) : IRequestHandler<UploadAttachmentForWorkCheckCommand, Result>
    {
        public async Task<Result> Handle(UploadAttachmentForWorkCheckCommand request, CancellationToken ct)
        {
            using var scope = logger.BeginScope(new Dictionary<string, object>
            {
              
                ["WorkCheckId"] = request.WorkCheckId,
                ["ActorUserId"] = request.ActorUserId,
                ["FileName"] = request.FileName,
                ["FileType"] = request.FileType,
                ["FileSize"] = request.FileData?.LongLength ?? 0
            });

            logger.LogInformation("Uploading attachment for WorkCheck {WorkCheckId}", request.WorkCheckId);

            await uow.BeginTransactionAsync(ct);

            try
            {
                await repo.AddForWorkCheckAsync(
                   
                    request.WorkCheckId,
                    request.FileName.Trim(),
                    request.FileType.Trim(),
                    request.FileData,
                    request.ActorUserId,
                    ct
                
                );

                await uow.CommitAsync(ct);
            }

            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to upload attachment for WorkCheck {WorkCheckId}", request.WorkCheckId);
                await uow.RollbackAsync(ct);
                return Result.Failure(new Error(ErrorCode.InternalServerError, "Не удалось загрузить файл."));
            }

            logger.LogInformation("Attachment uploaded for WorkCheck {WorkCheckId}", request.WorkCheckId);
            return Result.Success();
        }
    }
}
