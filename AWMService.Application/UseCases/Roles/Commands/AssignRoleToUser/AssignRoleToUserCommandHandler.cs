using AWMService.Application.Abstractions.Data;
using AWMService.Application.Abstractions.Repositories;
using AWMService.Domain.Constatns;
using KDS.Primitives.FluentResult;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AWMService.Application.UseCases.Roles.Commands.AssignRoleToUser
{
    public class AssignRoleToUserCommandHandler : IRequestHandler<AssignRoleToUserCommand, Result>
    {
        private readonly IRolesRepository _rolesRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AssignRoleToUserCommandHandler> _logger;

        public AssignRoleToUserCommandHandler(
            IRolesRepository rolesRepository,
            IUsersRepository usersRepository,
            IUnitOfWork unitOfWork,
            ILogger<AssignRoleToUserCommandHandler> logger)
        {
            _rolesRepository = rolesRepository;
            _usersRepository = usersRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result> Handle(AssignRoleToUserCommand request, CancellationToken ct)
        {
            _logger.LogInformation("Attempting to assign role {RoleId} to user {UserId} by actor {ActorUserId}", 
                request.RoleId, request.UserId, request.ActorUserId);

            var role = await _rolesRepository.GetByIdAsync(request.RoleId, ct);
            if (role == null)
            {
                _logger.LogWarning("Assign role failed: Role with ID {RoleId} not found", request.RoleId);
                return Result.Failure(new Error(ErrorCode.NotFound, "Role not found"));
            }

            var user = await _usersRepository.GetByIdAsync(request.UserId, ct);
            if (user == null)
            {
                _logger.LogWarning("Assign role failed: User with ID {UserId} not found", request.UserId);
                return Result.Failure(new Error(ErrorCode.NotFound, "User not found"));
            }

            await _unitOfWork.BeginTransactionAsync(ct);
            try
            {
                await _rolesRepository.AssignToUserAsync(request.RoleId, request.UserId, request.ActorUserId, ct);
                await _unitOfWork.CommitAsync(ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while assigning role {RoleId} to user {UserId}", request.RoleId, request.UserId);
                await _unitOfWork.RollbackAsync(ct);
                return Result.Failure(new Error(ErrorCode.InternalServerError, "Failed to assign role to user"));
            }

            _logger.LogInformation("Role {RoleId} assigned to user {UserId} successfully", request.RoleId, request.UserId);
            return Result.Success();
        }
    }
}
