using OrderManagementApi.Core.Modules;
using Microsoft.Extensions.DependencyInjection;

namespace OrderManagementApi.Core;

public static class ServiceCollection
{
    public static void AddCore(this IServiceCollection services)
    {
        services.AddSingleton<ModuleManager>();
    }
}