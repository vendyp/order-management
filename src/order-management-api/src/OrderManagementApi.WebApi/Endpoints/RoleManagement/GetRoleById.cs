using OrderManagementApi.Domain.Entities;
using OrderManagementApi.Shared.Abstractions.Databases;
using OrderManagementApi.Shared.Abstractions.Models;
using OrderManagementApi.WebApi.Common;
using OrderManagementApi.WebApi.Dto;
using OrderManagementApi.WebApi.Endpoints.RoleManagement.Scopes;
using OrderManagementApi.WebApi.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace OrderManagementApi.WebApi.Endpoints.RoleManagement;

public class GetRoleById : BaseEndpoint<GetRoleByIdRequest, RoleDetailDto>
{
    private readonly IDbContext _dbContext;

    public GetRoleById(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("roles/{roleId}")]
    [Authorize]
    [RequiredScope(typeof(RoleManagementScope), typeof(RoleManagementScopeReadOnly))]
    [SwaggerOperation(
        Summary = "Get roles by ID",
        Description = "",
        OperationId = "RoleManagement.GetRoleById",
        Tags = new[] { "RoleManagement" })
    ]
    [ProducesResponseType(typeof(RoleDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult<RoleDetailDto>> HandleAsync([FromRoute] GetRoleByIdRequest request,
        CancellationToken cancellationToken = new())
    {
        var validator = new GetRoleByIdRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(Error.Create("Invalid parameter", validationResult.Construct()));

        var role = await _dbContext.Set<Role>()
            .Include(e => e.RoleScopes)
            .Where(e => e.RoleId == request.RoleId)
            .FirstOrDefaultAsync(cancellationToken);
        if (role is null)
            return BadRequest(Error.Create("Data not found"));

        var dto = new RoleDetailDto(role.RoleId, role.Name, role.Description);

        foreach (var item in role.RoleScopes)
            dto.Scopes.Add(new ScopeDto(item.Name));

        return dto;
    }
}