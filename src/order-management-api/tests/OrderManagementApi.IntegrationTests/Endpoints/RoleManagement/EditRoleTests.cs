using OrderManagementApi.Domain.Entities;
using OrderManagementApi.Domain.Extensions;
using OrderManagementApi.Shared.Abstractions.Databases;
using OrderManagementApi.WebApi.Endpoints.RoleManagement;
using OrderManagementApi.WebApi.Endpoints.UserManagement.Scopes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace OrderManagementApi.IntegrationTests.Endpoints.RoleManagement;

[Collection(nameof(RoleManagementFixture))]
public class EditRoleTests
{
    private readonly RoleManagementFixture _serviceFixture;

    public EditRoleTests(RoleManagementFixture serviceFixture)
    {
        _serviceFixture = serviceFixture;
    }

    [Fact]
    public async Task EditRole_Given_CorrectRequest_Should_Be_Correct()
    {
        // Arrange
        using var scope = _serviceFixture.ServiceProvider.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<IDbContext>();

        var editRole = new EditRole(
            dbContext);

        var request = new EditRoleRequest
        {
            RoleId = RoleExtensions.AdministratorId,
            Payload = new EditRoleRequestPayload
            {
                Description = "Set to test",
                Scopes = new List<string>
                {
                    new UserManagementScope().ScopeName
                }
            }
        };

        // Act
        var result = await editRole.HandleAsync(request);

        // Assert the expected results
        result.ShouldNotBeNull();
        result.ShouldBeOfType(typeof(NoContentResult));

        var role = await dbContext.Set<Role>()
            .Include(e => e.RoleScopes)
            .Where(e => e.RoleId == request.RoleId)
            .FirstOrDefaultAsync(CancellationToken.None);
        role.ShouldNotBeNull();
        role.Description.ShouldBe(request.Payload.Description);

        var roleScope = role.RoleScopes.FirstOrDefault();
        roleScope.ShouldNotBeNull();
        roleScope.Name.ShouldBe(new UserManagementScope().ScopeName);
    }
}