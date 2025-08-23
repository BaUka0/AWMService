using AWMService.Application.Abstractions.Data;
using AWMService.Application.Abstractions.Repositories;
using AWMService.Domain.Constants;
using KDS.Primitives.FluentResult;
using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWMService.Application.UseCases.Attachments.Commands.RenameAttachment
{
    public sealed class RenameAttachmentCommandHandler(
        IAttachmentsRepository repo,
        IUnitOfWork uow,
        ILogger<RenameAttachmentCommandHandler> logger
        ) : IRequestHandler<RenameAttachmentCommand, Result>
    {
        public async Task<Result> Handle(RenameAttachmentCommand request, CancellationToken ct)
        {
            using var scope = logger.BeginScope(new Dictionary<string, object>
            {
                ["AttachmentId"] = request.Id,
                ["ActorUserId"] = request.ActorUserId,
                ["NewFileName"] = request.NewFileName
            });

            logger.LogInformation("Renaming attachment #{AttachmentId}", request.Id);

            var exists = await repo.GetAttachmentByIdAsync(request.Id, ct);
            if (exists is null)
            {
                logger.LogWarning("Attachment #{AttachmentId} not found.", request.Id);
                return Result.Failure(new Error(ErrorCode.NotFound, "Файл не найден."));
            }

            await uow.BeginTransactionAsync(ct);

            try
            {
                await repo.RenameAsync(request.Id, request.NewFileName.Trim(), request.ActorUserId, ct);
                await uow.CommitAsync();
            }

            catch(ArgumentException aex)
            {
                logger.LogWarning(aex, "Invalid new file name for attachment #{AttachmentId}", request.Id);
                await uow.RollbackAsync(ct);
                return Result.Failure(new Error(ErrorCode.BadRequest, aex.Message));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to rename attachment #{AttachmentId}", request.Id);
                await uow.RollbackAsync(ct);
                return Result.Failure(new Error(ErrorCode.InternalServerError, "Не удалось переименовать файл."));
            }

            logger.LogInformation("Attachment #{AttachmentId} renamed.", request.Id);
            return Result.Success();
        }
    }
}
