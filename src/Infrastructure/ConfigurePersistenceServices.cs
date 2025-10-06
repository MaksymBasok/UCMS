using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using UCMS.Application.Abstractions;
using UCMS.Application.Abstractions.Queries;
using UCMS.Application.Abstractions.Repositories;
using UCMS.Infrastructure.Persistence;
using UCMS.Infrastructure.Queries;
using UCMS.Infrastructure.Repositories;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using EFCore.NamingConventions;


namespace UCMS.Infrastructure;

public static class ConfigurePersistenceServices
{
    public static void AddPersistenceServices(this IServiceCollection services, IConfiguration cfg)
    {
        var cs = cfg.GetConnectionString("Postgres")
                 ?? "Host=localhost;Port=5432;Database=ucms_db;Username=postgres;Password=4321";

        var dsb = new NpgsqlDataSourceBuilder(cs);
        dsb.EnableDynamicJson();
        var dataSource = dsb.Build();

        services.AddDbContext<ApplicationDbContext>(opt => opt
    .UseNpgsql(dataSource, b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
    .UseSnakeCaseNamingConvention()
    .EnableSensitiveDataLogging()
    .EnableDetailedErrors());

        services.AddScoped<IStudentRepository, StudentRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<ICourseScheduleRepository, CourseScheduleRepository>();
        services.AddScoped<ISubmissionRepository, SubmissionRepository>();

        services.AddScoped<ICourseQueries, CourseQueries>();
        services.AddScoped<ICourseScheduleQueries, CourseScheduleQueries>();
        services.AddScoped<ISubmissionQueries, SubmissionQueries>();
        services.AddScoped<IStudentQueries, StudentQueries>();
    }
}
