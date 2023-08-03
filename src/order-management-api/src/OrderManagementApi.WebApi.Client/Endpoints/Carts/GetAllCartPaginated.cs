using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagementApi.Domain.Entities;
using OrderManagementApi.Shared.Abstractions.Contexts;
using OrderManagementApi.Shared.Abstractions.Databases;
using OrderManagementApi.Shared.Abstractions.Queries;
using OrderManagementApi.WebApi.Client.Common;
using OrderManagementApi.WebApi.Client.Dto;
using Swashbuckle.AspNetCore.Annotations;

namespace OrderManagementApi.WebApi.Client.Endpoints.Carts;

public class GetAllCartPaginated : BaseEndpoint<GetAllCartPaginatedRequest, PagedList<CartDto>>
{
    private readonly IDbContext _dbContext;
    private readonly IContext _context;

    public GetAllCartPaginated(IDbContext dbContext, IContext context)
    {
        _dbContext = dbContext;
        _context = context;
    }

    [HttpGet]
    [Authorize]
    [RequiredScope(typeof(UserScope))]
    [SwaggerOperation(
        Summary = "Get all cart by user",
        Description = "",
        OperationId = "Carts.GetAllCartPaginated",
        Tags = new[] { "Carts" })
    ]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult<PagedList<CartDto>>> HandleAsync(GetAllCartPaginatedRequest request,
        CancellationToken cancellationToken = new())
    {
        var queryable = _dbContext.Set<Cart>()
            .Include(e => e.Product)
            .AsQueryable();

        queryable = queryable.Where(e => e.UserId == _context.Identity.Id);

        queryable = queryable.OrderByDescending(e => e.CreatedAt);

        var results = await queryable
            .Select(e => new CartDto
            {
                CartId = e.CartId,
                Quantity = e.Quantity,
                ProductName = e.Product!.Name,
                Image = e.Product!.Image,
                Price = e.Product!.Price
            })
            .Skip(request.CalculateSkip())
            .Take(request.Size)
            .ToListAsync(cancellationToken);

        var vm = new PagedList<CartDto>(results, request.Page, request.Size);

        return vm;
    }
}