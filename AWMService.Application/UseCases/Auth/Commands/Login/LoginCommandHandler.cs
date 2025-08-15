using AWMService.Domain.Constatns;
using AWMService.Application.Abstractions;
using AWMService.Application.Abstractions.Services;
using AWMService.Application.DTOs;
using KDS.Primitives.FluentResult;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AWMService.Application.UseCases.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthResult>>
    {
        private readonly IUsersRepository _usersRepository;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtSettings _jwtSettings;
        private readonly ILogger<LoginCommandHandler> _logger;

        public LoginCommandHandler(
            IUsersRepository usersRepository, 
            ITokenService tokenService, 
            IPasswordHasher passwordHasher, 
            IUnitOfWork unitOfWork,
            IJwtSettings jwtSettings,
            ILogger<LoginCommandHandler> logger)
        {
            _usersRepository = usersRepository;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
            _jwtSettings = jwtSettings;
            _logger = logger;
        }

        public async Task<Result<AuthResult>> Handle(LoginCommand request, CancellationToken ct)
        {
            _logger.LogInformation("Attempting to log in user with email {Email}", request.Email);

            var user = await _usersRepository.GetByEmailWithRolesAsync(request.Email, ct);

            if (user == null)
            {
                _logger.LogWarning("Login failed: User with email {Email} not found.", request.Email);
                return Result.Failure<AuthResult>(new Error(ErrorCode.NotFound, "User not found"));
            }

            if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            {
                _logger.LogWarning("Login failed: Invalid password for user with email {Email}.", request.Email);
                return Result.Failure<AuthResult>(new Error(ErrorCode.Unauthorized, "Invalid password"));
            }

            var roles = user.UserRoles.Select(ur => ur.Role.Name);
            var permissions = user.UserRoles
                .SelectMany(ur => ur.Role.RolePermissions)
                .Select(rp => rp.Permission.Name)
                .Distinct();

            var accessToken = _tokenService.GenerateAccessToken(user, roles, permissions);
            var refreshToken = _tokenService.GenerateRefreshToken();

            await _unitOfWork.BeginTransactionAsync(ct);
            try
            {
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays);
                await _unitOfWork.CommitAsync(ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating refresh token for user {Email}", user.Email);
                await _unitOfWork.RollbackAsync(ct);
                return Result.Failure<AuthResult>(new Error(ErrorCode.InternalServerError, "Failed to update user"));
            }

            var result = new AuthResult
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
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

            _logger.LogInformation("User {Email} logged in successfully.", user.Email);
            return result;
        }
    }
}
