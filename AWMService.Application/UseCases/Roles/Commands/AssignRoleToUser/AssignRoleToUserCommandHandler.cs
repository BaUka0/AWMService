using AWMService.Application.Abstractions.Data;
using AWMService.Application.Abstractions.Repositories;
using AWMService.Domain.Constants;
using KDS.Primitives.FluentResult;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AWMService.Application.UseCases.Roles.Commands.AssignRoleToUser
{
    public class AssignRoleToUserCommandHandler(
        IRolesRepository rolesRepository,
        IUsersRepository usersRepository,
        IUnitOfWork unitOfWork,
        ILogger<AssignRoleToUserCommandHandler> logger) : IRequestHandler<AssignRoleToUserCommand, Result>
    {
        public async Task<Result> Handle(AssignRoleToUserCommand request, CancellationToken ct)
        {
            using var scope = logger.BeginScope(new Dictionary<string, object> { ["RoleId"] = request.RoleId, ["UserId"] = request.UserId, ["ActorUserId"] = request.ActorUserId });
            logger.LogInformation("Attempting to assign role {RoleId} to user {UserId} by actor {ActorUserId}", 
                request.RoleId, request.UserId, request.ActorUserId);

            var role = await rolesRepository.GetByIdAsync(request.RoleId, ct);
            if (role == null)
            {
                logger.LogWarning("Assign role failed: Role with ID {RoleId} not found", request.RoleId);
                return Result.Failure(new Error(ErrorCode.NotFound, "Role not found"));
            }

            var user = await usersRepository.GetByIdAsync(request.UserId, ct);
            if (user == null)
            {
                logger.LogWarning("Assign role failed: User with ID {UserId} not found", request.UserId);
                return Result.Failure(new Error(ErrorCode.NotFound, "User not found"));
            }

            await unitOfWork.BeginTransactionAsync(ct);
            try
            {
                await rolesRepository.AssignToUserAsync(request.RoleId, request.UserId, request.ActorUserId, ct);
                await unitOfWork.CommitAsync(ct);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while assigning role {RoleId} to user {UserId}", request.RoleId, request.UserId);
                await unitOfWork.RollbackAsync(ct);
                return Result.Failure(new Error(ErrorCode.InternalServerError, "Failed to assign role to user"));
            }
            logger.LogInformation("Role {RoleId} assigned to user {UserId} successfully", request.RoleId, request.UserId);
            return Result.Success();
        }
    }
}
