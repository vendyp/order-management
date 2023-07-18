using OrderManagementApi.Domain.Entities;
using OrderManagementApi.Shared.Abstractions.Databases;
using OrderManagementApi.WebApi.Common;
using OrderManagementApi.WebApi.Dto;
using OrderManagementApi.WebApi.Endpoints.UserManagement.Scopes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace OrderManagementApi.WebApi.Endpoints.UserManagement;

public class GetAllRole : BaseEndpoint<GetAllRoleRequest, List<RoleDto>>
{
    private readonly IDbContext _dbContext;

    public GetAllRole(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("users/roles")]
    [Authorize]
    [RequiredScope(typeof(UserManagementScope), typeof(UserManagementScopeReadOnly))]
    [SwaggerOperation(
        Summary = "Get roles",
        Description = "",
        OperationId = "UserManagement.GetAllRole",
        Tags = new[] { "UserManagement" })
    ]
    [ProducesResponseType(typeof(List<RoleDto>), StatusCodes.Status200OK)]
    public override async Task<ActionResult<List<RoleDto>>> HandleAsync([FromQuery] GetAllRoleRequest request,
        CancellationToken cancellationToken = new())
    {
        var queryable = _dbContext.Set<Role>().AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search) && request.Search.Length > 2)
            queryable = queryable.Where(e => EF.Functions.Like(e.Name, $"%{request.Search}%"));

        var data = await queryable.Select(e => new RoleDto(e.RoleId, e.Name, e.Description))
            .ToListAsync(cancellationToken);

        return data;
    }
}