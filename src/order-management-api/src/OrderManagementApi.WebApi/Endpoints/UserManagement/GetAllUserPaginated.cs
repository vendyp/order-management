using System.Linq.Dynamic.Core;
using OrderManagementApi.Domain.Entities;
using OrderManagementApi.Shared.Abstractions.Databases;
using OrderManagementApi.Shared.Abstractions.Queries;
using OrderManagementApi.WebApi.Common;
using OrderManagementApi.WebApi.Dto;
using OrderManagementApi.WebApi.Endpoints.UserManagement.Scopes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace OrderManagementApi.WebApi.Endpoints.UserManagement;

public class GetAllUserPaginated : BaseEndpoint<GetAllUserPaginatedRequest, PagedList<UserDto>>
{
    private readonly IDbContext _dbContext;

    public GetAllUserPaginated(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("users")]
    [Authorize]
    [RequiredScope(typeof(UserManagementScope), typeof(UserManagementScopeReadOnly))]
    [SwaggerOperation(
        Summary = "Get user paginated",
        Description = "",
        OperationId = "UserManagement.GetAll",
        Tags = new[] { "UserManagement" })
    ]
    [ProducesResponseType(typeof(PagedList<UserDto>), StatusCodes.Status200OK)]
    public override async Task<ActionResult<PagedList<UserDto>>> HandleAsync(
        [FromQuery] GetAllUserPaginatedRequest query,
        CancellationToken cancellationToken = new())
    {
        var queryable = _dbContext.Set<User>().AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Username))
            queryable = queryable.Where(e => EF.Functions.Like(e.Username, $"%{query.Username}%"));

        if (!string.IsNullOrWhiteSpace(query.Fullname))
            queryable = queryable.Where(e => EF.Functions.Like(e.FullName!, $"{query.Fullname}"));

        if (string.IsNullOrWhiteSpace(query.OrderBy))
            query.OrderBy = nameof(Domain.Entities.User.CreatedAt);

        if (string.IsNullOrWhiteSpace(query.OrderType))
            query.OrderType = "DESC";
        
        queryable = queryable.OrderBy($"{query.OrderBy} {query.OrderType}");

        var users = await queryable.Select(user => new UserDto
            {
                UserId = user.UserId,
                Username = user.Username,
                LastPasswordChangeAt = user.LastPasswordChangeAt,
                FullName = user.FullName,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                CreatedByName = user.CreatedByName,
                LastUpdatedAt = user.LastUpdatedAt,
                LastUpdatedByName = user.LastUpdatedByName
            })
            .Skip(query.CalculateSkip())
            .Take(query.Size)
            .ToListAsync(cancellationToken);

        var response = new PagedList<UserDto>(users, query.Page, query.Size);

        return response;
    }
}