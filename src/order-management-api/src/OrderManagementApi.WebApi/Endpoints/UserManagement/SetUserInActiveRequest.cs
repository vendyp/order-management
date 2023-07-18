using Microsoft.AspNetCore.Mvc;

namespace OrderManagementApi.WebApi.Endpoints.UserManagement;

public class SetUserInActiveRequest
{
    [FromRoute(Name = "userId")] public Guid UserId { get; set; }
}