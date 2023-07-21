using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagementApi.Domain.Entities;
using OrderManagementApi.Shared.Abstractions.Databases;
using OrderManagementApi.Shared.Abstractions.Models;
using OrderManagementApi.Shared.Abstractions.Queries;
using OrderManagementApi.WebApi.Common;
using OrderManagementApi.WebApi.Dto;
using OrderManagementApi.WebApi.Endpoints.ProductManagement.Scopes;
using Swashbuckle.AspNetCore.Annotations;

namespace OrderManagementApi.WebApi.Endpoints.ProductManagement;

public class GetAllProductPaginated : BaseEndpoint<GetAllProductPaginatedRequest, PagedList<ProductManagementDto>>
{
    private readonly IDbContext _dbContext;

    public GetAllProductPaginated(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("products")]
    [Authorize]
    [RequiredScope(typeof(ProductManagementScope))]
    [SwaggerOperation(
        Summary = "Get All Product API",
        Description = "",
        OperationId = "ProductManagement.GetAllProductPaginated",
        Tags = new[] { "ProductManagement" })
    ]
    [ProducesResponseType(typeof(PagedList<ProductManagementDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult<PagedList<ProductManagementDto>>> HandleAsync(
        [FromQuery] GetAllProductPaginatedRequest request, CancellationToken cancellationToken = new())
    {
        var queryable = _dbContext.Set<Product>().AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search) && request.Search.Length > 2)
            queryable = queryable.Where(e => EF.Functions.ILike(e.Name, $"%{request.Search}%"));

        var results = await queryable
            .Select(e => new ProductManagementDto
            {
                ProductId = e.ProductId,
                Name = e.Name,
                Description = e.Description,
                Price = e.Price
            })
            .Skip(request.CalculateSkip())
            .Take(request.Size)
            .ToListAsync(cancellationToken);

        return new PagedList<ProductManagementDto>(results, request.Page, request.Size);
    }
}