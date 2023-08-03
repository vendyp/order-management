namespace OrderManagementApi.WebApi.Client.Dto;

public record CartDto
{
    public Guid CartId { get; set; }
    public int Quantity { get; set; }
    public string? ProductName { get; set; }
    public string? Image { get; set; }
    public decimal? Price { get; set; }
}