using AWMService.Application.Abstractions;
using AWMService.Domain.Constatns;
using KDS.Primitives.FluentResult;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AWMService.Application.UseCases.Auth.Commands.Logout
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Result>
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<LogoutCommandHandler> _logger;

        public LogoutCommandHandler(IUsersRepository usersRepository, IUnitOfWork unitOfWork, ILogger<LogoutCommandHandler> logger)
        {
            _usersRepository = usersRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result> Handle(LogoutCommand request, CancellationToken ct)
        {
            _logger.LogInformation("Attempting to log out user with ID {UserId}", request.UserId);

            var user = await _usersRepository.GetByIdAsync(request.UserId, ct);

            if (user == null)
            {
                _logger.LogWarning("Logout failed: User with ID {UserId} not found.", request.UserId);
                return Result.Failure(new Error(ErrorCode.NotFound, "User not found"));
            }

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;

            await _unitOfWork.BeginTransactionAsync(ct);
            try
            {
                await _unitOfWork.CommitAsync(ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during logout for user {UserId}", request.UserId);
                await _unitOfWork.RollbackAsync(ct);
                return Result.Failure(new Error(ErrorCode.InternalServerError, "Failed to logout user."));
            }

            _logger.LogInformation("User with ID {UserId} logged out successfully.", request.UserId);
            return Result.Success();
        }
    }
}
