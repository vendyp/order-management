namespace OrderManagementApi.Shared.Abstractions.Databases;

public interface IInitializer
{
    Task ExecuteAsync(CancellationToken cancellationToken);
}