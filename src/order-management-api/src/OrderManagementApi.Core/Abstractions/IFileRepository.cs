using OrderManagementApi.Domain.Entities;

namespace OrderManagementApi.Core.Abstractions;

public interface IFileRepository
{
    Task<FileRepository?> GetFileBydIdAsync(Guid fileRepositoryId, CancellationToken cancellationToken);
    Task<bool> FileExistsAsync(Guid fileRepositoryId, string @for, CancellationToken cancellationToken);
}