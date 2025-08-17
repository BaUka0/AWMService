using AWMService.Application.Abstractions.Repositories;
using AWMService.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AWMService.WebAPI.Controllers
{
    [ApiController]
    [Route("api/supervisors")]
    [Authorize(Policy = "ManageSupervisors")] 
    public class SupervisorsController : ControllerBase
    {
        private readonly AppDbContext _ctx;
        private readonly ISupervisorApprovalsRepository _approvals;
        //private readonly INotificationService _notifications;

       /* public SupervisorsController(AppDbContext ctx, ISupervisorApprovalsRepository approvals, INotificationService notifications)
        {
            _ctx = ctx;
            _approvals = approvals;
            _notifications = notifications;
        }*/
        
        public record TeacherDto(int UserId, string FullName, string? Email, bool IsApproved);
        public class ApproveSupervisorsRequest
        {
            public int DepartmentId { get; set; }
            public int AcademicYearId { get; set; }
            public List<int> UserIds { get; set; } = new();
            public bool Notify { get; set; } = true;
        }
        public class RevokeSupervisorsRequest
        {
            public int DepartmentId { get; set; }
            public int AcademicYearId { get; set; }
            public List<int> UserIds { get; set; } = new();
        }
        
        [HttpGet("departments/{departmentId:int}/teachers")]
        public async Task<ActionResult<IEnumerable<TeacherDto>>> GetTeachers(
            int departmentId,
            [FromQuery] int academicYearId,
            CancellationToken ct)
        {
            var teachers = await (from u in _ctx.Users.AsNoTracking()
                                  join ut in _ctx.UserTypes.AsNoTracking() on u.UserTypeId equals ut.Id
                                  where u.DepartmentId == departmentId &&
                                        (ut.Name == "преподаватель" || ut.Name == "teacher")
                                  orderby u.LastName, u.FirstName
                                  select new { u.Id, u.LastName, u.FirstName, u.SurName, u.Email })
                                  .ToListAsync(ct);

            var approvedIds = (await _approvals
                .ListApprovedUserIdsByDepartmentAndYearAsync(departmentId, academicYearId, ct))
                .ToHashSet();

            var res = teachers.Select(t => new TeacherDto(
                t.Id,
                $"{t.LastName} {t.FirstName}{(string.IsNullOrWhiteSpace(t.SurName) ? "" : " " + t.SurName)}",
                t.Email,
                approvedIds.Contains(t.Id)));

            return Ok(res);
        }
        
        [HttpPost("approvals/bulk")]
        public async Task<IActionResult> ApproveBulk([FromBody] ApproveSupervisorsRequest req, CancellationToken ct)
        {
            if (req.UserIds.Count == 0) return BadRequest("UserIds is empty.");
            
            var teacherIds = await (from u in _ctx.Users.AsNoTracking()
                                    join ut in _ctx.UserTypes.AsNoTracking() on u.UserTypeId equals ut.Id
                                    where u.DepartmentId == req.DepartmentId &&
                                          (ut.Name == "преподаватель" || ut.Name == "teacher") &&
                                          req.UserIds.Contains(u.Id)
                                    select u.Id).ToListAsync(ct);

            var notTeachers = req.UserIds.Except(teacherIds).ToArray();
            if (notTeachers.Length > 0)
                return BadRequest($"Следующие пользователи не являются преподавателями указанной кафедры: {string.Join(",", notTeachers)}");
            
            var actorId = GetActorUserId();
            using var tx = await _ctx.Database.BeginTransactionAsync(ct);

            foreach (var uid in req.UserIds.Distinct())
                await _approvals.ApproveAsync(uid, req.DepartmentId, req.AcademicYearId, actorId, ct);

            await _ctx.SaveChangesAsync(ct);
            await tx.CommitAsync(ct);
            
            if (req.Notify)
            {
                var recipients = await _ctx.Users.AsNoTracking()
                    .Where(u => req.UserIds.Contains(u.Id))
                    .Select(u => new ValueTuple<int, string?>(u.Id, u.Email))
                    .ToListAsync(ct);

                //await _notifications.NotifySupervisorsApprovedAsync(recipients, req.DepartmentId, req.AcademicYearId, ct);
            }

            return NoContent();
        }
        
        [HttpDelete("approvals/bulk")]
        public async Task<IActionResult> RevokeBulk([FromBody] RevokeSupervisorsRequest req, CancellationToken ct)
        {
            if (req.UserIds.Count == 0) return BadRequest("UserIds is empty.");

            var actorId = GetActorUserId();
            using var tx = await _ctx.Database.BeginTransactionAsync(ct);

            foreach (var uid in req.UserIds.Distinct())
                await _approvals.RevokeAsync(uid, req.DepartmentId, req.AcademicYearId, actorId, ct);

            await _ctx.SaveChangesAsync(ct);
            await tx.CommitAsync(ct);

            return NoContent();
        }

        private int GetActorUserId()
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == "sub" || c.Type == "uid");
            if (claim == null || !int.TryParse(claim.Value, out var id))
                throw new UnauthorizedAccessException("Actor user id is missing.");
            return id;
        }
    }
}
