using AWMService.Application.Abstractions;
using AWMService.Application.Abstractions.Services;
using AWMService.Application.DTOs;
using AWMService.Domain.Constatns;
using KDS.Primitives.FluentResult;
using MediatR;

namespace AWMService.Application.UseCases.Auth.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<AuthResult>>
    {
        private readonly IUsersRepository _usersRepository;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtSettings _jwtSettings;

        public RefreshTokenCommandHandler(
            IUsersRepository usersRepository, 
            ITokenService tokenService, 
            IUnitOfWork unitOfWork, 
            IJwtSettings jwtSettings)
        {
            _usersRepository = usersRepository;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
            _jwtSettings = jwtSettings;
        }

        public async Task<Result<AuthResult>> Handle(RefreshTokenCommand request, CancellationToken ct)
        {
            var user = await _usersRepository.GetByRefreshTokenAsync(request.RefreshToken, ct);

            if (user == null)
                return Result.Failure<AuthResult>(new Error(ErrorCode.Unauthorized, "Invalid refresh token."));

            if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                return Result.Failure<AuthResult>(new Error(ErrorCode.Unauthorized, "Refresh token expired."));
            
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
            catch
            {
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

            return result;
        }
    }
}
