using OrderManagementApi.Core.Abstractions;
using OrderManagementApi.Domain.Entities;
using OrderManagementApi.Shared.Abstractions.Cache;
using OrderManagementApi.Shared.Abstractions.Clock;
using OrderManagementApi.Shared.Abstractions.Databases;
using OrderManagementApi.Shared.Abstractions.Models;
using OrderManagementApi.WebApi.Common;
using OrderManagementApi.WebApi.Dto;
using OrderManagementApi.WebApi.Endpoints.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace OrderManagementApi.IntegrationTests.Endpoints.Identity;

public class RefreshTokenTests : IClassFixture<RefreshTokenFixture>
{
    private readonly RefreshTokenFixture _serviceFixture;

    public RefreshTokenTests(RefreshTokenFixture refreshTokenFixture)
    {
        _serviceFixture = refreshTokenFixture;
    }

    public static IEnumerable<object[]> GetInvalidRequests()
    {
        yield return new object[]
        {
            // all empty
            new RefreshTokenRequest
            {
                RefreshToken = ""
            }
        };
        yield return new object[]
        {
            // all empty
            new RefreshTokenRequest
            {
                RefreshToken = string.Empty
            }
        };
    }

    [Theory]
    [MemberData(nameof(GetInvalidRequests))]
    public async Task RefreshToken_Given_InvalidRequest_ShouldReturn_BadRequest(RefreshTokenRequest request)
    {
        // Arrange
        using var scope = _serviceFixture.ServiceProvider.CreateScope();

        var refreshToken = new RefreshToken(
            scope.ServiceProvider.GetRequiredService<IDbContext>(),
            scope.ServiceProvider.GetRequiredService<IClock>(),
            scope.ServiceProvider.GetRequiredService<IUserService>(),
            scope.ServiceProvider.GetRequiredService<IAuthManager>(),
            scope.ServiceProvider.GetRequiredService<ICache>());

        // Act
        var result = await refreshToken.HandleAsync(request);

        // Assert the expected results
        result.ShouldNotBeNull();
        result.Result.ShouldBeOfType(typeof(BadRequestObjectResult));
        var actual = (result.Result as BadRequestObjectResult)!;
        actual.StatusCode.ShouldBe(400);
        actual.Value.ShouldBeOfType<Error>();
        var err = (actual.Value as Error)!;
        err.Message.ShouldBe("Invalid parameter");
    }

    [Fact]
    public async Task RefreshToken_Given_CorrectRequest_WithInvalidValue_ShouldReturn_BadRequest()
    {
        // Arrange
        using var scope = _serviceFixture.ServiceProvider.CreateScope();

        var refreshToken = new RefreshToken(
            scope.ServiceProvider.GetRequiredService<IDbContext>(),
            scope.ServiceProvider.GetRequiredService<IClock>(),
            scope.ServiceProvider.GetRequiredService<IUserService>(),
            scope.ServiceProvider.GetRequiredService<IAuthManager>(),
            scope.ServiceProvider.GetRequiredService<ICache>());

        // all empty
        var request = new RefreshTokenRequest
        {
            RefreshToken = "Hahihuheho"
        };

        // Act
        var result = await refreshToken.HandleAsync(request);
        // Assert the expected results
        result.ShouldNotBeNull();
        result.Result.ShouldBeOfType(typeof(BadRequestObjectResult));
        var actual = (result.Result as BadRequestObjectResult)!;
        actual.StatusCode.ShouldBe(400);
        actual.Value.ShouldBeOfType<Error>();
        var err = (actual.Value as Error)!;
        err.Message.ShouldBe("Invalid request");
    }

    [Fact]
    public async Task RefreshToken_Given_Correct_ShouldReturn_Ok_And_When_Retry_After_Success_ShouldReturn_BadRequest()
    {
        var refreshTokenStr = string.Empty;

        #region Sign in first

        using var tempScope = _serviceFixture.ServiceProvider.CreateScope();
        {
            var tempDbContext = tempScope.ServiceProvider.GetRequiredService<IDbContext>();
            var signIn = new SignIn(
                tempScope.ServiceProvider.GetRequiredService<IDbContext>(),
                tempScope.ServiceProvider.GetRequiredService<IUserService>(),
                tempScope.ServiceProvider.GetRequiredService<IClock>(),
                tempScope.ServiceProvider.GetRequiredService<IAuthManager>(),
                tempScope.ServiceProvider.GetRequiredService<ICache>());
            await signIn.HandleAsync(new SignInRequest
            {
                Username = "admin",
                Password = "Qwerty@1234"
            });

            refreshTokenStr = (await tempDbContext.Set<UserToken>().FirstAsync()).RefreshToken;
        }

        #endregion

        // Arrange
        using var scope = _serviceFixture.ServiceProvider.CreateScope();

        var refreshToken = new RefreshToken(
            scope.ServiceProvider.GetRequiredService<IDbContext>(),
            scope.ServiceProvider.GetRequiredService<IClock>(),
            scope.ServiceProvider.GetRequiredService<IUserService>(),
            scope.ServiceProvider.GetRequiredService<IAuthManager>(),
            tempScope.ServiceProvider.GetRequiredService<ICache>());

        // all empty
        var request = new RefreshTokenRequest
        {
            RefreshToken = refreshTokenStr
        };

        // Act
        var result = await refreshToken.HandleAsync(request);
        // Assert the expected results
        result.ShouldNotBeNull();
        result.Result.ShouldBeOfType(typeof(OkObjectResult));
        var actual = (result.Result as OkObjectResult)!;
        actual.StatusCode.ShouldBe(200);
        actual.Value.ShouldBeOfType<LoginDto>();

        // Act
        result = await refreshToken.HandleAsync(request);
        // Assert the expected results
        result.ShouldNotBeNull();
        result.Result.ShouldBeOfType(typeof(BadRequestObjectResult));
        var actual2 = (result.Result as BadRequestObjectResult)!;
        actual2.StatusCode.ShouldBe(400);
        actual2.Value.ShouldBeOfType<Error>();
        var err = (actual2.Value as Error)!;
        err.Message.ShouldBe("Invalid request");
    }
}