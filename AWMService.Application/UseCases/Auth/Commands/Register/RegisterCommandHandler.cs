using AWMService.Application.Abstractions;
using AWMService.Application.Abstractions.Services;
using AWMService.Application.DTOs;
using AWMService.Domain.Constatns;
using AWMService.Domain.Entities;
using KDS.Primitives.FluentResult;
using MediatR;

namespace AWMService.Application.UseCases.Auth.Commands.Register
{
    internal class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<AuthResult>>
    {
        private readonly IUsersRepository _usersRepository;
        private readonly ITokenService _tokenService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtSettings _jwtSettings;

        public RegisterCommandHandler(
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

        public async Task<Result<AuthResult>> Handle(RegisterCommand request, CancellationToken ct)
        {
            var exists = await _usersRepository.GetByEmailAsync(request.Email, ct);
            if (exists != null)
                return Result.Failure<AuthResult>(new Error(ErrorCode.Conflict, "User with this email already exists"));

            var user = new Users
            {
                CreatedOn = DateTime.UtcNow,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Login = request.Email,
                PasswordHash = _passwordHasher.HashPassword(request.Password),
                UserTypeId = 1 //TODO: надо переделать так чтобы данные брались из request-а
            };

            await _unitOfWork.BeginTransactionAsync(ct);
            try
            {
                await _usersRepository.AddUserAsync(user, ct);
                await _unitOfWork.CommitAsync(ct);

                await _unitOfWork.BeginTransactionAsync(ct);
                try
                {
                    foreach (var roleId in request.RoleIds)
                    {
                        user.UserRoles.Add(new UserRoles
                        {
                            UserId = user.Id,
                            RoleId = roleId,
                            AssignedOn = DateTime.UtcNow,
                            AssignedBy = 0
                        });
                    }

                    await _unitOfWork.CommitAsync(ct);
                }
                catch
                {
                    await _unitOfWork.RollbackAsync(ct);
                    return Result.Failure<AuthResult>(new Error(ErrorCode.InternalServerError, "Failed to create userrole relations"));
                }
            }
            catch
            {
                await _unitOfWork.RollbackAsync(ct);
                return Result.Failure<AuthResult>(new Error(ErrorCode.InternalServerError, "Failed to create user"));
            }

            var savedUser = await _usersRepository.GetByEmailWithRolesAsync(request.Email, ct);
            if (savedUser == null)
                return Result.Failure<AuthResult>(new Error(ErrorCode.NotFound, "User not found"));

            var roles = savedUser.UserRoles.Select(ur => ur.Role.Name);
            var permissions = savedUser.UserRoles
                .SelectMany(ur => ur.Role.RolePermissions)
                .Select(rp => rp.Permission.Name)
                .Distinct();

            var accessToken = _tokenService.GenerateAccessToken(savedUser, roles, permissions);
            var refreshToken = _tokenService.GenerateRefreshToken();

            await _unitOfWork.BeginTransactionAsync(ct);
            try
            {
                savedUser.RefreshToken = refreshToken;
                savedUser.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationInDays);
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
                    Id = savedUser.Id,
                    FirstName = savedUser.FirstName,
                    LastName = savedUser.LastName,
                    Email = savedUser.Email,
                    Roles = roles.ToList()
                }
            };

            return result;
        }
    }
}
