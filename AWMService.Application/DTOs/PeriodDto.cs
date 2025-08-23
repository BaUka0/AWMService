
namespace AWMService.Application.DTOs
{
    public record PeriodDto(
        int Id,
        int PeriodTypeId,
        string PeriodTypeName,
        int AcademicYearId,
        string AcademicYearName,
        DateTime StartDate,
        DateTime EndDate,
        int StatusId,
        string StatusName
    );
}
