using AWMService.Application.Abstractions;
using AWMService.Application.Abstractions.Data;
using AWMService.Application.Abstractions.Notification;
using AWMService.Application.Abstractions.Notification.Security;
using AWMService.Application.Abstractions.Data;
using AWMService.Application.Abstractions.Repositories;
using AWMService.Application.Abstractions.Services;
using AWMService.Infrastructure.Data;
using AWMService.Infrastructure.Hubs;
using AWMService.Infrastructure.RealTimeNotification;
using AWMService.Infrastructure.RealTimeNotification.Security;
using AWMService.Infrastructure.Repositories;
using AWMService.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));

        // Configure JwtSettings
        var jwtSettings = new JwtSettings();
        configuration.GetSection("JwtSettings").Bind(jwtSettings);
        services.AddSingleton<IJwtSettings>(jwtSettings);
        services.AddSingleton(jwtSettings);

        // JWT Authentication
        var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtSettings.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                NameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
                RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
            };
        });

        services.AddAuthorization();

        // Repositories
        services.AddScoped<IAcademicYearsRepository, AcademicYearsRepository>();
        services.AddScoped<IApplicationsRepository, ApplicationsRepository>();
        services.AddScoped<ICheckTypesRepository, CheckTypesRepository>();
        services.AddScoped<ICommissionMembersRepository, CommissionMembersRepository>();
        services.AddScoped<ICommissionsRepository, CommissionsRepository>();
        services.AddScoped<ICommissionTypesRepository, CommissionTypesRepository>();
        services.AddScoped<IDefenseGradesRepository, DefenseGradesRepository>();
        services.AddScoped<IDefenseSchedulesRepository, DefenseSchedulesRepository>();
        services.AddScoped<IDepartmentExpertsRepository, DepartmentExpertsRepository>();
        services.AddScoped<IDepartmentsRepository, DepartmentsRepository>();
        services.AddScoped<IDirectionsRepository, DirectionsRepository>();
        services.AddScoped<IExternalContactsRepository, ExternalContactsRepository>();
        services.AddScoped<IInstitutesRepository, InstitutesRepository>();
        services.AddScoped<INotificationsRepository, NotificationsRepository>();
        services.AddScoped<IPeriodsRepository, PeriodsRepository>();
        services.AddScoped<IPeriodTypesRepository, PeriodTypesRepository>();
        services.AddScoped<IPermissionsRepository, PermissionsRepository>();
        services.AddScoped<IRolesRepository, RolesRepository>();
        services.AddScoped<ISettingsRepository, SettingsRepository>();
        services.AddScoped<IStatusesRepository, StatusesRepository>();
        services.AddScoped<IStudentWorkRepository, StudentWorkRepository>();
        services.AddScoped<ISupervisorApprovalsRepository, SupervisorApprovalsRepository>();
        services.AddScoped<ITopicsRepository, TopicsRepository>();
        services.AddScoped<IUsersRepository, UsersRepository>();
        services.AddScoped<IUserTypesRepository, UserTypesRepository>();
        services.AddScoped<IWorkChecksRepository, WorkChecksRepository>();
        services.AddScoped<IWorkTypesRepository, WorkTypesRepository>();
        services.AddScoped<IEvaluationCriteriaRepository, EvaluationCriteriaRepository>();
        services.AddScoped<IEvaluationScoresRepository, EvaluationScoresRepository>();
        services.AddScoped<IAttachmentsRepository, AttachmentsRepository>();
        services.AddScoped<INotificationAccessService, NotificationAccessService>();
        // Services
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
        services.AddScoped<ITokenService, JwtTokenService>();
        // Notification
        services.AddSignalR();
        services.AddSingleton<IUserIdProvider, JwtSubAsUserIdProvider>();
        services.AddScoped<IRealTimeNotification, SignalRRealtimeNotifier>();
        services.AddScoped<IAfterCommitQueue, AfterCommitQueue>();

        return services;
    }
}

