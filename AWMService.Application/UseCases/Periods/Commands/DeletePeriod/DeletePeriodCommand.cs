using KDS.Primitives.FluentResult;
using MediatR;

namespace AWMService.Application.UseCases.Periods.Commands.DeletePeriod
{
    public sealed record DeletePeriodCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public int ActorUserId { get; set; }
    }
}
