using OrderManagementApi.Core.Abstractions;
using OrderManagementApi.Domain.Entities;
using OrderManagementApi.Shared.Abstractions.Databases;
using OrderManagementApi.Shared.Abstractions.Encryption;
using OrderManagementApi.WebApi.Common;
using OrderManagementApi.WebApi.Endpoints.UserManagement.Scopes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagementApi.WebApi.Shared.Validators;
using Swashbuckle.AspNetCore.Annotations;

namespace OrderManagementApi.WebApi.Endpoints.UserManagement;

public class CreateUser : BaseEndpointWithoutResponse<CreateUserRequest>
{
    private readonly IDbContext _dbContext;
    private readonly IUserService _userService;
    private readonly IRng _rng;
    private readonly ISalter _salter;

    public CreateUser(IDbContext dbContext, IUserService userService, IRng rng, ISalter salter)
    {
        _dbContext = dbContext;
        _userService = userService;
        _rng = rng;
        _salter = salter;
    }

    [HttpPost("users")]
    [Authorize]
    [RequiredScope(typeof(UserManagementScope))]
    [SwaggerOperation(
        Summary = "Create user API",
        Description = "",
        OperationId = "UserManagement.CreateUser",
        Tags = new[] { "UserManagement" })
    ]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult> HandleAsync(CreateUserRequest request,
        CancellationToken cancellationToken = new())
    {
        var validator = new CreateUserRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(Error.Create("Invalid parameter", validationResult.Construct()));

        var userExist = await _userService.IsUsernameExistAsync(request.Username!, cancellationToken);
        if (userExist)
            return BadRequest(Error.Create("Username already exists"));

        var salt = _rng.Generate(128, false);

        var user = new User
        {
            Username = request.Username!,
            NormalizedUsername = request.Username!.ToUpper(),
            Salt = salt,
            Password = _salter.Hash(salt, request.Password!),
            LastPasswordChangeAt = DateTime.UtcNow,
            FullName = request.Fullname
        };

        _dbContext.Insert(user);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}