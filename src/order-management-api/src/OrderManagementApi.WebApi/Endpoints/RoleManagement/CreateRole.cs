using OrderManagementApi.Domain.Entities;
using OrderManagementApi.Shared.Abstractions.Databases;
using OrderManagementApi.Shared.Abstractions.Models;
using OrderManagementApi.WebApi.Common;
using OrderManagementApi.WebApi.Endpoints.RoleManagement.Scopes;
using OrderManagementApi.WebApi.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace OrderManagementApi.WebApi.Endpoints.RoleManagement;

public class CreateRole : BaseEndpointWithoutResponse<CreateRoleRequest>
{
    private readonly IDbContext _dbContext;

    public CreateRole(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPost("roles")]
    [Authorize]
    [RequiredScope(typeof(RoleManagementScope))]
    [SwaggerOperation(
        Summary = "Create role API",
        Description = "",
        OperationId = "RoleManagement.CreateRole",
        Tags = new[] { "RoleManagement" })
    ]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult> HandleAsync(CreateRoleRequest request,
        CancellationToken cancellationToken = new())
    {
        var validator = new CreateRoleRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(Error.Create("Invalid parameter", validationResult.Construct()));

        var roleIsExists = await _dbContext.Set<Role>().Where(e => e.Name == request.Name)
            .FirstOrDefaultAsync(cancellationToken);
        if (roleIsExists != null)
            return BadRequest(Error.Create($"Role name {request.Name} already exists"));

        var role = new Role(request.Name!)
        {
            Description = request.Description
        };

        if (request.Scopes.Any())
            foreach (var item in request.Scopes)
                role.RoleScopes.Add(new RoleScope(role.RoleId, item));

        if (request.Modules.Any())
            foreach (var item in request.Modules)
                role.RoleModules.Add(new RoleModule(role.RoleId, item));

        _dbContext.Insert(role);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}