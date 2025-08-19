using KDS.Primitives.FluentResult;
using MediatR;
using System.Collections.Generic;

namespace AWMService.Application.UseCases.Supervisors.Commands.RevokeSupervisors
{
    public class RevokeSupervisorsCommand : IRequest<Result>
    {
        public int DepartmentId { get; set; }
        public int AcademicYearId { get; set; }
        public List<int> UserIds { get; set; } = new();
        public int ActorUserId { get; set; }
    }
}
