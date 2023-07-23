using OrderManagementApi.Core.Abstractions;
using OrderManagementApi.WebApi.Common;
using OrderManagementApi.WebApi.Dto;
using OrderManagementApi.WebApi.Endpoints.UserManagement.Scopes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace OrderManagementApi.WebApi.Endpoints.UserManagement;

public class GetUserById : BaseEndpoint<GetUserByIdRequest, UserDto>
{
    private readonly IUserService _userService;

    public GetUserById(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("users/{userId}")]
    [Authorize]
    [RequiredScope(typeof(UserManagementScope), typeof(UserManagementScopeReadOnly))]
    [SwaggerOperation(
        Summary = "Get user by id",
        Description = "",
        OperationId = "UserManagement.GetById",
        Tags = new[] { "UserManagement" })
    ]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult<UserDto>> HandleAsync([FromRoute] GetUserByIdRequest request,
        CancellationToken cancellationToken = new())
    {
        var user = await _userService.GetUserByIdAsync(request.UserId, cancellationToken);
        if (user is null)
            return BadRequest(Error.Create("Data not found"));

        var dto = new UserDto(user);

        return dto;
    }
}