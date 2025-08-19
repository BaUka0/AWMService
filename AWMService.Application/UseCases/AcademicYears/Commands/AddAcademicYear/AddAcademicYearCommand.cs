using KDS.Primitives.FluentResult;
using MediatR;
using System;

namespace AWMService.Application.UseCases.AcademicYears.Commands.AddAcademicYear
{
    public sealed class AddAcademicYearCommand : IRequest<Result>
    {
        public string YearName { get; set; } = default!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate   { get; set; }
        public int ActorUserId    { get; set; }
    }
}