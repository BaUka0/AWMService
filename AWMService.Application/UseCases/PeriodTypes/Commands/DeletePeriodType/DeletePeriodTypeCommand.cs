using KDS.Primitives.FluentResult;
using MediatR;

namespace AWMService.Application.UseCases.PeriodType.Commands.DeletePeriodType
{
    public sealed record DeletePeriodTypeCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public int ActorUserId { get; set; }
    }
}
