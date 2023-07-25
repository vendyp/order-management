using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagementApi.Domain.Entities;
using OrderManagementApi.Shared.Abstractions.Contexts;
using OrderManagementApi.Shared.Abstractions.Databases;
using OrderManagementApi.WebApi.Client.Common;
using Swashbuckle.AspNetCore.Annotations;

namespace OrderManagementApi.WebApi.Client.Endpoints.Carts;

public class AddCart : BaseEndpointWithoutResponse<AddCartRequest>
{
    private readonly IDbContext _dbContext;
    private readonly IContext _context;

    public AddCart(IDbContext dbContext, IContext context)
    {
        _dbContext = dbContext;
        _context = context;
    }

    [HttpPost]
    [Authorize]
    [RequiredScope(typeof(UserScope))]
    [SwaggerOperation(
        Summary = "Add product to cart",
        Description = "",
        OperationId = "Carts.AddCart",
        Tags = new[] { "Carts" })
    ]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult> HandleAsync([FromBody] AddCartRequest request,
        CancellationToken cancellationToken = new())
    {
        var productIsExist = await _dbContext.Set<Product>()
            .AnyAsync(e => e.ProductId == request.ProductId, cancellationToken);
        if (!productIsExist)
            return BadRequest(Error.Create("Invalid product"));

        var cart = await _dbContext.Set<Cart>()
            .Where(e => e.UserId == _context.Identity.Id && e.ProductId == request.ProductId)
            .FirstOrDefaultAsync(cancellationToken);

        if (cart is null)
        {
            cart = new Cart
            {
                UserId = _context.Identity.Id,
                ProductId = request.ProductId,
                Quantity = request.Quantity
            };

            _dbContext.Insert(cart);
        }
        else
        {
            _dbContext.AttachEntity(cart);
            cart.Quantity += request.Quantity;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}