using Microsoft.AspNetCore.Mvc;
using OrderManagementApi.Shared.Abstractions.Queries;

namespace OrderManagementApi.WebApi.Endpoints.OrderManagement;

public class GetAllOrderPaginatedRequest : BasePaginationCalculation
{
    [FromQuery(Name = "number")] public string? Number { get; set; }
    [FromQuery(Name = "startDate")] public DateTime? OrderStartDate { get; set; }
    [FromQuery(Name = "endDate")] public DateTime? OrderEndDate { get; set; }
}