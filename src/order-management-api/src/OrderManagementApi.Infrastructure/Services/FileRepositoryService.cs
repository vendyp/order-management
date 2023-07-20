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

    public Task<FileRepository?> GetFileBydId(Guid fileRepositoryId, CancellationToken cancellationToken)
        => _dbContext.Set<FileRepository>()
            .Where(e => e.FileRepositoryId == fileRepositoryId)
            .Select(e => new FileRepository
            {
                FileRepositoryId = e.FileRepositoryId,
                FileName = e.FileName,
                Source = e.Source,
                FileType = e.FileType,
                FileStoreAt = e.FileStoreAt,
                IsFileDeleted = e.IsFileDeleted
            })
            .FirstOrDefaultAsync(cancellationToken);
}