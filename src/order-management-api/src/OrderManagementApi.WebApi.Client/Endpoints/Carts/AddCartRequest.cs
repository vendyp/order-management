namespace OrderManagementApi.WebApi.Client.Endpoints.Carts;

public class AddCartRequest
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}