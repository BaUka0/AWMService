using AWMService.Application.Abstractions.Repositories;
using AWMService.Application.Abstractions.Data;
using AWMService.Domain.Constants;
using KDS.Primitives.FluentResult;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AWMService.Application.UseCases.Auth.Commands.Logout
{
    public class LogoutCommandHandler(IUsersRepository usersRepository, IUnitOfWork unitOfWork, ILogger<LogoutCommandHandler> logger) : IRequestHandler<LogoutCommand, Result>
    {
        public async Task<Result> Handle(LogoutCommand request, CancellationToken ct)
        {
            using var scope = logger.BeginScope(new Dictionary<string, object> { ["UserId"] = request.UserId });
            logger.LogInformation("Attempting to log out user with ID {UserId}", request.UserId);

            var user = await usersRepository.GetByIdAsync(request.UserId, ct);

            if (user == null)
            {
                logger.LogWarning("Logout failed: User with ID {UserId} not found.", request.UserId);
                return Result.Failure(new Error(ErrorCode.NotFound, "User not found"));
            }

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;

            await unitOfWork.BeginTransactionAsync(ct);
            try
            {
                await unitOfWork.CommitAsync(ct);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during logout for user {UserId}", request.UserId);
                await unitOfWork.RollbackAsync(ct);
                return Result.Failure(new Error(ErrorCode.InternalServerError, "Failed to logout user."));
            }

            logger.LogInformation("User with ID {UserId} logged out successfully.", request.UserId);
            return Result.Success();
        }
    }
}
