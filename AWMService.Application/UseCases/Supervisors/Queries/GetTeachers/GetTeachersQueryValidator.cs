using FluentValidation;

namespace AWMService.Application.UseCases.Supervisors.Queries.GetTeachers
{
    public class GetTeachersQueryValidator : AbstractValidator<GetTeachersQuery>
    {
        public GetTeachersQueryValidator()
        {
            RuleFor(x => x.DepartmentId)
                .GreaterThan(0).WithMessage("DepartmentId must be greater than 0.");

            RuleFor(x => x.AcademicYearId)
                .GreaterThan(0).WithMessage("AcademicYearId must be greater than 0.");
        }
    }
}