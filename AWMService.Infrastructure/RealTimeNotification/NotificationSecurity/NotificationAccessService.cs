using AWMService.Application.Abstractions.Notification.Security;
using AWMService.Domain.Entities;
using AWMService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Globalization;


namespace AWMService.Infrastructure.RealTimeNotification.Security
{
    public class NotificationAccessService : INotificationAccessService
    {
        private readonly AppDbContext _ctx;
        public NotificationAccessService(AppDbContext ctx) => _ctx = ctx;

        public async Task<bool> CanSubscribeAsync(string userId, string entity, string id, CancellationToken ct)
        {
            int? uid = int.TryParse(userId, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedId) ? parsedId : null;

            var e = entity.Trim().ToLowerInvariant();
            var s = id?.Trim();

            if (string.IsNullOrEmpty(s) || string.IsNullOrWhiteSpace(s))
            {
                return false;
            }

            switch (e)
            {
                case "role":
                    return await _ctx.Set<UserRoles>()
                    .Join(_ctx.Set<Roles>(), ur => ur.RoleId, r => r.Id, (ur, r) => new { ur, r })
                    .AnyAsync(x => x.ur.UserId == uid && x.r.Name == s, ct);

                case "work":
                    if (!int.TryParse(s, out var workId)) return false;

                    var work = await (
                    from w in _ctx.Set<StudentWork>()
                    where w.Id == workId
                    join t in _ctx.Set<Topics>() on w.TopicId equals t.Id
                    join student in _ctx.Set<Users>() on w.StudentId equals student.Id
                    join sDept in _ctx.Set<Departments>() on student.DepartmentId equals sDept.Id into sdep
                    from sDept in sdep.DefaultIfEmpty()
                    join supervisor in _ctx.Set<Users>() on t.SupervisorId equals supervisor.Id
                    join supDept in _ctx.Set<Departments>() on supervisor.DepartmentId equals supDept.Id into supdep
                    from supDept in supdep.DefaultIfEmpty()
                    select new
                    {
                        w.Id,
                        w.StudentId,
                        t.SupervisorId,
                        StudentInstituteId = (int?)sDept.InstituteId,
                        SupervisorInstituteId = (int?)supDept.InstituteId
                    })
                    .SingleOrDefaultAsync(ct);

                    if (work is null) return false;

                    
                    var isParticipant = uid == work.StudentId || uid == work.SupervisorId;

                    
                    var isChecker = await _ctx.Set<WorkChecks>()
                        .AnyAsync(c => c.StudentWorkId == workId && c.ExpertId == uid, ct);

                    
                    var userInstituteId = await (
                        from u in _ctx.Set<Users>()
                        where u.Id == uid
                        join d in _ctx.Set<Departments>() on u.DepartmentId equals d.Id
                        select (int?)d.InstituteId
                    ).FirstOrDefaultAsync(ct);

                    var isSecretary = userInstituteId is not null
                        && (userInstituteId == work.StudentInstituteId || userInstituteId == work.SupervisorInstituteId)
                        && await (
                            from ur in _ctx.Set<UserRoles>()
                            join r in _ctx.Set<Roles>() on ur.RoleId equals r.Id
                            where ur.UserId == uid && r.Name == "Secretary"
                            select 1
                        ).AnyAsync(ct);

                    return isParticipant || isChecker || isSecretary;

                case "defense":
                    if (!int.TryParse(s, out var defenseId)) return false;

                    
                    var info = await (
                        from ds in _ctx.Set<DefenseSchedules>()
                        where ds.Id == defenseId
                        join w in _ctx.Set<StudentWork>() on ds.StudentWorkId equals w.Id
                        join t in _ctx.Set<Topics>() on w.TopicId equals t.Id
                        join c in _ctx.Set<Commissions>() on ds.CommissionId equals c.Id
                        join dept in _ctx.Set<Departments>() on c.DepartmentId equals dept.Id
                        select new
                        {
                            w.StudentId,
                            t.SupervisorId,
                            CommissionId = c.Id,
                            CommissionSecretaryId = c.SecretaryId,
                            dept.InstituteId
                        })
                        .SingleOrDefaultAsync(ct);

                    if (info is null) return false;

                    var isStudentOrSupervisor = uid == info.StudentId || uid == info.SupervisorId;
                    var isCommissionMember = await _ctx.Set<CommissionMembers>()
                        .AnyAsync(cm => cm.CommissionId == info.CommissionId && cm.MemberId == uid, ct);
                    var isCommissionSecretary = uid == info.CommissionSecretaryId;

                   
                    var userInst = await (
                        from u in _ctx.Set<Users>()
                        where u.Id == uid
                        join d in _ctx.Set<Departments>() on u.DepartmentId equals d.Id
                        select d.InstituteId
                    ).FirstOrDefaultAsync(ct);

                    var isInstituteSecretary = userInst == info.InstituteId
                        && await (
                            from ur in _ctx.Set<UserRoles>()
                            join r in _ctx.Set<Roles>() on ur.RoleId equals r.Id
                            where ur.UserId == uid && r.Name == "Secretary"
                            select 1
                        ).AnyAsync(ct);

                    return isStudentOrSupervisor || isCommissionMember || isCommissionSecretary || isInstituteSecretary;

                default:
                    return false;




            }
        }
    }
}
