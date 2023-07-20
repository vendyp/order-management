using OrderManagementApi.Domain.Entities;

namespace OrderManagementApi.Core.Abstractions;

public interface IFileRepository
{
    Task<FileRepository?> GetFileBydIdAsync(Guid fileRepositoryId, CancellationToken cancellationToken);
    Task UpdateFileRepositorySourceAsync(Guid fileRepositoryId, string source, CancellationToken cancellationToken);
}