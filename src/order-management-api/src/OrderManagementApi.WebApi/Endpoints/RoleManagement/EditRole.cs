﻿using OrderManagementApi.Domain.Entities;
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

public class EditRole : BaseEndpointWithoutResponse<EditRoleRequest>
{
    private readonly IDbContext _dbContext;

    public EditRole(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpPut("roles/{roleId}")]
    [Authorize]
    [RequiredScope(typeof(RoleManagementScope))]
    [SwaggerOperation(
        Summary = "Edit role API",
        Description = "",
        OperationId = "RoleManagement.EditRole",
        Tags = new[] { "RoleManagement" })
    ]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult> HandleAsync([FromRoute] EditRoleRequest request,
        CancellationToken cancellationToken = new())
    {
        var validator = new EditRoleRequestPayloadValidator();
        var validationResult = await validator.ValidateAsync(request.Payload, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(Error.Create("Invalid parameter", validationResult.Construct()));

        var role = await _dbContext.Set<Role>()
            .Include(e => e.RoleScopes)
            .Include(e => e.RoleModules)
            .Where(e => e.RoleId == request.RoleId)
            .FirstOrDefaultAsync(cancellationToken);

        if (role is null)
            return BadRequest(Error.Create("Data not found"));

        _dbContext.AttachEntity(role);

        if (request.Payload.Description != role.Description)
            role.Description = request.Payload.Description;

        foreach (var item in role.RoleScopes)
        {
            if (!request.Payload.Scopes.Any(e => e == item.Name))
                item.SetToDeleted();
        }

        foreach (var item in request.Payload.Scopes)
        {
            if (!role.RoleScopes.Any(e => e.Name == item))
                role.RoleScopes.Add(new RoleScope(role.RoleId, item));
        }

        foreach (var item in role.RoleModules)
        {
            if (!request.Payload.Modules.Any(e => e == item.Name))
                item.SetToDeleted();
        }

        foreach (var item in request.Payload.Modules)
        {
            if (!role.RoleModules.Any(e => e.Name == item))
                role.RoleModules.Add(new RoleModule(role.RoleId, item));
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}