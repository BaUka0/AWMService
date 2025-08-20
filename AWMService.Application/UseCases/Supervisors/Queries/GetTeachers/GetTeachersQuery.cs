using AWMService.Application.DTOs;
using KDS.Primitives.FluentResult;
using MediatR;
using System.Collections.Generic;

namespace AWMService.Application.UseCases.Supervisors.Queries.GetTeachers
{
    public sealed record GetTeachersQuery : IRequest<Result<IEnumerable<TeacherDto>>>
    {
        public int DepartmentId { get; set; }
        public int AcademicYearId { get; set; }
    }
}
