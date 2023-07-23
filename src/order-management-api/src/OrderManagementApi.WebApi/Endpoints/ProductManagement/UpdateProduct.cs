using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagementApi.Core.Abstractions;
using OrderManagementApi.Domain.Entities;
using OrderManagementApi.Shared.Abstractions.Databases;
using OrderManagementApi.WebApi.Common;
using OrderManagementApi.WebApi.Endpoints.ProductManagement.Scopes;
using OrderManagementApi.WebApi.Shared.Validators;
using Swashbuckle.AspNetCore.Annotations;

namespace OrderManagementApi.WebApi.Endpoints.ProductManagement;

public class UpdateProduct : BaseEndpointWithoutResponse<UpdateProductRequest>
{
    private readonly IDbContext _dbContext;
    private readonly IFileRepository _fileRepository;

    public UpdateProduct(IDbContext dbContext, IFileRepository fileRepository)
    {
        _dbContext = dbContext;
        _fileRepository = fileRepository;
    }

    [HttpPut("products/{productId}")]
    [Authorize]
    [RequiredScope(typeof(ProductManagementScope))]
    [SwaggerOperation(
        Summary = "Update Product API",
        Description = "",
        OperationId = "ProductManagement.UpdateProduct",
        Tags = new[] { "ProductManagement" })
    ]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult> HandleAsync([FromRoute] UpdateProductRequest request,
        CancellationToken cancellationToken = new())
    {
        var validator = new UpdateProductRequestValidator();
        var validationResult = await validator.ValidateAsync(request.Payload, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(Error.Create("Invalid parameter", validationResult.Construct()));

        var product = await _dbContext.Set<Product>()
            .Where(e => e.ProductId == request.ProductId)
            .FirstOrDefaultAsync(cancellationToken);
        if (product is null)
            return BadRequest(Error.Create("Product not found"));

        if (!string.IsNullOrWhiteSpace(request.Payload.File))
            if (!await _fileRepository.FileExistsAsync(new Guid(request.Payload.File!), "PRODUCT", cancellationToken))
                return BadRequest(Error.Create("Invalid request"));

        _dbContext.AttachEntity(product);

        if (request.Payload.Name != product.Name)
            product.Name = request.Payload.Name!;

        if (request.Payload.Description != product.Description)
            product.Description = request.Payload.Description;

        if (!string.IsNullOrWhiteSpace(request.Payload.File))
            product.Image = request.Payload.File;

        if (product.Price != request.Payload.Price)
            product.Price = request.Payload.Price!.Value;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}