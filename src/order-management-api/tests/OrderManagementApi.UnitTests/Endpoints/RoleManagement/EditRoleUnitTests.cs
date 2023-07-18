using OrderManagementApi.Domain.Entities;
using OrderManagementApi.Domain.Extensions;
using OrderManagementApi.Shared.Abstractions.Enums;
using OrderManagementApi.Shared.Abstractions.Models;
using OrderManagementApi.UnitTests.Builders;
using OrderManagementApi.UnitTests.Builders.Entities;
using OrderManagementApi.WebApi.Endpoints.RoleManagement;
using OrderManagementApi.WebApi.Endpoints.RoleManagement.Scopes;
using OrderManagementApi.WebApi.Endpoints.UserManagement.Scopes;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;

namespace OrderManagementApi.UnitTests.Endpoints.RoleManagement;

public class EditRoleUnitTests
{
    public static IEnumerable<object[]> GetInvalidRequests()
    {
        yield return new object[]
        {
            new EditRoleRequest
            {
                RoleId = "test",
                Payload = new EditRoleRequestPayload
                {
                    Scopes = new List<string>
                    {
                        "a",
                        "b"
                    }
                }
            }
        };

        yield return new object[]
        {
            new EditRoleRequest
            {
                RoleId = "test",
                Payload = new EditRoleRequestPayload
                {
                    Scopes = new List<string>
                    {
                        RoleExtensions.AdministratorId,
                        RoleExtensions.AdministratorId,
                    }
                }
            }
        };

        yield return new object[]
        {
            new EditRoleRequest
            {
                RoleId = "test",
                Payload = new EditRoleRequestPayload
                {
                    Description = "Hello, 世界!",
                    Scopes = new List<string>
                    {
                        RoleExtensions.AdministratorId
                    }
                }
            }
        };
    }

    [Theory]
    [MemberData(nameof(GetInvalidRequests))]
    public async Task EditRole_With_InvalidRequest_Should_Return_BadRequest(EditRoleRequest request)
    {
        var dbContext = DbContextBuilder.Create();

        var editRole = new EditRole(dbContext.Object);

        var result = await editRole.HandleAsync(request, CancellationToken.None);

        // Assert the expected results
        result.ShouldNotBeNull();
        result.ShouldBeOfType(typeof(BadRequestObjectResult));
        var actual = (result as BadRequestObjectResult)!;
        actual.StatusCode.ShouldBe(400);
        actual.Value.ShouldBeOfType<Error>();
    }

    [Fact]
    public async Task EditRole_Given_Wrong_RoleId_Should_Return_BadRequest()
    {
        //assume only these within database
        var list = new List<Role>
        {
            new RoleBuilder().Create(RoleExtensions.SuperAdministratorId, RoleExtensions.SuperAdministrator)
                .SetupScopeAll().Build(),
            new RoleBuilder().Create(RoleExtensions.AdministratorId, RoleExtensions.Administrator)
                .SetupScope(new UserManagementScopeReadOnly().ScopeName)
                .SetupScope(new RoleManagementScopeReadOnly().ScopeName)
                .Build(),
        };

        var dbContext = DbContextBuilder.Create();
        dbContext.Setup(list);

        var request = new EditRoleRequest
        {
            RoleId = "abcde",
            Payload = new EditRoleRequestPayload
            {
                Description = "Set to test"
            }
        };

        var editRole = new EditRole(dbContext.Object);

        var result = await editRole.HandleAsync(request, CancellationToken.None);

        // Assert the expected results
        result.ShouldNotBeNull();
        result.ShouldBeOfType(typeof(BadRequestObjectResult));
        var actual = (result as BadRequestObjectResult)!;
        actual.StatusCode.ShouldBe(400);
        actual.Value.ShouldBeOfType<Error>();

        var err = actual.Value as Error;
        err.ShouldNotBeNull();
        err.Message.ShouldBe("Data not found");
    }

    /// <summary>
    /// Successfully update description
    /// </summary>
    [Fact]
    public async Task EditRole_Given_Correct_Should_Return_NoContent_UseCase_1()
    {
        //assume only these within database
        var list = new List<Role>
        {
            new RoleBuilder().Create(RoleExtensions.AdministratorId, RoleExtensions.Administrator)
                .SetupScope(new UserManagementScopeReadOnly().ScopeName)
                .SetupScope(new RoleManagementScopeReadOnly().ScopeName)
                .Build(),
        };

        var dbContext = DbContextBuilder.Create();

        dbContext.Setup(list);
        Role? role = null; //validates when callback hits

        //first validates return of request should be same as above
        dbContext.Setup(e => e.AttachEntity(It.IsAny<Role>()))
            .Callback<Role>((entity) =>
            {
                role = entity;
                role.RoleId.ShouldBe(RoleExtensions.AdministratorId);
                role.Name.ShouldBe(RoleExtensions.Administrator);
                role.Description.ShouldBeNull();
            });

        //remove role management scope read only
        var request = new EditRoleRequest
        {
            RoleId = RoleExtensions.AdministratorId,
            Payload = new EditRoleRequestPayload
            {
                Description = "Set to test",
                Scopes = new List<string>
                {
                    new UserManagementScopeReadOnly().ScopeName,
                    new RoleManagementScopeReadOnly().ScopeName
                }
            }
        };

        var editRole = new EditRole(dbContext.Object);

        var result = await editRole.HandleAsync(request, CancellationToken.None);

        dbContext.Verify(e => e.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

        // Assert the expected results
        result.ShouldNotBeNull();
        result.ShouldBeOfType(typeof(NoContentResult));

        //after editRole, role description should be change
        role.ShouldNotBeNull();
        role.Description.ShouldBe(request.Payload.Description);
    }

    /// <summary>
    /// Update same as use case 1 but additional test for remove one of scopes
    /// </summary>
    [Fact]
    public async Task EditRole_Given_Correct_Should_Return_NoContent_UseCase_2()
    {
        //assume only these within database
        var list = new List<Role>
        {
            new RoleBuilder().Create(RoleExtensions.AdministratorId, RoleExtensions.Administrator)
                .SetupScope(new UserManagementScopeReadOnly().ScopeName)
                .SetupScope(new RoleManagementScopeReadOnly().ScopeName)
                .Build(),
        };

        var dbContext = DbContextBuilder.Create();

        dbContext.Setup(list);

        Role? role = null; //validates when callback hits

        //first validates return of request should be same as above
        dbContext.Setup(e => e.AttachEntity(It.IsAny<Role>()))
            .Callback<Role>((entity) =>
            {
                role = entity;
                role.RoleId.ShouldBe(RoleExtensions.AdministratorId);
                role.Name.ShouldBe(RoleExtensions.Administrator);
                role.Description.ShouldBeNull();
                role.RoleScopes.Count.ShouldBe(2); // same as assumed data
            });

        //remove role management scope read only
        var request = new EditRoleRequest
        {
            RoleId = RoleExtensions.AdministratorId,
            Payload = new EditRoleRequestPayload
            {
                Description = "Set to test",
                Scopes = new List<string>
                {
                    new UserManagementScopeReadOnly().ScopeName,
                    //remove role management read only
                    //new RoleManagementScopeReadOnly().ScopeName
                }
            }
        };

        var editRole = new EditRole(dbContext.Object);

        var result = await editRole.HandleAsync(request, CancellationToken.None);

        dbContext.Verify(e => e.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

        // Assert the expected results
        result.ShouldNotBeNull();
        result.ShouldBeOfType(typeof(NoContentResult));

        //after editRole, role description should be change
        role.ShouldNotBeNull();
        role.Description.ShouldBe(request.Payload.Description);

        //after editRole, scope of role management readonly should be deleted
        role.RoleScopes.Count(e => e.IsDeleted).ShouldBe(1);
        var scope = role.RoleScopes.FirstOrDefault(e => e.IsDeleted);
        scope.ShouldNotBeNull();
        scope.Name.ShouldBe(new RoleManagementScopeReadOnly().ScopeName);
        scope.StatusRecord.ShouldBe(StatusRecord.InActive);
    }

    /// <summary>
    /// Update same as use case 2 but additional test for remove one and add one
    /// </summary>
    [Fact]
    public async Task EditRole_Given_Correct_Should_Return_NoContent_UseCase_3()
    {
        //assume only these within database
        var list = new List<Role>
        {
            new RoleBuilder().Create(RoleExtensions.AdministratorId, RoleExtensions.Administrator)
                .SetupScope(new UserManagementScopeReadOnly().ScopeName)
                .SetupScope(new RoleManagementScopeReadOnly().ScopeName)
                .Build(),
        };

        var dbContext = DbContextBuilder.Create();

        dbContext.Setup(list);

        Role? role = null; //validates when callback hits

        //first validates return of request should be same as above
        dbContext.Setup(e => e.AttachEntity(It.IsAny<Role>()))
            .Callback<Role>((entity) =>
            {
                role = entity;
                role.RoleId.ShouldBe(RoleExtensions.AdministratorId);
                role.Name.ShouldBe(RoleExtensions.Administrator);
                role.Description.ShouldBeNull();
                role.RoleScopes.Count.ShouldBe(2); // same as assumed data
            });

        //remove role management scope read only
        var request = new EditRoleRequest
        {
            RoleId = RoleExtensions.AdministratorId,
            Payload = new EditRoleRequestPayload
            {
                Description = "Set to test",
                Scopes = new List<string>
                {
                    new UserManagementScopeReadOnly().ScopeName,
                    //remove role management read only
                    //new RoleManagementScopeReadOnly().ScopeName

                    //added new one
                    new UserManagementScope().ScopeName
                }
            }
        };

        var editRole = new EditRole(dbContext.Object);

        var result = await editRole.HandleAsync(request, CancellationToken.None);

        dbContext.Verify(e => e.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

        // Assert the expected results
        result.ShouldNotBeNull();
        result.ShouldBeOfType(typeof(NoContentResult));

        //after editRole, role description should be change
        role.ShouldNotBeNull();
        role.Description.ShouldBe(request.Payload.Description);

        //after editRole, scope of role management readonly should be deleted
        role.RoleScopes.Count(e => e.IsDeleted).ShouldBe(1);
        var scope = role.RoleScopes.FirstOrDefault(e => e.IsDeleted);
        scope.ShouldNotBeNull();
        scope.Name.ShouldBe(new RoleManagementScopeReadOnly().ScopeName);
        scope.StatusRecord.ShouldBe(StatusRecord.InActive);

        //after editRole, new scope added, role management scope read only
        var newScope = role.RoleScopes.FirstOrDefault(e => e.Name == new UserManagementScope().ScopeName);
        newScope.ShouldNotBeNull();
        newScope.Name.ShouldBe(new UserManagementScope().ScopeName);
        newScope.StatusRecord.ShouldBe(StatusRecord.Active);
        
        role.RoleScopes.Count(e => !e.IsDeleted).ShouldBe(2);
        role.RoleScopes.Count.ShouldBe(3);
    }
}