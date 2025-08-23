using KDS.Primitives.FluentResult;
using MediatR;

namespace AWMService.Application.UseCases.Periods.Commands.UpdatePeriod
{
    public sealed record UpdatePeriodCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public int PeriodTypeId { get; set; }
        public int AcademicYearId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int StatusId { get; set; }
        public int ActorUserId { get; set; }
    }
}
