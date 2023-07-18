using Microsoft.AspNetCore.Mvc;

namespace OrderManagementApi.WebApi.Endpoints.RoleManagement;

public class EditRoleRequest
{
    [FromRoute(Name = "roleId")] public string RoleId { get; set; } = null!;
    [FromBody] public EditRoleRequestPayload Payload { get; set; } = null!;
}