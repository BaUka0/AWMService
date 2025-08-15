using AWMService.Application.Abstractions;
using AWMService.Application.Abstractions.Services;
using AWMService.Application.DTOs;
using AWMService.Domain.Constatns;
using AWMService.Domain.Entities;
using KDS.Primitives.FluentResult;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AWMService.Application.UseCases.Auth.Commands.Register
{
    internal class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<AuthResult>>
    {
        private readonly IUsersRepository _usersRepository;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtSettings _jwtSettings;
        private readonly ILogger<RegisterCommandHandler> _logger;

        public RegisterCommandHandler(
            IUsersRepository usersRepository, 
            ITokenService tokenService, 
            IPasswordHasher passwordHasher, 
            IUnitOfWork unitOfWork,
            IJwtSettings jwtSettings,
            ILogger<RegisterCommandHandler> logger)
        {
            _usersRepository = usersRepository;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
            _jwtSettings = jwtSettings;
            _logger = logger;
        }

        public async Task<Result<AuthResult>> Handle(RegisterCommand request, CancellationToken ct)
        {
            _logger.LogInformation("Attempting to register new user with email {Email}", request.Email);

            var exists = await _usersRepository.GetByEmailAsync(request.Email, ct);
            if (exists != null)
            {
                _logger.LogWarning("Registration failed: User with email {Email} already exists.", request.Email);
                return Result.Failure<AuthResult>(new Error(ErrorCode.Conflict, "User with this email already exists"));
            }

            var passwordHash = _passwordHasher.HashPassword(request.Password);
            var refreshToken = _tokenService.GenerateRefreshToken();
            var refreshTokenExpiry = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays);

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
                    AssignedBy = 0
                });
            }

            await _unitOfWork.BeginTransactionAsync(ct);
            try
            {
                await _usersRepository.AddUserAsync(user, ct);
                await _unitOfWork.CommitAsync(ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating user with email {Email}", request.Email);
                await _unitOfWork.RollbackAsync(ct);
                return Result.Failure<AuthResult>(new Error(ErrorCode.InternalServerError, "Failed to create user."));
            }
            
            var savedUser = await _usersRepository.GetByEmailWithRolesAsync(request.Email, ct);
            if (savedUser == null)
            {
                _logger.LogError("Failed to retrieve user {Email} immediately after creation.", request.Email);
                return Result.Failure<AuthResult>(new Error(ErrorCode.NotFound, "Failed to retrieve created user."));
            }

            var roles = savedUser.UserRoles.Select(ur => ur.Role.Name);
            var permissions = savedUser.UserRoles
                .SelectMany(ur => ur.Role.RolePermissions)
                .Select(rp => rp.Permission.Name)
                .Distinct();

            var accessToken = _tokenService.GenerateAccessToken(savedUser, roles, permissions);

            var result = new AuthResult
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationInMinutes),
                User = new UserDto
                {
                    Id = savedUser.Id,
                    FirstName = savedUser.FirstName,
                    LastName = savedUser.LastName,
                    Email = savedUser.Email,
                    Roles = roles.ToList()
                }
            };

            _logger.LogInformation("User {Email} registered successfully with ID {UserId}", savedUser.Email, savedUser.Id);
            return result;
        }
    }
}
