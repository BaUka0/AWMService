namespace AWMService.Application.DTOs
{
    public record TeacherDto(int UserId, string FullName, string? Email, bool IsApproved);
}
