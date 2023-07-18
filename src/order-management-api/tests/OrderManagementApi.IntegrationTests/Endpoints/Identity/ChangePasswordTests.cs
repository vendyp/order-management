using OrderManagementApi.Core.Abstractions;
using OrderManagementApi.IntegrationTests.Dependencies;
using OrderManagementApi.Shared.Abstractions.Clock;
using OrderManagementApi.Shared.Abstractions.Databases;
using OrderManagementApi.Shared.Abstractions.Encryption;
using OrderManagementApi.Shared.Abstractions.Models;
using OrderManagementApi.WebApi.Endpoints.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace OrderManagementApi.IntegrationTests.Endpoints.Identity;

public class ChangePasswordTests : IClassFixture<ChangePasswordFixture>
{
    private readonly ChangePasswordFixture _serviceFixture;

    public ChangePasswordTests(ChangePasswordFixture serviceFixture)
    {
        _serviceFixture = serviceFixture;
    }

    public static IEnumerable<object[]> GetInvalidRequests()
    {
        yield return new object[]
        {
            // all empty
            new ChangePasswordRequest
            {
                CurrentPassword = "",
                NewPassword = ""
            }
        };
        yield return new object[]
        {
            // current and new same value
            new ChangePasswordRequest
            {
                CurrentPassword = "Qwerty@12345",
                NewPassword = "Qwerty@12345"
            }
        };
        yield return new object[]
        {
            new ChangePasswordRequest
            {
                CurrentPassword = "Qwerty@12345",
                NewPassword = ""
            }
        };
        yield return new object[]
        {
            new ChangePasswordRequest
            {
                CurrentPassword = "",
                NewPassword = "Qwerty@12345"
            }
        };
    }

    [Theory]
    [MemberData(nameof(GetInvalidRequests))]
    public async Task ChangePassword_Given_InvalidRequest_ShouldReturn_BadRequest(ChangePasswordRequest request)
    {
        // Arrange
        using var scope = _serviceFixture.ServiceProvider.CreateScope();

        var changePassword = new ChangePassword(
            scope.ServiceProvider.GetRequiredService<IUserService>(),
            scope.ServiceProvider.GetRequiredService<ISalter>(),
            scope.ServiceProvider.GetRequiredService<IClock>(),
            scope.ServiceProvider.GetRequiredService<IDbContext>(),
            scope.ServiceProvider.GetRequiredService<IRng>(),
            new Context(Guid.Empty));

        // Act
        var result = await changePassword.HandleAsync(request);

        // Assert the expected results
        result.ShouldNotBeNull();
        result.ShouldBeOfType(typeof(BadRequestObjectResult));
        var actual = (result as BadRequestObjectResult)!;
        actual.StatusCode.ShouldBe(400);
        actual.Value.ShouldBeOfType<Error>();
    }

    /// <summary>
    /// This may not be happening because Identity.Id will always validated through authorization
    /// </summary>
    [Fact]
    public async Task ChangePassword_Given_CorrectRequest_WithInvalidInjector_ShouldReturn_BadRequest()
    {
        // Arrange
        using var scope = _serviceFixture.ServiceProvider.CreateScope();

        // Act
        var request = new ChangePasswordRequest
        {
            CurrentPassword = "Qwerytgsdgw", //default password should be Qwerty@1234
            NewPassword = "Qwerty@12345"
        };

        var changePassword = new ChangePassword(
            scope.ServiceProvider.GetRequiredService<IUserService>(),
            scope.ServiceProvider.GetRequiredService<ISalter>(),
            scope.ServiceProvider.GetRequiredService<IClock>(),
            scope.ServiceProvider.GetRequiredService<IDbContext>(),
            scope.ServiceProvider.GetRequiredService<IRng>(),
            new Context(Guid.NewGuid()));

        var result = await changePassword.HandleAsync(request);

        // Assert the expected results
        result.ShouldNotBeNull();
        result.ShouldBeOfType(typeof(BadRequestObjectResult));
    }

    [Fact]
    public async Task ChangePassword_Given_CorrectRequest_WithInvalidPassword_ShouldReturn_BadRequest()
    {
        // Arrange
        using var scope = _serviceFixture.ServiceProvider.CreateScope();

        // Act
        var request = new ChangePasswordRequest
        {
            CurrentPassword = "Qwerytgsdgw", //default password should be Qwerty@1234
            NewPassword = "Qwerty@12345"
        };

        var changePassword = new ChangePassword(
            scope.ServiceProvider.GetRequiredService<IUserService>(),
            scope.ServiceProvider.GetRequiredService<ISalter>(),
            scope.ServiceProvider.GetRequiredService<IClock>(),
            scope.ServiceProvider.GetRequiredService<IDbContext>(),
            scope.ServiceProvider.GetRequiredService<IRng>(),
            new Context(Guid.Empty));

        var result = await changePassword.HandleAsync(request);

        // Assert the expected results
        result.ShouldNotBeNull();
        result.ShouldBeOfType(typeof(BadRequestObjectResult));
        var actual = (result as BadRequestObjectResult)!;
        actual.StatusCode.ShouldBe(400);
        actual.Value.ShouldBeOfType<Error>();
        var obj = (actual.Value as Error);
        obj.ShouldNotBeNull();
        obj.Message.ShouldBe("Invalid password");
    }

    [Fact]
    public async Task ChangePassword_Given_CorrectRequest_WithCorrectPassword_ShouldReturn_Ok()
    {
        // Arrange
        using var scope = _serviceFixture.ServiceProvider.CreateScope();

        // Act
        var request = new ChangePasswordRequest
        {
            CurrentPassword = "Qwerty@1234", //default password should be Qwerty@1234
            NewPassword = "Qwerty@12345"
        };

        var changePassword = new ChangePassword(
            scope.ServiceProvider.GetRequiredService<IUserService>(),
            scope.ServiceProvider.GetRequiredService<ISalter>(),
            scope.ServiceProvider.GetRequiredService<IClock>(),
            scope.ServiceProvider.GetRequiredService<IDbContext>(),
            scope.ServiceProvider.GetRequiredService<IRng>(),
            new Context(Guid.Empty));

        var result = await changePassword.HandleAsync(request);

        // Assert the expected results
        result.ShouldNotBeNull();
        result.ShouldBeOfType(typeof(OkResult));
        var actual = (result as OkResult)!;
        actual.StatusCode.ShouldBe(200);
    }
}