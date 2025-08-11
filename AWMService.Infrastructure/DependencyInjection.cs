using AWMService.Application.Abstractions;
using AWMService.Infrastructure.Data;
using AWMService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(connectionString));
       
       
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
        services.AddScoped<IUnitOfWork, UnitOfWork>();


        return services;
    }
}

