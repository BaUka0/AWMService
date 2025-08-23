using KDS.Primitives.FluentResult;
using MediatR;

namespace AWMService.Application.UseCases.PeriodType.Commands.UpdatePeriodType
{
    public sealed record UpdatePeriodTypeCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public int ActorUserId { get; set; }
    }
}
