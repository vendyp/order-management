using OrderManagementApi.Domain.Entities;
using OrderManagementApi.Domain.Extensions;
using OrderManagementApi.Shared.Abstractions.Models;
using OrderManagementApi.UnitTests.Builders;
using OrderManagementApi.WebApi.Endpoints.RoleManagement;
using OrderManagementApi.WebApi.Endpoints.RoleManagement.Scopes;
using OrderManagementApi.WebApi.Endpoints.UserManagement.Scopes;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;

namespace OrderManagementApi.UnitTests.Endpoints.RoleManagement;

public class CreateRoleUnitTests
{
    public static IEnumerable<object[]> GetInvalidRequests()
    {
        yield return new object[]
        {
            new CreateRoleRequest
            {
                Name = string.Empty
            }
        };

        yield return new object[]
        {
            new CreateRoleRequest
            {
                Name = ""
            }
        };

        yield return new object[]
        {
            new CreateRoleRequest
            {
                Name = "Staff",
                Scopes = new List<string>
                {
                    "LorepIpsumDolor",
                }
            }
        };
    }

    [Theory]
    [MemberData(nameof(GetInvalidRequests))]
    public async Task CreateRole_Given_InvalidRequest_ShouldReturn_BadRequest(CreateRoleRequest request)
    {
        var createRole = new CreateRole(DbContextBuilder.Create().Object);
        var result = await createRole.HandleAsync(request);

        // Assert the expected results
        result.ShouldNotBeNull();
        result.ShouldBeOfType(typeof(BadRequestObjectResult));
        var actual = (result as BadRequestObjectResult)!;
        actual.StatusCode.ShouldBe(400);
        actual.Value.ShouldBeOfType<Error>();
    }

    [Fact]
    public async Task CreateRole_Should_Do_Correctly()
    {
        var dbContext = DbContextBuilder.Create();

        var list = new List<Role>
        {
            new("superadministrator", RoleExtensions.SuperAdministrator),
            new("administrator", RoleExtensions.Administrator)
        };

        dbContext.Setup(list);

        var createRole = new CreateRole(dbContext.Object);

        var request = new CreateRoleRequest
        {
            Name = "Staff",
            Description = "Test desc"
        };
        request.Scopes.Add(new UserManagementScope().ScopeName);
        request.Scopes.Add(new RoleManagementScope().ScopeName);

        //validates callback role that passed to insert, must satisfied those requirements
        dbContext.Setup(e => e.Insert(It.IsAny<Role>()))
            .Callback<Role>((entity) =>
            {
                entity.RoleId.ShouldBe(request.Name.ToLower());
                entity.Name.ShouldBe(request.Name);
                entity.Description.ShouldBe(request.Description);

                entity.RoleScopes.Count.ShouldBe(request.Scopes.Count);
                entity.RoleScopes.Select(e => e.Name).ShouldBe(request.Scopes);
            });

        _ = await createRole.HandleAsync(request, CancellationToken.None);

        dbContext.Verify(e => e.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}