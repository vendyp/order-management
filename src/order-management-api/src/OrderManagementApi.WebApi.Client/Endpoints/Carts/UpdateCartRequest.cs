using Microsoft.AspNetCore.Mvc;

namespace OrderManagementApi.WebApi.Client.Endpoints.Carts;

public class UpdateCartRequest
{
    [FromRoute(Name = "cartId")] public Guid CartId { get; set; }
    [FromBody] public UpdateCartRequestPayload Payload { get; set; } = null!;
}

public class UpdateCartRequestPayload
{
    public int Quantity { get; set; }
}