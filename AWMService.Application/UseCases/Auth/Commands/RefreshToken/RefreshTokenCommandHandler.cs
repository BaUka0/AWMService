using AWMService.Application.Abstractions.Repositories;
using AWMService.Application.Abstractions.Data;
using AWMService.Application.Abstractions.Services;
using AWMService.Application.DTOs;
using AWMService.Domain.Constants;
using KDS.Primitives.FluentResult;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AWMService.Application.UseCases.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler(
        IUsersRepository usersRepository,
        ITokenService tokenService,
        IUnitOfWork unitOfWork,
        IJwtSettings jwtSettings,
        ILogger<RefreshTokenCommandHandler> logger) : IRequestHandler<RefreshTokenCommand, Result<AuthResult>>
    {
        public async Task<Result<AuthResult>> Handle(RefreshTokenCommand request, CancellationToken ct)
        {
            using var scope = logger.BeginScope(new Dictionary<string, object> { ["RefreshToken"] = request.RefreshToken });
            logger.LogInformation("Attempting to refresh token.");

            var user = await usersRepository.GetByRefreshTokenAsync(request.RefreshToken, ct);

            if (user == null)
            {
                logger.LogWarning("Token refresh failed: No user found for the provided refresh token.");
                return Result.Failure<AuthResult>(new Error(ErrorCode.Unauthorized, "Invalid refresh token."));
            }

            if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                logger.LogWarning("Token refresh failed for user {UserId}: Refresh token expired.", user.Id);
                return Result.Failure<AuthResult>(new Error(ErrorCode.Unauthorized, "Refresh token expired."));
            }
            
            var roles = user.UserRoles.Select(ur => ur.Role.Name);
            var permissions = user.UserRoles
                .SelectMany(ur => ur.Role.RolePermissions)
                .Select(rp => rp.Permission.Name)
                .Distinct();

            var newAccessToken = tokenService.GenerateAccessToken(user, roles, permissions);
            var newRefreshToken = tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(jwtSettings.RefreshTokenExpirationInDays);

            await unitOfWork.BeginTransactionAsync(ct);
            try
            {
                await unitOfWork.CommitAsync(ct);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while updating refresh token for user {UserId}", user.Id);
                await unitOfWork.RollbackAsync(ct);
                return Result.Failure<AuthResult>(new Error(ErrorCode.InternalServerError, "Failed to update refresh token."));
            }

            var result = new AuthResult
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(jwtSettings.AccessTokenExpirationInMinutes),
                User = new UserDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Roles = roles.ToList()
                }
            };

            logger.LogInformation("Token refreshed successfully for user {UserId}.", user.Id);
            return result;
        }
    }
}
