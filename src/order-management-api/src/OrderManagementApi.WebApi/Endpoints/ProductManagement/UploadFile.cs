using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagementApi.Shared.Abstractions.Databases;
using OrderManagementApi.Shared.Abstractions.Files;
using OrderManagementApi.Shared.Abstractions.Models;
using OrderManagementApi.WebApi.Common;
using OrderManagementApi.WebApi.Dto;
using OrderManagementApi.WebApi.Endpoints.ProductManagement.Scopes;
using Swashbuckle.AspNetCore.Annotations;

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

    [HttpPost("products/upload-file")]
    [Authorize]
    [RequiredScope(typeof(ProductManagementScope))]
    [SwaggerOperation(
        Summary = "Upload file",
        Description = "",
        OperationId = "ProductManagement.UploadFile",
        Tags = new[] { "ProductManagement" })
    ]
    [ProducesResponseType(typeof(UploadFileDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult<UploadFileDto>> HandleAsync([FromForm] UploadFileRequest request,
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