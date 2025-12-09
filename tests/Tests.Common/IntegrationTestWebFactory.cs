using System.Collections.Generic;
using DotNet.Testcontainers.Builders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
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
        Environment.SetEnvironmentVariable("Testing__SkipMigrations", "false");
    }

    public async Task InitializeAsync()
    {
        if (IsDockerAvailable && _dbContainer is not null)
            await _dbContainer.StartAsync();

        using var scope = Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await db.Database.MigrateAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        if (!IsDockerAvailable)
            return;

        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Testing:UseInMemoryDatabase"] = "false",
                ["Testing:SkipMigrations"] = "false"
            });
        });

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<ApplicationDbContext>();
            services.RemoveAll<DbContextOptions<ApplicationDbContext>>();

            var dataSourceBuilder = new NpgsqlDataSourceBuilder(_dbContainer!.GetConnectionString());
            dataSourceBuilder.EnableDynamicJson();
            var dataSource = dataSourceBuilder.Build();

            services.AddDbContext<ApplicationDbContext>(options => options
                .UseNpgsql(dataSource, cfg =>
                    cfg.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
                .UseSnakeCaseNamingConvention());
        });
    }

    protected override void ConfigureClient(HttpClient client)
    {
        base.ConfigureClient(client);
    }

    public async Task DisposeAsync()
    {
        if (IsDockerAvailable && _dbContainer is not null)
            await _dbContainer.DisposeAsync();
    }
}
