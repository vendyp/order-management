using OrderManagementApi.Shared.Abstractions.Databases;
using OrderManagementApi.Shared.Abstractions.Files;
using OrderManagementApi.WebApi.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace OrderManagementApi.WebApi.Endpoints.FileRepository;

public class UploadFile : BaseEndpoint<UploadFileRequest, UploadFileDto>
{
    private readonly IDbContext _dbContext;
    private readonly IFileService _fileService;

    public UploadFile(IDbContext dbContext, IFileService fileService)
    {
        _dbContext = dbContext;
        _fileService = fileService;
    }

    [HttpPost]
    [Authorize]
    [SwaggerOperation(
        Summary = "File repository upload",
        Description = "",
        OperationId = "FileRepository.Upload",
        Tags = new[] { "FileRepository" })
    ]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public override async Task<ActionResult<UploadFileDto>> HandleAsync([FromForm] UploadFileRequest request,
        CancellationToken cancellationToken = new())
    {
        var file = await _fileService.UploadAsync(new FileRequest(request.File.FileName, request.File.OpenReadStream()),
            cancellationToken);

        //default value of fileRepository FileStoreAt is filesystem,
        //then by default, if the implementation is change, you must update this and add
        //FileStoreAt into something that is related to the current implementations
        //such as FileStoreAt.AzureBlob next to Source = request.Source
        var fileRepository =
            new Domain.Entities.FileRepository(request.File.FileName, file.NewFileName, request.File.Length)
            {
                Source = request.Source
            };

        _dbContext.Insert(fileRepository);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new UploadFileDto { FileId = fileRepository.FileRepositoryId };
    }
}