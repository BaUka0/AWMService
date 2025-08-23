using AWMService.Application.Abstractions.Data;
using AWMService.Application.Abstractions.Repositories;
using AWMService.Domain.Constants;
using KDS.Primitives.FluentResult;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWMService.Application.UseCases.Attachments.Commands.DeleteAttachment
{
    public sealed class DeleteAttachmentCommandHandler(
        IAttachmentsRepository repo,
        IUnitOfWork uow,
        ILogger<DeleteAttachmentCommandHandler> logger
        ) : IRequestHandler<DeleteAttachmentCommand, Result>
    {
        public async Task<Result> Handle(DeleteAttachmentCommand request, CancellationToken ct)
        {
            using var scope = logger.BeginScope(new Dictionary<string, object>
            {
                ["AttachmentId"] = request.Id,
                ["ActorUserId"] = request.ActorUserId
            });

            logger.LogInformation("Soft delete attachment #{AttachmentId}", request.Id);

            var exists = await repo.GetAttachmentByIdAsync(request.Id, ct);
            if (exists is null)
            {
                logger.LogWarning("Attachment #{AttachmentId} not found.", request.Id);
                return Result.Failure(new Error(ErrorCode.NotFound, "Файл не найден."));
            }

            await uow.BeginTransactionAsync(ct);

            try
            {
                await repo.SoftDeleteAsync(request.Id, request.ActorUserId, ct);
                await uow.CommitAsync(ct);
            }

            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to soft delete attachment #{AttachmentId}", request.Id);
                await uow.RollbackAsync(ct);
                return Result.Failure(new Error(ErrorCode.InternalServerError, "Не удалось удалить файл."));
            }

            logger.LogInformation("Attachment #{AttachmentId} soft deleted.", request.Id);
            return Result.Success();
        }
    }
}
