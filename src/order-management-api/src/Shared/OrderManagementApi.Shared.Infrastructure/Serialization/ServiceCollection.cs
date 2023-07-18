using OrderManagementApi.Shared.Abstractions.Serialization;
using OrderManagementApi.Shared.Infrastructure.Serialization.SystemTextJson;
using Microsoft.Extensions.DependencyInjection;

namespace OrderManagementApi.Shared.Infrastructure.Serialization;

public static class ServiceCollection
{
    public static void AddJsonSerialization(this IServiceCollection services)
    {
        services.AddSingleton<IJsonSerializer, SystemTextJsonSerializer>();
    }
}