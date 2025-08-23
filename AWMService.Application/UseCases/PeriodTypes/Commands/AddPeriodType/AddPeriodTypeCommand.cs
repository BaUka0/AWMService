using KDS.Primitives.FluentResult;
using MediatR;

namespace AWMService.Application.UseCases.PeriodType.Commands.AddPeriodType
{
    public sealed record AddPeriodTypeCommand : IRequest<Result<int>>
    {
        public string Name { get; set; } = default!;
        public int ActorUserId { get; set; }
    }
}
