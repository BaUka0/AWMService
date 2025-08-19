using AWMService.Application.Abstractions.Repositories;
using AWMService.Application.DTOs;
using KDS.Primitives.FluentResult;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AWMService.Application.UseCases.Supervisors.Queries.GetTeachers
{
    public class GetTeachersQueryHandler : IRequestHandler<GetTeachersQuery, Result<IEnumerable<TeacherDto>>>
    {
        private readonly IUsersRepository _usersRepository;
        private readonly ISupervisorApprovalsRepository _approvalsRepository;
        private readonly ILogger<GetTeachersQueryHandler> _logger;

        public GetTeachersQueryHandler(
            IUsersRepository usersRepository,
            ISupervisorApprovalsRepository approvalsRepository,
            ILogger<GetTeachersQueryHandler> logger)
        {
            _usersRepository = usersRepository;
            _approvalsRepository = approvalsRepository;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<TeacherDto>>> Handle(GetTeachersQuery request, CancellationToken ct)
        {
            _logger.LogInformation("Fetching teachers for DepartmentId: {DepartmentId} and AcademicYearId: {AcademicYearId}", 
                request.DepartmentId, request.AcademicYearId);

            var teachers = await _usersRepository.GetTeachersByDepartmentAsync(request.DepartmentId, ct);

            var approvedIds = (await _approvalsRepository
                .ListApprovedUserIdsByDepartmentAndYearAsync(request.DepartmentId, request.AcademicYearId, ct))
                .ToHashSet();

            var result = teachers.Select(t => new TeacherDto(
                t.Id,
                $"{t.LastName} {t.FirstName}{(string.IsNullOrWhiteSpace(t.SurName) ? "" : " " + t.SurName)}",
                t.Email,
                approvedIds.Contains(t.Id)));

            return Result.Success(result);
        }
    }
}
