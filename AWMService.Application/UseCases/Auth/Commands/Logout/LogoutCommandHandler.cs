using AWMService.Application.Abstractions;
using AWMService.Domain.Constatns;
using KDS.Primitives.FluentResult;
using MediatR;

namespace AWMService.Application.UseCases.Auth.Commands.Logout
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Result>
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IUnitOfWork _unitOfWork;

        public LogoutCommandHandler(IUsersRepository usersRepository, IUnitOfWork unitOfWork)
        {
            _usersRepository = usersRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(LogoutCommand request, CancellationToken ct)
        {
            var user = await _usersRepository.GetByIdAsync(request.UserId, ct);

            if (user == null)
            {
                return Result.Failure(new Error(ErrorCode.NotFound, "User not found"));
            }

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;

            await _unitOfWork.BeginTransactionAsync(ct);
            try
            {
                await _unitOfWork.CommitAsync(ct);
            }
            catch
            {
                await _unitOfWork.RollbackAsync(ct);
                return Result.Failure(new Error(ErrorCode.InternalServerError, "Failed to logout user."));
            }

            return Result.Success();
        }
    }
}
