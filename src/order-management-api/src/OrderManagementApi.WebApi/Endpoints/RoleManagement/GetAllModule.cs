using OrderManagementApi.Core.Modules;
using OrderManagementApi.WebApi.Common;
using OrderManagementApi.WebApi.Endpoints.RoleManagement.Scopes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace OrderManagementApi.WebApi.Endpoints.RoleManagement;

public class GetAllModule : BaseEndpoint<List<Module>>
{
    private readonly ModuleManager _moduleManager;

    public GetAllModule(ModuleManager moduleManager)
    {
        _moduleManager = moduleManager;
    }

    [HttpGet("roles/modules")]
    [Authorize]
    [RequiredScope(typeof(RoleManagementScope), typeof(RoleManagementScopeReadOnly))]
    [SwaggerOperation(
        Summary = "Get all modules API",
        Description = "",
        OperationId = "RoleManagement.GetAllModule",
        Tags = new[] { "RoleManagement" })
    ]
    [ProducesResponseType(typeof(List<Module>), StatusCodes.Status200OK)]
    public override Task<ActionResult<List<Module>>> HandleAsync(
        CancellationToken cancellationToken = new())
    {
        return Task.FromResult<ActionResult<List<Module>>>(_moduleManager.GetAllModules());
    }
}