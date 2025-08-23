using AWMService.Application.Abstractions.Data;
using AWMService.Application.Abstractions.Repositories;
using AWMService.Domain.Constants;
using KDS.Primitives.FluentResult;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AWMService.Application.UseCases.Attachments.Commands.UploadAttachment.ForStudentWork
{
    public sealed class UploadAttachmentForStudentWorkCommandHandler
        (
        IAttachmentsRepository repo,
        IUnitOfWork uow,
        ILogger<UploadAttachmentForStudentWorkCommandHandler> logger
        ) : IRequestHandler<UploadAttachmentForStudentWorkCommand, Result>
    {
        public async Task<Result> Handle(UploadAttachmentForStudentWorkCommand request, CancellationToken ct)
        {
            using var scope = logger.BeginScope(new Dictionary<string, object>
            {
                ["StudentWorkId"] = request.StudentWorkId,
                ["FileName"] = request.FileName,
                ["FileType"] = request.FileType,
                ["ActorUserId"] = request.ActorUserId,
                ["FileSize"] = request.FileData?.LongLength ?? 0
            });
            logger.LogInformation("Uploading attachment for StudentWork {StudentWorkId}", request.StudentWorkId);

            await uow.BeginTransactionAsync(ct);

            try
            {
                await repo.AddForStudentWorkAsync(
                    request.StudentWorkId,
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
                logger.LogError(ex, "Failed to upload attachment for StudentWork {StudentWorkId}", request.StudentWorkId);
                await uow.RollbackAsync(ct);
                return Result.Failure(new Error(ErrorCode.InternalServerError, "Не удалось загрузить файл."));
            }

            logger.LogInformation("Attachment uploaded for StudentWork {StudentWorkId}", request.StudentWorkId);
            return Result.Success();


        }
    }
}
