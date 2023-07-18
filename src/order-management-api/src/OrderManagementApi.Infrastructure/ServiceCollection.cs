using System.Runtime.CompilerServices;
using OrderManagementApi.Core;
using OrderManagementApi.Core.Abstractions;
using OrderManagementApi.Infrastructure.Services;
using OrderManagementApi.Persistence.SqlServer;
using OrderManagementApi.Shared.Abstractions.Encryption;
using OrderManagementApi.Shared.Infrastructure;
using OrderManagementApi.Shared.Infrastructure.Api;
using OrderManagementApi.Shared.Infrastructure.Clock;
using OrderManagementApi.Shared.Infrastructure.Contexts;
using OrderManagementApi.Shared.Infrastructure.Encryption;
using OrderManagementApi.Shared.Infrastructure.Files.FileSystems;
using OrderManagementApi.Shared.Infrastructure.Initializer;
using OrderManagementApi.Shared.Infrastructure.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("OrderManagementApi.UnitTests")]

namespace OrderManagementApi.Infrastructure;

public static class ServiceCollection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCore();
        services.AddSharedInfrastructure();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IEmailService, EmailSenderService>();
        services.AddSingleton<ISalter, Salter>();

        services.AddFileSystemService();
        services.AddJsonSerialization();
        services.AddClock();
        services.AddContext();
        services.AddEncryption();
        services.AddCors();

        services.AddCorsPolicy();
        
        services.AddApplicationInitializer();
        services.AddInitializer<AutoMigrationService>();
        services.AddInitializer<CoreInitializer>();

        services.AddSqlServerDbContext(configuration, "sqlserver");
    }
}