using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagementApi.Domain.Entities;
using OrderManagementApi.Shared.Abstractions.Contexts;
using OrderManagementApi.Shared.Abstractions.Databases;
using OrderManagementApi.WebApi.Client.Common;
using Swashbuckle.AspNetCore.Annotations;

namespace OrderManagementApi.WebApi.Client.Endpoints.Carts;

public class UpdateCart : BaseEndpointWithoutResponse<UpdateCartRequest>
{
    private readonly IDbContext _dbContext;
    private readonly IContext _context;

    public UpdateCart(IDbContext dbContext, IContext context)
    {
        _dbContext = dbContext;
        _context = context;
    }

    [HttpPut("{cartId}")]
    [Authorize]
    [RequiredScope(typeof(UserScope))]
    [SwaggerOperation(
        Summary = "Update product to cart",
        Description = "",
        OperationId = "Carts.UpdateCart",
        Tags = new[] { "Carts" })
    ]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult> HandleAsync([FromRoute] UpdateCartRequest request,
        CancellationToken cancellationToken = new())
    {
        var cart = await _dbContext.Set<Cart>()
            .Where(e => e.CartId == request.CartId)
            .Select(e => new Cart
            {
                CartId = e.CartId,
                UserId = e.UserId,
                //Quantity = e.Quantity
            })
            .FirstOrDefaultAsync(cancellationToken);
        if (cart is null || cart.UserId != _context.Identity.Id)
            return BadRequest(Error.Create("Cart not found"));

        _dbContext.AttachEntity(cart);

        cart.Quantity = request.Payload.Quantity;

        return NoContent();
    }
}