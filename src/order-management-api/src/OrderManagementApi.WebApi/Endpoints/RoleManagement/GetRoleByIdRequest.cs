using Microsoft.AspNetCore.Mvc;

namespace OrderManagementApi.WebApi.Endpoints.RoleManagement;

public class GetRoleByIdRequest
{
    [FromRoute(Name = "roleId")] public string RoleId { get; set; } = null!;
}