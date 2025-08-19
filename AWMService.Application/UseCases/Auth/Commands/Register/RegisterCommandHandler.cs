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
        private readonly IRolesRepository _rolesRepository;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtSettings _jwtSettings;
        private readonly ILogger<RegisterCommandHandler> _logger;

        public RegisterCommandHandler(
            IUsersRepository usersRepository,
            IRolesRepository rolesRepository,
            ITokenService tokenService,
            IPasswordHasher passwordHasher,
            IUnitOfWork unitOfWork,
            IJwtSettings jwtSettings,
            ILogger<RegisterCommandHandler> logger)
        {
            _usersRepository = usersRepository;
            _rolesRepository = rolesRepository;
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
                    AssignedBy = null
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
            
            var roles = await _rolesRepository.GetByIdsWithPermissionsAsync(request.RoleIds, ct);
            var roleNames = roles.Select(r => r.Name);
            var permissions = roles
                .SelectMany(r => r.RolePermissions)
                .Select(rp => rp.Permission.Name)
                .Distinct();

            var accessToken = _tokenService.GenerateAccessToken(user, roleNames, permissions);

            var result = new AuthResult
            {
                AccessToken = accessToken,
                RefreshToken = user.RefreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationInMinutes),
                User = new UserDto
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Roles = roleNames.ToList()
                }
            };

            _logger.LogInformation("User {Email} registered successfully with ID {UserId}", user.Email, user.Id);
            return result;
        }
    }
}
