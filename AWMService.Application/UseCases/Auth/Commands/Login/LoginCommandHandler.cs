using AWMService.Domain.Constants;
using AWMService.Application.Abstractions.Repositories;
using AWMService.Application.Abstractions.Data;
using AWMService.Application.Abstractions.Services;
using AWMService.Application.DTOs;
using KDS.Primitives.FluentResult;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AWMService.Application.UseCases.Auth.Commands.Login
{
    public class LoginCommandHandler(
        IUsersRepository usersRepository,
        ITokenService tokenService,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork,
        IJwtSettings jwtSettings,
        ILogger<LoginCommandHandler> logger) : IRequestHandler<LoginCommand, Result<AuthResult>>
    {
        public async Task<Result<AuthResult>> Handle(LoginCommand request, CancellationToken ct)
        {
            using var scope = logger.BeginScope(new Dictionary<string, object> { ["Email"] = request.Email });
            logger.LogInformation("Attempting to log in user with email {Email}", request.Email);

            var user = await usersRepository.GetByEmailWithRolesAsync(request.Email, ct);

            if (user == null)
            {
                logger.LogWarning("Login failed: User with email {Email} not found.", request.Email);
                return Result.Failure<AuthResult>(new Error(ErrorCode.NotFound, "User not found"));
            }

            if (!passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            {
                logger.LogWarning("Login failed: Invalid password for user with email {Email}.", request.Email);
                return Result.Failure<AuthResult>(new Error(ErrorCode.Unauthorized, "Invalid password"));
            }

            var roles = user.UserRoles.Select(ur => ur.Role.Name);
            var permissions = user.UserRoles
                .SelectMany(ur => ur.Role.RolePermissions)
                .Select(rp => rp.Permission.Name)
                .Distinct();

            var accessToken = tokenService.GenerateAccessToken(user, roles, permissions);
            var refreshToken = tokenService.GenerateRefreshToken();

            await unitOfWork.BeginTransactionAsync(ct);
            try
            {
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(jwtSettings.RefreshTokenExpirationInDays);
                await unitOfWork.CommitAsync(ct);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while updating refresh token for user {Email}", user.Email);
                await unitOfWork.RollbackAsync(ct);
                return Result.Failure<AuthResult>(new Error(ErrorCode.InternalServerError, "Failed to update user"));
            }

            var result = new AuthResult
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
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

            logger.LogInformation("User {Email} logged in successfully.", user.Email);
            return result;
        }
    }
}
