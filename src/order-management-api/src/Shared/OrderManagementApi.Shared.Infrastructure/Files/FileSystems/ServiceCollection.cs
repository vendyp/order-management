using OrderManagementApi.Shared.Abstractions.Files;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace OrderManagementApi.Shared.Infrastructure.Files.FileSystems;

public static class ServiceCollection
{
    public static void AddFileSystemService(this IServiceCollection services, string name = "fileOptions")
    {
        var options = services.GetOptions<FileSystemOptions>(name);
        services.TryAddSingleton(options);
        services.TryAddSingleton<IFileService, FileSystemService>();
    }
}