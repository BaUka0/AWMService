using AWMService.Application.DTOs;
using KDS.Primitives.FluentResult;
using MediatR;

namespace AWMService.Application.UseCases.Auth.Commands.Register
{
    public class RegisterCommand : IRequest<Result<AuthResult>>
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public List<int> RoleIds { get; set; } = new List<int>();
        public int UserTypeId { get; set; }
    }
}
