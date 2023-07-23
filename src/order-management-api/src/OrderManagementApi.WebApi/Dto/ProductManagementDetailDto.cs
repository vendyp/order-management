namespace OrderManagementApi.WebApi.Dto;

public record ProductManagementDetailDto : ProductManagementDto
{
    public string? File { get; set; }
    public string? CreatedByName { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? LastUpdatedByName { get; set; }
    public DateTime? LastUpdatedAt { get; set; }
}