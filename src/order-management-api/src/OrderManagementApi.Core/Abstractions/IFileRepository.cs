using OrderManagementApi.Domain.Entities;

namespace OrderManagementApi.Core.Abstractions;

public interface IFileRepository
{
    Task<FileRepository?> GetFileBydId(Guid fileRepositoryId, CancellationToken cancellationToken);
}