using OrderManagementApi.WebApi.Common;
using OrderManagementApi.WebApi.Dto;
using OrderManagementApi.WebApi.Endpoints.RoleManagement.Scopes;
using OrderManagementApi.WebApi.Scopes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace OrderManagementApi.WebApi.Endpoints.RoleManagement;

public class GetAllScope : BaseEndpoint<List<ScopeDto>>
{
    [HttpGet("roles/scopes")]
    [Authorize]
    [RequiredScope(typeof(RoleManagementScope), typeof(RoleManagementScopeReadOnly))]
    [SwaggerOperation(
        Summary = "Get all scopes API",
        Description = "",
        OperationId = "RoleManagement.GetAllScope",
        Tags = new[] { "RoleManagement" })
    ]
    [ProducesResponseType(typeof(List<ScopeDto>), StatusCodes.Status200OK)]
    public override Task<ActionResult<List<ScopeDto>>> HandleAsync(
        CancellationToken cancellationToken = new())
    {
        var list = new List<ScopeDto>();

        foreach (var item in ScopeManager.Instance.GetAllScopes())
            list.Add(new ScopeDto(item));

        return Task.FromResult<ActionResult<List<ScopeDto>>>(list);
    }
}