using OrderManagementApi.Core.Abstractions;
using OrderManagementApi.Infrastructure.Services;
using OrderManagementApi.IntegrationTests.Dependencies;
using OrderManagementApi.Shared.Abstractions.Clock;
using OrderManagementApi.Shared.Abstractions.Contexts;
using OrderManagementApi.Shared.Abstractions.Encryption;
using OrderManagementApi.Shared.Abstractions.Files;
using OrderManagementApi.Shared.Infrastructure.Cache;
using OrderManagementApi.Shared.Infrastructure.Clock;
using OrderManagementApi.Shared.Infrastructure.Encryption;
using OrderManagementApi.Shared.Infrastructure.Files.FileSystems;
using OrderManagementApi.Shared.Infrastructure.Serialization;
using OrderManagementApi.UnitTests.Builders;
using OrderManagementApi.WebApi.Common;
using Microsoft.Extensions.DependencyInjection;

namespace OrderManagementApi.IntegrationTests;

public static class DependencyInjection
{
    public static void AddDefaultInjectedServices(this IServiceCollection services)
    {
        services.AddScoped<IContext>(_ => ContextBuilder.Create().Object);
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthManager, Dependencies.AuthManager>();
        services.AddSingleton<IFileService, FileSystemServiceMock>();
        services.AddInternalMemoryCache();
        services.AddJsonSerialization();
        services.AddSingleton<IClock, Clock>();
        services.AddSingleton<ISalter, Salter>();
        services.AddEncryption();
        services.AddSingleton(new ClockOptions());
        services.AddScoped<IFileRepository, FileRepositoryService>();
    }
}