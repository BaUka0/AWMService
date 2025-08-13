namespace AWMService.Application.Abstractions.Services
{
    public interface IJwtSettings
    {
        int AccessTokenExpirationInMinutes { get; }
        int RefreshTokenExpirationInDays { get; }
    }
}