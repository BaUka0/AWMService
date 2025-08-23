using AWMService.Application.Abstractions.Repositories;
using AWMService.Application.DTOs;
using KDS.Primitives.FluentResult;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AWMService.Application.UseCases.Supervisors.Queries.GetTeachers
{
    public class GetTeachersQueryHandler(
        IUsersRepository usersRepository,
        ISupervisorApprovalsRepository approvalsRepository,
        ILogger<GetTeachersQueryHandler> logger) : IRequestHandler<GetTeachersQuery, Result<IEnumerable<TeacherDto>>>
    {
        public async Task<Result<IEnumerable<TeacherDto>>> Handle(GetTeachersQuery request, CancellationToken ct)
        {
            using var scope = logger.BeginScope(new Dictionary<string, object> { ["DepartmentId"] = request.DepartmentId, ["AcademicYearId"] = request.AcademicYearId });
            logger.LogInformation("Fetching teachers for DepartmentId: {DepartmentId} and AcademicYearId: {AcademicYearId}", 
                request.DepartmentId, request.AcademicYearId);

            var teachers = await usersRepository.GetTeachersByDepartmentAsync(request.DepartmentId, ct);

            var approvedIds = (await approvalsRepository
                .ListApprovedUserIdsByDepartmentAndYearAsync(request.DepartmentId, request.AcademicYearId, ct))
                .ToHashSet();

            var result = teachers.Select(t => new TeacherDto(
                t.Id,
                $"{t.LastName} {t.FirstName}{(string.IsNullOrWhiteSpace(t.SurName) ? "" : " " + t.SurName)}",
                t.Email,
                approvedIds.Contains(t.Id))).ToList();

            return result;
        }
    }
}
