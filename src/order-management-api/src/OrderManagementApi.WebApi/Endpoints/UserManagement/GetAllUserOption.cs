using OrderManagementApi.Shared.Abstractions.Queries;
using OrderManagementApi.WebApi.Common;
using OrderManagementApi.WebApi.Dto;
using OrderManagementApi.WebApi.Endpoints.UserManagement.Scopes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace OrderManagementApi.WebApi.Endpoints.UserManagement;

public class GetAllUserOption : BaseEndpoint<QueryOption>
{
    [HttpOptions("users")]
    [Authorize]
    [RequiredScope(typeof(UserManagementScope), typeof(UserManagementScopeReadOnly))]
    [SwaggerOperation(
        Summary = "Get user paginated options",
        Description = "",
        OperationId = "UserManagement.GetAllUserOption",
        Tags = new[] { "UserManagement" })
    ]
    [ProducesResponseType(typeof(QueryOption), StatusCodes.Status200OK)]
    public override Task<ActionResult<QueryOption>> HandleAsync(CancellationToken cancellationToken = new())
    {
        var options = new QueryOption();
        
        //setup columns
        options.Columns.Add(new Column(nameof(UserDto.Username)) { EnableOrder = true });
        options.Columns.Add(new Column(nameof(UserDto.FullName)));
        options.Columns.Add(new Column(nameof(UserDto.Email)));
        options.Columns.Add(new Column(nameof(UserDto.CreatedAt)) { EnableOrder = true });
        
        //filters
        options.Filters.Add(new Filter(nameof(UserDto.Username)));
        options.Filters.Add(new Filter(nameof(UserDto.FullName)));
        
        options.EnableGlobalSearch = false;
        
        return Task.FromResult<ActionResult<QueryOption>>(options);
    }
}