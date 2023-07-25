namespace OrderManagementApi.WebApi.Client.Endpoints.Carts;

public class AddProductRequest
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}