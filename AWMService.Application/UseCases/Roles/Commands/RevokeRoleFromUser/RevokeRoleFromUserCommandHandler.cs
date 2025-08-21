using AWMService.Application.Abstractions.Data;
using AWMService.Application.Abstractions.Repositories;
using AWMService.Domain.Constants;
using KDS.Primitives.FluentResult;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AWMService.Application.UseCases.Roles.Commands.RevokeRoleFromUser
{
    public class RevokeRoleFromUserCommandHandler(
        IRolesRepository rolesRepository,
        IUnitOfWork unitOfWork,
        ILogger<RevokeRoleFromUserCommandHandler> logger) : IRequestHandler<RevokeRoleFromUserCommand, Result>
    {
        public async Task<Result> Handle(RevokeRoleFromUserCommand request, CancellationToken ct)
        {
            using var scope = logger.BeginScope(new Dictionary<string, object> { ["RoleId"] = request.RoleId, ["UserId"] = request.UserId, ["ActorUserId"] = request.ActorUserId });
            logger.LogInformation("Attempting to revoke role {RoleId} from user {UserId} by actor {ActorUserId}",
                request.RoleId, request.UserId, request.ActorUserId);

            await unitOfWork.BeginTransactionAsync(ct);
            try
            {
                await rolesRepository.RemoveFromUserAsync(request.RoleId, request.UserId, request.ActorUserId, ct);
                await unitOfWork.CommitAsync(ct);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while revoking role {RoleId} from user {UserId}", request.RoleId, request.UserId);
                await unitOfWork.RollbackAsync(ct);
                return Result.Failure(new Error(ErrorCode.InternalServerError, "Failed to revoke role from user"));
            }

            logger.LogInformation("Role {RoleId} revoked from user {UserId} successfully", request.RoleId, request.UserId);
            return Result.Success();
        }
    }
}
