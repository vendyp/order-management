using OrderManagementApi.Core.Abstractions;
using OrderManagementApi.Shared.Abstractions.Databases;
using OrderManagementApi.WebApi.Endpoints.UserManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace OrderManagementApi.IntegrationTests.Endpoints.UserManagement;

[Collection(nameof(UserManagementFixture))]
public class UpdateUserTests
{
    private readonly UserManagementFixture _serviceFixture;

    public UpdateUserTests(UserManagementFixture serviceFixture)
    {
        _serviceFixture = serviceFixture;
    }

    [Fact]
    public async Task UpdateUser_Given_CorrectRequest_ShouldReturn_Ok()
    {
        // Arrange
        using var scope = _serviceFixture.ServiceProvider.CreateScope();
        var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
        var user = await userService.GetUserByIdAsync(Guid.Empty, CancellationToken.None);
        user!.FullName.ShouldBe("Super Administrator"); // default value

        var updateUser = new UpdateUser(scope.ServiceProvider.GetRequiredService<IDbContext>());

        const string newFullName = "Test1234";
        // Act
        var result = await updateUser.HandleAsync(new UpdateUserRequest
        {
            UserId = Guid.Empty,
            UpdateUserRequestPayload = new UpdateUserRequestPayload() { Fullname = newFullName }
        });

        // Assert the expected results
        result.ShouldNotBeNull();
        result.ShouldBeOfType(typeof(NoContentResult));
        user = await userService.GetUserByIdAsync(Guid.Empty, CancellationToken.None);
        user!.FullName.ShouldBe(newFullName);
    }
}