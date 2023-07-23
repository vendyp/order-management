using OrderManagementApi.Core.Abstractions;
using OrderManagementApi.Shared.Abstractions.Clock;
using OrderManagementApi.Shared.Abstractions.Contexts;
using OrderManagementApi.Shared.Abstractions.Databases;
using OrderManagementApi.Shared.Abstractions.Encryption;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagementApi.WebApi.Shared.Validators;
using Swashbuckle.AspNetCore.Annotations;

namespace OrderManagementApi.WebApi.Endpoints.Identity;

public class ChangePassword : BaseEndpointWithoutResponse<ChangePasswordRequest>
{
    private readonly IUserService _userService;
    private readonly ISalter _salter;
    private readonly IClock _clock;
    private readonly IDbContext _dbContext;
    private readonly IRng _rng;
    private readonly IContext _context;

    public ChangePassword(IUserService userService,
        ISalter salter,
        IClock clock,
        IDbContext dbContext,
        IRng rng,
        IContext context)
    {
        _userService = userService;
        _salter = salter;
        _clock = clock;
        _dbContext = dbContext;
        _rng = rng;
        _context = context;
    }

    [HttpPut("password")]
    [SwaggerOperation(
        Summary = "Change Password",
        Description = "Change Password API dedicated for identity",
        OperationId = "Identity.ChangePassword",
        Tags = new[] { "Identity" })
    ]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult> HandleAsync([FromBody] ChangePasswordRequest request,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var validator = new ChangePasswordRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(Error.Create("Invalid parameter", validationResult.Construct()));

        var user = await _userService.GetUserByIdAsync(_context.Identity.Id, cancellationToken);
        if (user?.Password is null)
            return BadRequest(Error.Create("Data not found"));

        if (!_userService.VerifyPassword(user.Password!, user.Salt!, request.CurrentPassword!))
            return BadRequest(Error.Create("Invalid password"));

        _dbContext.AttachEntity(user);

        user.Salt = _rng.Generate(128);
        user.Password = _salter.Hash(user.Salt, request.NewPassword!);
        user.LastPasswordChangeAt = _clock.CurrentDate();

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Ok();
    }
}