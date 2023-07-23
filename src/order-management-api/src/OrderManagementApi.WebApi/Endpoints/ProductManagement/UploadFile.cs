using Microsoft.AspNetCore.Mvc;
using OrderManagementApi.Shared.Abstractions.Databases;
using OrderManagementApi.Shared.Abstractions.Files;
using OrderManagementApi.WebApi.Dto;

namespace OrderManagementApi.WebApi.Endpoints.ProductManagement;

public class UploadFile : BaseEndpoint<UploadFileRequest, UploadFileDto>
{
    private readonly IDbContext _dbContext;
    private readonly IFileService _fileService;

    public UploadFile(IDbContext dbContext, IFileService fileService)
    {
        _dbContext = dbContext;
        _fileService = fileService;
    }

    public override async Task<ActionResult<UploadFileDto>> HandleAsync(UploadFileRequest request,
        CancellationToken cancellationToken = new())
    {
        var file = await _fileService.UploadAsync(new FileRequest(request.File.FileName, request.File.OpenReadStream()),
            cancellationToken);

        var fileRepository =
            new Domain.Entities.FileRepository(request.File.FileName, file.NewFileName, request.File.Length)
            {
                FilePath = file.Path,
                For = "PRODUCT"
            };

        _dbContext.Insert(fileRepository);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new UploadFileDto { FileId = fileRepository.FileRepositoryId };
    }
}