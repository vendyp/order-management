using Microsoft.AspNetCore.Mvc;
using OrderManagementApi.Shared.Abstractions.Queries;

namespace OrderManagementApi.WebApi.Endpoints.ProductManagement;

public class GetAllProductPaginatedRequest : BasePaginationCalculation
{
    [FromQuery(Name = "s")] public string? Search { get; set; }
}