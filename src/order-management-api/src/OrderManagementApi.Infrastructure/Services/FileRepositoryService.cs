using Microsoft.EntityFrameworkCore;
using OrderManagementApi.Core.Abstractions;
using OrderManagementApi.Domain.Entities;
using OrderManagementApi.Shared.Abstractions.Databases;

namespace OrderManagementApi.Infrastructure.Services;

public class FileRepositoryService : IFileRepository
{
    private readonly IDbContext _dbContext;

    public FileRepositoryService(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<FileRepository?> GetFileBydIdAsync(Guid fileRepositoryId, CancellationToken cancellationToken)
        => _dbContext.Set<FileRepository>()
            .Where(e => e.FileRepositoryId == fileRepositoryId)
            .Select(e => new FileRepository
            {
                FileRepositoryId = e.FileRepositoryId,
                FileName = e.FileName,
                FileType = e.FileType,
                FileStoreAt = e.FileStoreAt,
                IsFileDeleted = e.IsFileDeleted
            })
            .FirstOrDefaultAsync(cancellationToken);

    public Task<bool> FileExistsAsync(Guid fileRepositoryId, string @for, CancellationToken cancellationToken)
        => _dbContext.Set<FileRepository>()
            .AnyAsync(e =>
                e.FileRepositoryId == fileRepositoryId
                && e.For == @for, cancellationToken);
}