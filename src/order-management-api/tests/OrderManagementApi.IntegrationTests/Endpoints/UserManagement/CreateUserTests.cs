using OrderManagementApi.Core.Abstractions;
using OrderManagementApi.Shared.Abstractions.Databases;
using OrderManagementApi.Shared.Abstractions.Encryption;
using OrderManagementApi.WebApi.Endpoints.UserManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace OrderManagementApi.IntegrationTests.Endpoints.UserManagement;

[Collection(nameof(UserManagementFixture))]
public class CreateUserTests
{
    private readonly UserManagementFixture _serviceFixture;

    public CreateUserTests(UserManagementFixture serviceFixture)
    {
        _serviceFixture = serviceFixture;
    }

    [Fact]
    public async Task CreateUser_Given_CorrectRequest_With_CorrectValue_ShouldReturn_NoContent()
    {
        // Arrange
        using var scope = _serviceFixture.ServiceProvider.CreateScope();

        var userService = scope.ServiceProvider.GetRequiredService<IUserService>();

        var createUser = new CreateUser(
            scope.ServiceProvider.GetRequiredService<IDbContext>(),
            userService,
            scope.ServiceProvider.GetRequiredService<IRng>(),
            scope.ServiceProvider.GetRequiredService<ISalter>());

        var request = new CreateUserRequest
        {
            Username = "admin2",
            Password = "Test@12345",
            Fullname = "Super Administrator",
            EmailAddress = "test@test.com"
        };

        // Act
        var result = await createUser.HandleAsync(request);

        // Assert the expected results
        result.ShouldNotBeNull();
        result.ShouldBeOfType(typeof(NoContentResult));

        var user = await userService.GetUserByUsernameAsync(request.Username, CancellationToken.None);
        user.ShouldNotBeNull();
        user.NormalizedUsername.ShouldBe(request.Username.ToUpper());
    }
}