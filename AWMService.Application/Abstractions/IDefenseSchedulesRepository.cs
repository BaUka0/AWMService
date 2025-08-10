using AWMService.Domain.Entities;


namespace AWMService.Application.Abstractions
{
    public interface IDefenseSchedulesRepository
    {
        public sealed record DefenseSlotCreate(int StudentWorkId, DateTime DefenseDate, string Location);

 
            Task<DefenseSchedules?> GetDefenseScheduleByIdAsync(int id, CancellationToken ct);
            Task<IReadOnlyList<DefenseSchedules>> ListByCommissionAsync(int commissionId, CancellationToken ct);
            Task<IReadOnlyList<DefenseSchedules>> ListByDateAsync(DateTime date, CancellationToken ct); 


            Task AddSlotAsync(int commissionId, int studentWorkId, DateTime defenseDate, string location, int actorUserId, CancellationToken ct);
            Task AddSlotsAsync(int commissionId, IEnumerable<DefenseSlotCreate> slots, int actorUserId, CancellationToken ct);
            Task RescheduleAsync(int id, DateTime newDate, string? newLocation, int actorUserId, CancellationToken ct);
            Task CancelSlotAsync(int id, int actorUserId, CancellationToken ct);
        }
    }

