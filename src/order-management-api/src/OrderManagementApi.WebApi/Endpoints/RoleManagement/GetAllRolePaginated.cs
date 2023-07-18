using System.Linq.Dynamic.Core;
using OrderManagementApi.Domain.Entities;
using OrderManagementApi.Domain.Extensions;
using OrderManagementApi.Shared.Abstractions.Databases;
using OrderManagementApi.Shared.Abstractions.Queries;
using OrderManagementApi.WebApi.Common;
using OrderManagementApi.WebApi.Dto;
using OrderManagementApi.WebApi.Endpoints.RoleManagement.Scopes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace OrderManagementApi.WebApi.Endpoints.RoleManagement;

public class GetAllRolePaginated : BaseEndpoint<GetAllRolePaginatedRequest, PagedList<RoleDto>>
{
    private readonly IDbContext _dbContext;

    public GetAllRolePaginated(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("roles")]
    [Authorize]
    [RequiredScope(typeof(RoleManagementScope), typeof(RoleManagementScopeReadOnly))]
    [SwaggerOperation(
        Summary = "Get roles paginated",
        Description = "",
        OperationId = "RoleManagement.GetAll",
        Tags = new[] { "RoleManagement" })
    ]
    [ProducesResponseType(typeof(PagedList<UserDto>), StatusCodes.Status200OK)]
    public override async Task<ActionResult<PagedList<RoleDto>>> HandleAsync(
        [FromQuery] GetAllRolePaginatedRequest request,
        CancellationToken cancellationToken = new())
    {
        var queryable = _dbContext.Set<Role>().AsQueryable()
            .Where(e => e.RoleId != RoleExtensions.SuperAdministratorId);

        if (!string.IsNullOrWhiteSpace(request.Search) && request.Search.Length > 2)
            queryable = queryable.Where(e => EF.Functions.Like(e.Name, $"%{request.Search}%"));

        if (string.IsNullOrWhiteSpace(request.OrderBy))
            request.OrderBy = nameof(Role.CreatedAt);

        if (string.IsNullOrWhiteSpace(request.OrderType))
            request.OrderType = "DESC";

        queryable = queryable.OrderBy($"{request.OrderBy} {request.OrderType}");

        var users = await queryable
            .Select(user => new RoleDto(user.RoleId, user.Name, user.Description))
            .Skip(request.CalculateSkip())
            .Take(request.Size)
            .ToListAsync(cancellationToken);

        var response = new PagedList<RoleDto>(users, request.Page, request.Size);

        return response;
    }
}