using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagementApi.WebApi.Common;
using OrderManagementApi.WebApi.Endpoints.UserManagement.Scopes;
using OrderManagementApi.WebApi.Shared.Abstractions;
using Swashbuckle.AspNetCore.Annotations;

namespace OrderManagementApi.WebApi.Endpoints.UserManagement;

public class GetAllScope : BaseEndpoint<List<string>>
{
    private readonly IScopeManager _scopeManager;

    public GetAllScope(IScopeManager scopeManager)
    {
        _scopeManager = scopeManager;
    }

    [HttpGet("users/scopes")]
    [Authorize]
    [RequiredScope(typeof(UserManagementScope), typeof(UserManagementScopeReadOnly))]
    [SwaggerOperation(
        Summary = "Get all scope API",
        Description = "",
        OperationId = "UserManagement.GetAllScope",
        Tags = new[] { "UserManagement" })
    ]
    [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
    public override Task<ActionResult<List<string>>> HandleAsync(CancellationToken cancellationToken = new())
    {
        return Task.FromResult<ActionResult<List<string>>>(_scopeManager.GetAllScopes());
    }
}