using KDS.Primitives.FluentResult;
using MediatR;
using System.Collections.Generic;

namespace AWMService.Application.UseCases.Supervisors.Commands.ApproveSupervisors
{
    public class ApproveSupervisorsCommand : IRequest<Result>
    {
        public int DepartmentId { get; set; }
        public int AcademicYearId { get; set; }
        public List<int> UserIds { get; set; } = new();
        public bool Notify { get; set; } = true;
        public int ActorUserId { get; set; }
    }
}
