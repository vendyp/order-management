using Microsoft.AspNetCore.Mvc;

namespace OrderManagementApi.WebApi.Endpoints.UserManagement;

public class UpdateUserRequest
{
    public UpdateUserRequest()
    {
        UpdateUserRequestPayload = new UpdateUserRequestPayload();
    }

    [FromRoute(Name = "userId")] public Guid UserId { get; set; }
    [FromBody] public UpdateUserRequestPayload UpdateUserRequestPayload { get; set; }
}

public class UpdateUserRequestPayload
{
    public string? Fullname { get; set; }
}