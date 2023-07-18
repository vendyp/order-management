using OrderManagementApi.Core.Abstractions;
using OrderManagementApi.Shared.Abstractions.Cache;
using OrderManagementApi.Shared.Abstractions.Clock;
using OrderManagementApi.Shared.Abstractions.Databases;
using OrderManagementApi.Shared.Abstractions.Models;
using OrderManagementApi.WebApi.Common;
using OrderManagementApi.WebApi.Dto;
using OrderManagementApi.WebApi.Endpoints.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace OrderManagementApi.IntegrationTests.Endpoints.Identity;

public class SignInTests : IClassFixture<SignInFixture>
{
    public SignInFixture ServiceFixture { get; }

    public SignInTests(SignInFixture serviceFixture)
    {
        ServiceFixture = serviceFixture;
    }

    /// <summary>
    /// Given :
    /// Invalid request payload
    ///
    /// Expected :
    /// Return bad request with value object of type <see cref="Error"/>
    /// </summary>
    [Fact]
    public async Task SignIn_Given_InvalidRequest_ShouldReturn_BadRequest()
    {
        // Arrange
        using var scope = ServiceFixture.ServiceProvider.CreateScope();

        // Act
        var request = new SignInRequest
        {
            Username = string.Empty,
            Password = string.Empty
        };

        var signIn = new SignIn(
            scope.ServiceProvider.GetRequiredService<IDbContext>(),
            scope.ServiceProvider.GetRequiredService<IUserService>(),
            scope.ServiceProvider.GetRequiredService<IClock>(),
            scope.ServiceProvider.GetRequiredService<IAuthManager>(),
            scope.ServiceProvider.GetRequiredService<ICache>());

        var result = await signIn.HandleAsync(request);

        // Assert the expected results
        result.Result.ShouldNotBeNull();
        result.Result.ShouldBeOfType(typeof(BadRequestObjectResult));
        var actual = (result.Result! as BadRequestObjectResult)!;
        actual.StatusCode.ShouldBe(400);
        actual.Value.ShouldBeOfType<Error>();
    }

    /// <summary>
    /// Given :
    /// Correct request payload
    ///
    /// Expected :
    /// Return bad request with value object of type <see cref="Error"/>
    /// </summary>
    [Fact]
    public async Task SignIn_Given_CorrectRequest_WithInvalidUsernameOrPassword_ShouldReturn_BadRequest()
    {
        // Arrange
        using var scope = ServiceFixture.ServiceProvider.CreateScope();

        // Act
        var request = new SignInRequest
        {
            Username = "lorep",
            Password = "doloripsum"
        };

        var signIn = new SignIn(
            scope.ServiceProvider.GetRequiredService<IDbContext>(),
            scope.ServiceProvider.GetRequiredService<IUserService>(),
            scope.ServiceProvider.GetRequiredService<IClock>(),
            scope.ServiceProvider.GetRequiredService<IAuthManager>(),
            scope.ServiceProvider.GetRequiredService<ICache>());

        var result = await signIn.HandleAsync(request);

        // Assert the expected results
        result.Result.ShouldNotBeNull();
        result.Result.ShouldBeOfType(typeof(BadRequestObjectResult));
        var actual = (result.Result! as BadRequestObjectResult)!;
        actual.StatusCode.ShouldBe(400);
        actual.Value.ShouldBeOfType<Error>();
    }

    /// <summary>
    /// Given :
    /// Correct request payload
    ///
    /// Expected :
    /// Return Ok.
    /// </summary>
    [Fact]
    public async Task SignIn_Given_CorrectRequest_WithCorrectUsernameAndPassword_ShouldReturn_LoginDto()
    {
        // Arrange
        using var scope = ServiceFixture.ServiceProvider.CreateScope();

        // Act
        var request = new SignInRequest
        {
            Username = "admin",
            Password = "Qwerty@1234"
        };

        var signIn = new SignIn(
            scope.ServiceProvider.GetRequiredService<IDbContext>(),
            scope.ServiceProvider.GetRequiredService<IUserService>(),
            scope.ServiceProvider.GetRequiredService<IClock>(),
            scope.ServiceProvider.GetRequiredService<IAuthManager>(),
            scope.ServiceProvider.GetRequiredService<ICache>());

        var result = await signIn.HandleAsync(request);

        result.Result.ShouldNotBeNull();
        result.Result.ShouldBeOfType(typeof(OkObjectResult));
        var actual = (result.Result! as OkObjectResult)!;
        actual.Value.ShouldNotBeNull();
        actual.Value.ShouldBeOfType<LoginDto>();

        var dto = (actual.Value! as LoginDto)!;
        dto.UserId.ShouldBe(default);
        dto.AccessToken.ShouldNotBeNullOrWhiteSpace();
        dto.RefreshToken.ShouldNotBeNullOrWhiteSpace();
        dto.Expiry.ShouldBeGreaterThan(0);
    }
}