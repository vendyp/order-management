using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagementApi.Core.Abstractions;
using OrderManagementApi.Domain.Entities;
using OrderManagementApi.Shared.Abstractions.Databases;
using OrderManagementApi.Shared.Abstractions.Models;
using OrderManagementApi.WebApi.Common;
using OrderManagementApi.WebApi.Endpoints.ProductManagement.Scopes;
using OrderManagementApi.WebApi.Shared.Validators;
using Swashbuckle.AspNetCore.Annotations;

namespace OrderManagementApi.WebApi.Endpoints.ProductManagement;

public class CreateProduct : BaseEndpointWithoutResponse<CreateProductRequest>
{
    private readonly IDbContext _dbContext;
    private readonly IFileRepository _fileRepository;

    public CreateProduct(IDbContext dbContext, IFileRepository fileRepository)
    {
        _dbContext = dbContext;
        _fileRepository = fileRepository;
    }

    [HttpPost("products")]
    [Authorize]
    [RequiredScope(typeof(ProductManagementScope))]
    [SwaggerOperation(
        Summary = "Create Product API",
        Description = "",
        OperationId = "ProductManagement.CreateProduct",
        Tags = new[] { "ProductManagement" })
    ]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult> HandleAsync(CreateProductRequest request,
        CancellationToken cancellationToken = new())
    {
        var validator = new CreateProductRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(Error.Create("Invalid parameter", validationResult.Construct()));

        var fileRepository = await _fileRepository.GetFileBydIdAsync(new Guid(request.File!), cancellationToken);
        if (fileRepository is null)
            return BadRequest("Invalid request");

        _dbContext.AttachEntity(fileRepository);

        var newProduct = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price!.Value
        };

        fileRepository.Source = newProduct.ProductId.ToString();

        _dbContext.Insert(newProduct);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}