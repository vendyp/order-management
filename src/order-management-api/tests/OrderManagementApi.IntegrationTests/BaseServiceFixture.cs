using OrderManagementApi.Domain;
using OrderManagementApi.Domain.Entities;
using OrderManagementApi.Domain.Extensions;
using OrderManagementApi.Shared.Abstractions.Clock;
using OrderManagementApi.Shared.Abstractions.Databases;
using OrderManagementApi.Shared.Abstractions.Encryption;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderManagementApi.Persistence.SqlServer;

namespace OrderManagementApi.IntegrationTests;

public abstract class BaseServiceFixture : IDisposable
{
    private string SqlLiteDbName { get; }
    public ServiceProvider ServiceProvider { get; }

    protected BaseServiceFixture(string name)
    {
        SqlLiteDbName = $"{name}TestsDb";
        var services = new Microsoft.Extensions.DependencyInjection.ServiceCollection();
        services.AddDefaultInjectedServices();

        services.AddDbContext<SqlServerDbContext>(e =>
            e.UseSqlite($"Data Source={SqlLiteDbName}")
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
        services.AddScoped<IDbContext>(serviceProvider => serviceProvider.GetRequiredService<SqlServerDbContext>());

        ServiceProvider = services.BuildServiceProvider();

        var dbContext = ServiceProvider.GetRequiredService<SqlServerDbContext>();
        dbContext.Database.EnsureCreated();
        if (dbContext.Set<User>().Any(e => e.UserId == Guid.Empty)) return;
        var salter = ServiceProvider.GetRequiredService<ISalter>();
        var rng = ServiceProvider.GetRequiredService<IRng>();
        var clock = ServiceProvider.GetRequiredService<IClock>();
        dbContext.Insert(new Role(RoleExtensions.AdministratorId, "Administrator"));
        dbContext.Insert(new Role(RoleExtensions.SuperAdministratorId, "Super Administrator"));
        dbContext.Insert(DefaultUser.SuperAdministrator(rng, salter, clock));
        dbContext.SaveChangesAsync().GetAwaiter().GetResult();
    }

    public void Dispose()
    {
        var dbContext = ServiceProvider.GetRequiredService<SqlServerDbContext>();
        dbContext.Database.EnsureDeleted();
        ServiceProvider.Dispose();
    }
}