using AWMService.Application.Abstractions.Repositories;
using AWMService.Application.Abstractions.Data;
using AWMService.Application.Abstractions.Services;
using AWMService.Application.DTOs;
using AWMService.Domain.Constants;
using AWMService.Domain.Entities;
using KDS.Primitives.FluentResult;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AWMService.Application.UseCases.Auth.Commands.Register
{
    public class RegisterCommandHandler(
        IUsersRepository usersRepository,
        IRolesRepository rolesRepository,
        ITokenService tokenService,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork,
        IJwtSettings jwtSettings,
        ILogger<RegisterCommandHandler> logger) : IRequestHandler<RegisterCommand, Result<AuthResult>>
    {
        public async Task<Result<AuthResult>> Handle(RegisterCommand request, CancellationToken ct)
        {
            using var scope = logger.BeginScope(new Dictionary<string, object> { ["Email"] = request.Email });
            logger.LogInformation("Attempting to register new user with email {Email}", request.Email);

            var exists = await usersRepository.GetByEmailAsync(request.Email, ct);
            if (exists != null)
            {
                logger.LogWarning("Registration failed: User with email {Email} already exists.", request.Email);
                return Result.Failure<AuthResult>(new Error(ErrorCode.Conflict, "User with this email already exists"));
            }

            var roles = await rolesRepository.GetByIdsWithPermissionsAsync(request.RoleIds, ct);
            var existingRoleIds = roles.Select(r => r.Id).ToHashSet();
            var missingRoleIds = request.RoleIds.Where(id => !existingRoleIds.Contains(id)).ToList();

            if (missingRoleIds.Any())
            {
                logger.LogWarning("Registration failed: Roles with IDs {RoleIds} do not exist.", string.Join(", ", missingRoleIds));
                return Result.Failure<AuthResult>(new Error(ErrorCode.NotFound, $"Roles with IDs {string.Join(", ", missingRoleIds)} do not exist"));
            }

            var passwordHash = passwordHasher.HashPassword(request.Password);
            var refreshToken = tokenService.GenerateRefreshToken();
            var refreshTokenExpiry = DateTime.UtcNow.AddDays(jwtSettings.RefreshTokenExpirationInDays);

            var user = new Users
            {
                CreatedOn = DateTime.UtcNow,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Login = request.Email,
                PasswordHash = passwordHash,
                UserTypeId = request.UserTypeId,
                RefreshToken = refreshToken,
                RefreshTokenExpiryTime = refreshTokenExpiry
            };

            foreach (var roleId in request.RoleIds)
            {
                user.UserRoles.Add(new UserRoles
                {
                    RoleId = roleId,
                    AssignedOn = DateTime.UtcNow,
                    AssignedBy = null
                });
            }

            await unitOfWork.BeginTransactionAsync(ct);
            try
            {
                await usersRepository.AddUserAsync(user, ct);
                await unitOfWork.CommitAsync(ct);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while creating user with email {Email}", request.Email);
                await unitOfWork.RollbackAsync(ct);
                return Result.Failure<AuthResult>(new Error(ErrorCode.InternalServerError, "Failed to create user."));
            }
            
            var roleNames = roles.Select(r => r.Name);
            var permissions = roles
                .SelectMany(r => r.RolePermissions)
                .Select(rp => rp.Permission.Name)
                .Distinct();

            var accessToken = tokenService.GenerateAccessToken(user, roleNames, permissions);

            var result = new AuthResult
            {
                AccessToken = accessToken,
                RefreshToken = user.RefreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(jwtSettings.AccessTokenExpirationInMinutes),
                User = new UserDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Roles = roleNames.ToList()
                }
            };

            logger.LogInformation("User {Email} registered successfully with ID {UserId}", user.Email, user.Id);
            return result;
        }
    }
}
