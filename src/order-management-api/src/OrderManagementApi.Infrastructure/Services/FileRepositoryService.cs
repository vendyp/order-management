﻿using Microsoft.EntityFrameworkCore;
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
                Source = e.Source,
                FileType = e.FileType,
                FileStoreAt = e.FileStoreAt,
                IsFileDeleted = e.IsFileDeleted
            })
            .FirstOrDefaultAsync(cancellationToken);

    public async Task UpdateFileRepositorySourceAsync(Guid fileRepositoryId, string source,
        CancellationToken cancellationToken)
    {
        var fileRepository = await _dbContext.Set<FileRepository>()
            .Where(e => e.FileRepositoryId == fileRepositoryId)
            .Select(e => new FileRepository
            {
                FileRepositoryId = e.FileRepositoryId,
                Source = e.Source
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (fileRepository is null)
            throw new InvalidOperationException("File Repository is null");

        _dbContext.AttachEntity(fileRepository);

        fileRepository.Source = source;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}