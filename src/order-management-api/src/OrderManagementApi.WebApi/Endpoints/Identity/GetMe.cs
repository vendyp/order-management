using OrderManagementApi.Core.Abstractions;
using OrderManagementApi.Core.Modules;
using OrderManagementApi.Domain.Extensions;
using OrderManagementApi.Shared.Abstractions.Contexts;
using OrderManagementApi.Shared.Abstractions.Models;
using OrderManagementApi.WebApi.Dto;
using OrderManagementApi.WebApi.Scopes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace OrderManagementApi.WebApi.Endpoints.Identity;

public class GetMe : BaseEndpoint<UserDto>
{
    private readonly IContext _context;
    private readonly IUserService _userService;
    private readonly ModuleManager _moduleManager;

    public GetMe(IContext context, IUserService userService, ModuleManager moduleManager)
    {
        _context = context;
        _userService = userService;
        _moduleManager = moduleManager;
    }

    [HttpGet("me")]
    [SwaggerOperation(
        Summary = "Get me",
        Description = "Get profile by user id of its user",
        OperationId = "Identity.GetMe",
        Tags = new[] { "Identity" })
    ]
    [Authorize]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult<UserDto>> HandleAsync(
        CancellationToken cancellationToken = new())
    {
        var user = await _userService.GetUserByIdAsync(_context.Identity.Id, cancellationToken);
        if (user is null)
            return BadRequest(Error.Create("Data not found"));

        var dto = new UserDto
        {
            UserId = user.UserId,
            Username = user.Username,
            FullName = user.FullName,
            LastPasswordChangeAt = user.LastPasswordChangeAt,
            Email = user.Email,

            CreatedAt = user.CreatedAt,
            CreatedByName = user.CreatedByName,

            LastUpdatedAt = user.LastUpdatedAt,
            LastUpdatedByName = user.LastUpdatedByName
        };
        dto.Scopes.AddRange(user.UserRoles.Select(e => e.Role).SelectMany(e => e!.RoleScopes).Select(e => e.Name));
        dto.Modules.AddRange(user.UserRoles.Select(e => e.Role).SelectMany(e => e!.RoleModules).Select(e => e.Name));

        if (!user.UserRoles.Any(e => e.RoleId == RoleExtensions.SuperAdministratorId))
            return Ok(dto);

        dto.Scopes.Clear();
        dto.Modules.Clear();

        dto.Scopes.AddRange(ScopeManager.Instance.GetAllScopes());
        dto.Modules.AddRange(_moduleManager.GetAllModuleName());

        return Ok(dto);
    }
}