using OrderManagementApi.Domain.Entities;
using OrderManagementApi.Shared.Abstractions.Databases;
using OrderManagementApi.Shared.Abstractions.Models;
using OrderManagementApi.WebApi.Endpoints.RoleManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace OrderManagementApi.IntegrationTests.Endpoints.RoleManagement;

[Collection(nameof(RoleManagementFixture))]
public class CreateRoleTests
{
    private readonly RoleManagementFixture _serviceFixture;

    public CreateRoleTests(RoleManagementFixture serviceFixture)
    {
        _serviceFixture = serviceFixture;
    }

    [Fact]
    public async Task CreateRole_Given_CorrectRequest_With_RoleAlreadyExists_ShouldReturn_BadRequest()
    {
        // Arrange
        using var scope = _serviceFixture.ServiceProvider.CreateScope();

        var createUser = new CreateRole(
            scope.ServiceProvider.GetRequiredService<IDbContext>());

        var request = new CreateRoleRequest
        {
            Name = "Administrator"
        };

        // Act
        var result = await createUser.HandleAsync(request, CancellationToken.None);

        // Assert the expected results
        result.ShouldNotBeNull();
        result.ShouldBeOfType(typeof(BadRequestObjectResult));
        var actual = (result as BadRequestObjectResult)!;
        actual.StatusCode.ShouldBe(400);
        actual.Value.ShouldBeOfType<Error>();
    }

    [Fact]
    public async Task CreateRole_Given_CorrectRequest_ShouldReturn_NoContent()
    {
        // Arrange
        using var scope = _serviceFixture.ServiceProvider.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<IDbContext>();

        var createUser = new CreateRole(dbContext);

        var request = new CreateRoleRequest
        {
            Name = "Staff"
        };

        // Act
        var result = await createUser.HandleAsync(request, CancellationToken.None);

        // Assert the expected results
        result.ShouldNotBeNull();
        result.ShouldBeOfType(typeof(NoContentResult));

        var role = await dbContext.Set<Role>().Where(e => e.Name == request.Name)
            .FirstOrDefaultAsync(CancellationToken.None);
        role.ShouldNotBeNull();
    }
}