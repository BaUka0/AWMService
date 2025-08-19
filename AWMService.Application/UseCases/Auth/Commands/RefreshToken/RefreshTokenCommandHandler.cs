using AWMService.Application.Abstractions.Repositories;
using AWMService.Application.Abstractions.Data;
using AWMService.Application.Abstractions.Services;
using AWMService.Application.DTOs;
using AWMService.Domain.Constatns;
using KDS.Primitives.FluentResult;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AWMService.Application.UseCases.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<AuthResult>>
    {
        private readonly IUsersRepository _usersRepository;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtSettings _jwtSettings;
        private readonly ILogger<RefreshTokenCommandHandler> _logger;

        public RefreshTokenCommandHandler(
            IUsersRepository usersRepository, 
            ITokenService tokenService, 
            IUnitOfWork unitOfWork, 
            IJwtSettings jwtSettings,
            ILogger<RefreshTokenCommandHandler> logger)
        {
            _usersRepository = usersRepository;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
            _jwtSettings = jwtSettings;
            _logger = logger;
        }

        public async Task<Result<AuthResult>> Handle(RefreshTokenCommand request, CancellationToken ct)
        {
            _logger.LogInformation("Attempting to refresh token.");

            var user = await _usersRepository.GetByRefreshTokenAsync(request.RefreshToken, ct);

            if (user == null)
            {
                _logger.LogWarning("Token refresh failed: No user found for the provided refresh token.");
                return Result.Failure<AuthResult>(new Error(ErrorCode.Unauthorized, "Invalid refresh token."));
            }

            if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                _logger.LogWarning("Token refresh failed for user {UserId}: Refresh token expired.", user.Id);
                return Result.Failure<AuthResult>(new Error(ErrorCode.Unauthorized, "Refresh token expired."));
            }
            
            var roles = user.UserRoles.Select(ur => ur.Role.Name);
            var permissions = user.UserRoles
                .SelectMany(ur => ur.Role.RolePermissions)
                .Select(rp => rp.Permission.Name)
                .Distinct();

            var newAccessToken = _tokenService.GenerateAccessToken(user, roles, permissions);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays);

            await _unitOfWork.BeginTransactionAsync(ct);
            try
            {
                await _unitOfWork.CommitAsync(ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating refresh token for user {UserId}", user.Id);
                await _unitOfWork.RollbackAsync(ct);
                return Result.Failure<AuthResult>(new Error(ErrorCode.InternalServerError, "Failed to update refresh token."));
            }

            var result = new AuthResult
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationInMinutes),
                User = new UserDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Roles = roles.ToList()
                }
            };

            _logger.LogInformation("Token refreshed successfully for user {UserId}.", user.Id);
            return result;
        }
    }
}
