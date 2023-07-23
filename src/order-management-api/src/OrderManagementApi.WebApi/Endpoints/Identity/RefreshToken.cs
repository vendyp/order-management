using OrderManagementApi.Core.Abstractions;
using OrderManagementApi.Domain.Entities;
using OrderManagementApi.Shared.Abstractions.Cache;
using OrderManagementApi.Shared.Abstractions.Clock;
using OrderManagementApi.Shared.Abstractions.Databases;
using OrderManagementApi.WebApi.Common;
using OrderManagementApi.WebApi.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagementApi.WebApi.Shared.Helpers;
using OrderManagementApi.WebApi.Shared.Validators;
using Swashbuckle.AspNetCore.Annotations;

namespace OrderManagementApi.WebApi.Endpoints.Identity;

public class RefreshToken : BaseEndpoint<RefreshTokenRequest, LoginDto>
{
    private readonly IDbContext _dbContext;
    private readonly IClock _clock;
    private readonly IUserService _userService;
    private readonly IAuthManager _authManager;
    private readonly ICache _cache;

    public RefreshToken(IDbContext dbContext, IClock clock, IUserService userService, IAuthManager authManager,
        ICache cache)
    {
        _dbContext = dbContext;
        _clock = clock;
        _userService = userService;
        _authManager = authManager;
        _cache = cache;
    }

    [HttpPost("refresh")]
    [SwaggerOperation(
        Summary = "Refresh token",
        Description = "",
        OperationId = "Identity.Refresh",
        Tags = new[] { "Identity" })
    ]
    [ProducesResponseType(typeof(LoginDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public override async Task<ActionResult<LoginDto>> HandleAsync(RefreshTokenRequest request,
        CancellationToken cancellationToken = new())
    {
        var validator = new RefreshTokenRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return BadRequest(Error.Create("Invalid parameter", validationResult.Construct()));

        var userToken = await _dbContext.Set<UserToken>()
            .Where(e => e.RefreshToken == request.RefreshToken)
            .Select(e => new UserToken
            {
                UserTokenId = e.UserTokenId,
                RefreshToken = e.RefreshToken,
                IsUsed = e.IsUsed,
                ExpiryAt = e.ExpiryAt
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (userToken is null || userToken.IsUsed || userToken.ExpiryAt < _clock.CurrentDate())
            return BadRequest(Error.Create("Invalid request"));

        _dbContext.AttachEntity(userToken);

        userToken.IsUsed = true;

        await _cache.DeleteAsync(userToken.UserTokenId.ToString("N"));

        var tsExpiry = new TimeSpan(0, 7, 0, 0);

        var newUserToken = new UserToken
        {
            UserId = userToken.UserId,
            RefreshToken = Guid.NewGuid().ToString("N"),
            ExpiryAt = _clock.CurrentDate().Add(tsExpiry),
        };

        _dbContext.Set<UserToken>().Add(newUserToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        var user = await _userService.GetUserByIdAsync(userToken.UserId, cancellationToken);

        var claims = ClaimsGenerator.Generate(user!);

        var token = _authManager.CreateToken(newUserToken.UserTokenId.ToString("N"), _authManager.Options.Audience,
            claims);

        var dto = new LoginDto(user!)
        {
            AccessToken = token.AccessToken,
            Expiry = token.Expiry,
            RefreshToken = newUserToken.RefreshToken
        };

        await _cache.SetAsync(newUserToken.UserTokenId.ToString("N"), claims, tsExpiry);

        return Ok(dto);
    }
}