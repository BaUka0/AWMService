using AWMService.Application.DTOs;
using KDS.Primitives.FluentResult;
using MediatR;

namespace AWMService.Application.UseCases.Periods.Commands.AddPeriod
{
    public sealed record AddPeriodCommand : IRequest<Result<PeriodDto>>
    {
        public int PeriodTypeId { get; set; }
        public int AcademicYearId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int StatusId { get; set; }
        public int ActorUserId { get; set; }
    }
}
