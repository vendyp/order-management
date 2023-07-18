using Microsoft.AspNetCore.Mvc;

namespace OrderManagementApi.WebApi.Endpoints.UserManagement;

public class GetAllRoleRequest
{
    [FromQuery(Name = "s")] public string? Search { get; set; }
}