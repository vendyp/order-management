using OrderManagementApi.Shared.Abstractions.Queries;
using Microsoft.AspNetCore.Mvc;

namespace OrderManagementApi.WebApi.Endpoints.RoleManagement;

public class GetAllRolePaginatedRequest : BasePaginationCalculation
{
    [FromQuery(Name = "s")] public string? Search { get; set; }
}