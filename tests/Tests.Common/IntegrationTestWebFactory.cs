using System.Collections.Generic;
using System.Linq;
using DotNet.Testcontainers.Builders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using Testcontainers.PostgreSql;
using UCMS.Infrastructure.Persistence;
using Xunit;

namespace Tests.Common;

public sealed class IntegrationTestWebFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer? _dbContainer;

    public bool IsDockerAvailable { get; }

    public IntegrationTestWebFactory()
    {
        try
        {
            _dbContainer = new PostgreSqlBuilder()
                .WithImage("postgres:latest")
                .WithDatabase("ucms-tests")
                .WithUsername("postgres")
                .WithPassword("postgres")
                .Build();
            IsDockerAvailable = true;
        }
        catch (DockerUnavailableException)
        {
            IsDockerAvailable = false;
        }

        Environment.SetEnvironmentVariable("Testing__UseInMemoryDatabase", (!IsDockerAvailable).ToString());
        Environment.SetEnvironmentVariable("Testing__SkipMigrations", (!IsDockerAvailable).ToString());
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((_, configurationBuilder) =>
        {
            configurationBuilder
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["Testing:SkipMigrations"] = (!IsDockerAvailable).ToString(),
                    ["Testing:UseInMemoryDatabase"] = (!IsDockerAvailable).ToString()
                })
                .AddEnvironmentVariables();
        });

        if (!IsDockerAvailable)
        {
            return;
        }

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<ApplicationDbContext>();
            services.RemoveAll<DbContextOptions<ApplicationDbContext>>();

            var dataSourceBuilder = new NpgsqlDataSourceBuilder(_dbContainer!.GetConnectionString());
            dataSourceBuilder.EnableDynamicJson();
            var dataSource = dataSourceBuilder.Build();

            services.AddDbContext<ApplicationDbContext>(options => options
                .UseNpgsql(dataSource, cfg => cfg.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
                .UseSnakeCaseNamingConvention()
                .ConfigureWarnings(w => w.Ignore(CoreEventId.ManyServiceProvidersCreatedWarning)));
        });
    }

    public Task InitializeAsync()
        => IsDockerAvailable && _dbContainer is not null
            ? _dbContainer.StartAsync()
            : Task.CompletedTask;

    public new Task DisposeAsync()
        => IsDockerAvailable && _dbContainer is not null
            ? _dbContainer.DisposeAsync().AsTask()
            : Task.CompletedTask;
}

public static class ServiceCollectionExtensions
{
    public static void RemoveServiceByType(this IServiceCollection services, Type serviceType)
    {
        var descriptors = services.Where(s => s.ServiceType == serviceType).ToList();
        foreach (var descriptor in descriptors)
        {
            services.Remove(descriptor);
        }
    }
}
