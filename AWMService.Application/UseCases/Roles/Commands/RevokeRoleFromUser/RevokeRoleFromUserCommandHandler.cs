using AWMService.Application.Abstractions.Data;
using AWMService.Application.Abstractions.Repositories;
using AWMService.Domain.Constatns;
using KDS.Primitives.FluentResult;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AWMService.Application.UseCases.Roles.Commands.RevokeRoleFromUser
{
    public class RevokeRoleFromUserCommandHandler : IRequestHandler<RevokeRoleFromUserCommand, Result>
    {
        private readonly IRolesRepository _rolesRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RevokeRoleFromUserCommandHandler> _logger;

        public RevokeRoleFromUserCommandHandler(
            IRolesRepository rolesRepository,
            IUnitOfWork unitOfWork,
            ILogger<RevokeRoleFromUserCommandHandler> logger)
        {
            _rolesRepository = rolesRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result> Handle(RevokeRoleFromUserCommand request, CancellationToken ct)
        {
            _logger.LogInformation("Attempting to revoke role {RoleId} from user {UserId} by actor {ActorUserId}",
                request.RoleId, request.UserId, request.ActorUserId);

            await _unitOfWork.BeginTransactionAsync(ct);
            try
            {
                await _rolesRepository.RemoveFromUserAsync(request.RoleId, request.UserId, request.ActorUserId, ct);
                await _unitOfWork.CommitAsync(ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while revoking role {RoleId} from user {UserId}", request.RoleId, request.UserId);
                await _unitOfWork.RollbackAsync(ct);
                return Result.Failure(new Error(ErrorCode.InternalServerError, "Failed to revoke role from user"));
            }

            _logger.LogInformation("Role {RoleId} revoked from user {UserId} successfully", request.RoleId, request.UserId);
            return Result.Success();
        }
    }
}
