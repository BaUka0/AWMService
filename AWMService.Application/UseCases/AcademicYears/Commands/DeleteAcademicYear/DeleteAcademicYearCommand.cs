using KDS.Primitives.FluentResult;
using MediatR;

namespace AWMService.Application.UseCases.AcademicYears.Commands.DeleteAcademicYear
{
    public sealed record DeleteAcademicYearCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public int ActorUserId { get; set; }
    }
}