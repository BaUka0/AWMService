using AWMService.Domain.Constatns;
using AWMService.Application.Abstractions;
using AWMService.Application.Abstractions.Services;
using AWMService.Application.DTOs;
using KDS.Primitives.FluentResult;
using MediatR;

namespace AWMService.Application.UseCases.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthResult>>
    {
        private readonly IUsersRepository _usersRepository;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtSettings _jwtSettings;

        public LoginCommandHandler(
            IUsersRepository usersRepository, 
            ITokenService tokenService, 
            IPasswordHasher passwordHasher, 
            IUnitOfWork unitOfWork,
            IJwtSettings jwtSettings)
        {
            _usersRepository = usersRepository;
            _tokenService = tokenService;
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
            _jwtSettings = jwtSettings;
        }

        public async Task<Result<AuthResult>> Handle(LoginCommand request, CancellationToken ct)
        {
            var user = await _usersRepository.GetByEmailWithRolesAsync(request.Email, ct);

            if (user == null)
                return Result.Failure<AuthResult>(new Error(ErrorCode.NotFound, "User not found"));

            if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
                return Result.Failure<AuthResult>(new Error(ErrorCode.Unauthorized, "Invalid password"));

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
            catch
            {
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
                    Roles = roles.ToList(),
                }
            };

            return Result.Success(result);
        }
    }
}
