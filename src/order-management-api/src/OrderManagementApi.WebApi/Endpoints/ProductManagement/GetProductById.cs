using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagementApi.Domain.Entities;
using OrderManagementApi.Shared.Abstractions.Databases;
using OrderManagementApi.Shared.Abstractions.Models;
using OrderManagementApi.WebApi.Common;
using OrderManagementApi.WebApi.Dto;
using OrderManagementApi.WebApi.Endpoints.ProductManagement.Scopes;
using Swashbuckle.AspNetCore.Annotations;

namespace OrderManagementApi.WebApi.Endpoints.ProductManagement;

public class GetProductById : BaseEndpoint<GetProductByIdRequest, ProductManagementDetailDto>
{
    private readonly IDbContext _dbContext;

    public GetProductById(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("products/{productId}")]
    [Authorize]
    [RequiredScope(typeof(ProductManagementScope))]
    [SwaggerOperation(
        Summary = "Get Product by Id",
        Description = "",
        OperationId = "ProductManagement.GetProductById",
        Tags = new[] { "ProductManagement" })
    ]
    [ProducesResponseType(typeof(ProductManagementDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult<ProductManagementDetailDto>> HandleAsync(
        [FromRoute] GetProductByIdRequest request, CancellationToken cancellationToken = new())
    {
        var product = await _dbContext.Set<Product>()
            .Where(e => e.ProductId == request.ProductId)
            .FirstOrDefaultAsync(cancellationToken);
        if (product is null)
            return BadRequest(Error.Create("Product not found"));

        var dto = new ProductManagementDetailDto
        {
            ProductId = product.ProductId,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            File = product.Image,
            CreatedAt = product.CreatedAt,
            CreatedByName = product.CreatedByName,
            LastUpdatedAt = product.LastUpdatedAt,
            LastUpdatedByName = product.LastUpdatedByName
        };

        return dto;
    }
}