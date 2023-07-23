using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagementApi.Domain.Entities;
using OrderManagementApi.Shared.Abstractions.Databases;
using OrderManagementApi.Shared.Abstractions.Models;
using OrderManagementApi.WebApi.Common;
using OrderManagementApi.WebApi.Endpoints.ProductManagement.Scopes;
using Swashbuckle.AspNetCore.Annotations;

namespace OrderManagementApi.WebApi.Endpoints.ProductManagement;

public class DeleteProduct : BaseEndpointWithoutResponse<DeleteProductRequest>
{
    private readonly IDbContext _dbContext;

    public DeleteProduct(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpDelete("products/{productId}")]
    [Authorize]
    [RequiredScope(typeof(ProductManagementScope))]
    [SwaggerOperation(
        Summary = "Delete Product API",
        Description = "",
        OperationId = "ProductManagement.DeleteProduct",
        Tags = new[] { "ProductManagement" })
    ]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult> HandleAsync([FromRoute] DeleteProductRequest request,
        CancellationToken cancellationToken = new())
    {
        var product = await _dbContext.Set<Product>()
            .Where(e => e.ProductId == request.ProductId)
            .Select(e => new Product
            {
                ProductId = e.ProductId
            }).FirstOrDefaultAsync(cancellationToken);
        if (product is null)
            return BadRequest(Error.Create("Data not found"));

        _dbContext.AttachEntity(product);

        product.SetToDeleted();

        await _dbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}